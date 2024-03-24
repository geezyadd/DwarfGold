using RSG.Muffin.MatrixModule.Core.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DwarfMiner : DwarfBase
{
    private Vector2Int _spawnPosition;
    private Vector2Int _currentPosition;
    private List<Vector2Int> _path = new();

    private Matrix<OreBase> _matrixMap;
    private MapGenerator _mapGenerator;
    private bool _isStepActive;
    private bool _isPathPassed;
    private bool _isRun;
    private bool _isAttack;
    [SerializeField] private float _moveToOneCellTime;
    [SerializeField] private float _mineCellTime;


    private DwarfAnimationType _currentAnimation;
    [SerializeField] private Animator _animator;

    [Inject]
    public void Construct(MapGenerator mapGenerator)
    {
        _mapGenerator = mapGenerator;
    }

    public void Start()
    {
        _spawnPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        _currentPosition = _spawnPosition;
        _matrixMap = _mapGenerator.GetMapMatrix();
        SetRandomPath();
        if(_path.Count != 0)
        {
            MoveToPoint(_matrixMap, _path);
        }
        
    }

    private void SetRandomPath()
    {
        _path.Clear();
        _path = _matrixMap.PathFinding(_currentPosition, GetRandomPoint(), x => x.GetType() != typeof(BedRockOre), true);
        if (_path.Count == 0)
        {
            SetRandomPath();
        }
    }

    private void SetPathToHome()
    {
        _path.Clear();
        _path = _matrixMap.PathFinding(_currentPosition, _spawnPosition, x => x.GetType() == typeof(MinedOre));
        if (_path.Count == 0)
        {
            SetPathToHome();
        }
    }

    private Vector2Int GetRandomPoint()
    {
        int rows = _matrixMap.Rows.Count;
        int cols = _matrixMap.Rows[0].Data.Count;
        return new Vector2Int(Random.Range(0, cols), Random.Range(0, rows));
    }

    private bool PointValidator(Vector2Int point)
    {
        if (_matrixMap.GetValue(point.x, point.y).GetType() == typeof(BedRockOre))
        {
            return false;
        }

        return true;
    }

    public virtual void MoveToPoint(Matrix<OreBase> mapMatrix, List<Vector2Int> path)
    {
        _matrixMap = mapMatrix;
        StartCoroutine(PathMover(path));
    }

    private void GoToSpawnPosition()
    {
        SetPathToHome();
        MoveToPoint(_matrixMap, _path);
    }
    private IEnumerator PathMover(List<Vector2Int> path)
    {
        Vector2Int lastStep = _currentPosition;
        _isPathPassed = false;
        foreach (Vector2Int step in path)
        {
            if (_matrixMap.GetValue(lastStep.x, lastStep.y).GetType() == typeof(GoldOre))
            {
                _isPathPassed = true;
            }
            else if (_matrixMap.GetValue(step.x, step.y).GetType() == typeof(MinedOre))
            {
                lastStep = step;
                yield return new WaitForSeconds(_moveToOneCellTime);
                StartCoroutine(MoveForOneCell(_moveToOneCellTime, step));
            }
            else
            {
                lastStep = step;
                _isAttack = true;
                yield return new WaitForSeconds(_mineCellTime);
                _isAttack = false;
                StartCoroutine(MoveForOneCell(_moveToOneCellTime, step));
            }
        }
        if (!_isPathPassed)
        {
            _isPathPassed = true;
            SetRandomPath();
            MoveToPoint(_matrixMap, _path);
        }
        else
        {
            GoToSpawnPosition();
        }

    }

    private void SetStepPassed(Vector2Int step)
    {
        _mapGenerator.SetOreMined(step);
    }

    private IEnumerator MoveForOneCell(float duration, Vector2Int step)
    {
        Vector3 newPosition = new Vector3(step.x, step.y, 0);
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;

        if (startingPos.x > step.x) // גûםוסעט ג מעהוכüםûי סונגטס
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (startingPos.x < step.x)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        _isRun = true;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.MoveTowards(startingPos, newPosition, (elapsedTime / duration) * Vector3.Distance(startingPos, newPosition));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isRun = false;
        transform.position = newPosition;
        _currentPosition = step;
        SetStepPassed(step);
        _isStepActive = false;
    }

    private void PlayAnimations(DwarfAnimationType animationType, bool active)
    {
        if (!active)
        {
            if (_currentAnimation == DwarfAnimationType.Idle || _currentAnimation != animationType)
            {
                return;
            }
            _currentAnimation = DwarfAnimationType.Idle;
            PlayAnimation(_currentAnimation);
            return;
        }
        if (_currentAnimation > animationType)
        {
            return;
        }
        _currentAnimation = animationType;
        PlayAnimation(_currentAnimation);
    }
    private void PlayAnimation(DwarfAnimationType animationType)
    {
        _animator.SetInteger(nameof(DwarfAnimationType), (int)animationType);
    }
    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        PlayAnimations(DwarfAnimationType.Run, _isRun);
        PlayAnimations(DwarfAnimationType.Attack, _isAttack);
    }
}
