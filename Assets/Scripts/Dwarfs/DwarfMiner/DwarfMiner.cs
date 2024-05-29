using RSG.Muffin.MatrixModule.Core.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DwarfMiner : DwarfBase
{
    public TeamType Type { get; set; }
    private Vector2Int _spawnPosition;
    private Vector2Int _currentPosition;
    private List<Vector2Int> _path = new();
    private IMatrix<OreBase> _matrixMap;
    private MapGenerator _mapGenerator;
    private MapModel _mapModel;
    private FundController _fundController;
    private bool _isStepActive;
    private bool _isPathPassed;
    private bool _isRun;
    private bool _isAttack;
    [SerializeField] private float _moveToOneCellTime;
    [SerializeField] private float _mineCellTime;
    private DwarfAnimationType _currentAnimation;
    [SerializeField] private Animator _animator;
    private List<Vector2Int> _points;
    private bool _isPathActive;
    private bool _isScaryGoHome;
    private bool _isGoldInBag;

    [Inject]
    public void Construct(MapGenerator mapGenerator, MapModel mapModel, FundController fundController)
    {
        _mapGenerator = mapGenerator;
        _mapModel = mapModel;
        _fundController = fundController;
    }

    public void Start()
    {
        _spawnPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        _currentPosition = _spawnPosition;
        _matrixMap = _mapGenerator.GetMapMatrix();
        SetRandomPath();
        MoveToPoint(_matrixMap, _path);
        
    }

    private void SetRandomPath()
    {
        _path.Clear();
        _path = _matrixMap.PathFinding(_currentPosition, GetRandomPoint(), x => x.GetType() != typeof(BedRockOre));
    }

    private void SetPathToHome()
    {
        _path.Clear();
        _path = _matrixMap.PathFinding(_currentPosition, _spawnPosition, x => x.GetType() == typeof(MinedOre));
    }

    private Vector2Int GetRandomPoint()
    {
        List<Vector2Int> points = new();
        foreach (Vector2Int point in _mapModel.NotMinedOres)
        {
            if (_mapGenerator.GetMapMatrix().GetValue(point.x, point.y).GetType() == typeof(GoldOre)) {  points.Add(point); }
        }
        int randomIndex = Random.Range(0, points.Count);

        if(points.Count == 0)
        {
            return _spawnPosition;
        }

        return points[randomIndex];
    }
    

    public virtual void MoveToPoint(IMatrix<OreBase> mapMatrix, List<Vector2Int> path)
    {
        _matrixMap = mapMatrix;
        StartCoroutine(PathMover(path));
    }

    private void GoToSpawnPosition()
    {
        SetPathToHome();
        MoveToPoint(_matrixMap, _path);
    }

    private void FixedUpdate()
    {
        if (!_isPathActive)
        {
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


        if (_currentPosition == _spawnPosition && _isGoldInBag)
        {
            _isGoldInBag = false;
            _fundController.IncreaseFund(Type);
        }

        if (_isGoldInBag)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
            
        }
        else
        {
            if (Type == TeamType.Red)
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    private IEnumerator PathMover(List<Vector2Int> path)
    {
        _isPathActive = true;
        if (path.Count == 0)
        {
            GoToSpawnPosition();
            yield break;
        }
        
        _isPathPassed = false;
        Vector2Int lastStep = _currentPosition;
        foreach (Vector2Int step in path)
        {
            if (_isScaryGoHome)
            {
                _isGoldInBag = false;
                _isPathPassed = true;
                _isScaryGoHome = false;
                break;
            }

            if (_matrixMap.GetValue(lastStep.x, lastStep.y).GetType() == typeof(GoldOre))
            {
                _isGoldInBag = true;
                _isPathPassed = true;
                break;
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
        _isPathActive = false;
    }

    private void SetStepPassed(Vector2Int step)
    {
        _mapGenerator.SetSpriteOreMined(step);
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

    public bool ScaryGoHome()
    {
        _isScaryGoHome = true;
        if (!_isPathActive)
        {
            GoToSpawnPosition();
        }
        return _isGoldInBag;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_fundController.IsTeamsMeets)
        {
            if (other.gameObject.layer == 7)
            {
                DwarfMiner dwarf = other.gameObject.GetComponent<DwarfMiner>();
                if (dwarf.Type != Type)
                {
                    _fundController.IsTeamsMeets = true;
                }
            }
        }
    }

}
