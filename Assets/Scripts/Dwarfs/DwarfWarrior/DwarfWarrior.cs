using RSG.Muffin.MatrixModule.Core.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class DwarfWarrior : DwarfBase
{
    public TeamType Type { get; set; }
    private Vector2Int _currentPosition;
    private List<Vector2Int> _path = new();
    private IMatrix<OreBase> _matrixMap;
    private MapGenerator _mapGenerator;
    private MapModel _mapModel;
    private FundController _fundController;
    private bool _isStepActive;
    private bool _isPathPassed;
    private bool _isGold;
    private bool _isGoHomeWithGold;
    private Vector2Int _spawnPosition;
    private bool _isGoldInBag;
    private bool _isFight;
    private bool _isRun;
    private bool _isAttack;
    private DwarfAnimationType _currentAnimation;
    [SerializeField] private Animator _animator;
    private float _level = 0;

    public float Level
    {
        get =>
            _level;
        set
        {
            _level = value;
        }
    }

    public bool IsFight { 
        get =>
            _isFight;
        set
        {
            _isFight = value;
        }
    }

    public bool IsGoHomeWithGold
    {
        get =>
            _isGoHomeWithGold;
        set
        {
            _isGoHomeWithGold = value;
        }
    }

    public bool IsPassPassed
    {
        get =>
            _isPathPassed;
        set
        {
            _isPathPassed = value;
        }
    }


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
        _path = _matrixMap.PathFinding(_currentPosition, GetRandomPoint(), x => x.GetType() == typeof(MinedOre));
    }

    private Vector2Int GetRandomPoint() // else
    {
        int pointCount = Random.Range(0, _mapModel.MinedOres.Count);
        return _mapModel.MinedOres[pointCount];
    }

    private void SetPathToHome()
    {
        _path.Clear();
        _path = _matrixMap.PathFinding(_currentPosition, _spawnPosition, x => x.GetType() == typeof(MinedOre));
    }

    private void GoToHome()
    {
        SetPathToHome();
        MoveToPoint(_matrixMap, _path);
    }


    public virtual void MoveToPoint(IMatrix<OreBase> mapMatrix, List<Vector2Int> path)
    {
        _matrixMap = mapMatrix;
        StartCoroutine(PathMover(path));
    }
    private void FixedUpdate()
    {
        if (_isPathPassed)
        {
            if (!_isGoHomeWithGold)
            {
                SetRandomPath();
                MoveToPoint(_matrixMap, _path);
            }
            else
            {
                GoToHome();
            }
            
        }
        

        if (_currentPosition == _spawnPosition)
        {
            if (_isGoHomeWithGold)
            {
                _isGoHomeWithGold = false;
                _fundController.IncreaseFund(Type);
            }
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

    private IEnumerator PathMover(List<Vector2Int> path) //else
    {
        _isPathPassed = false;
        foreach (Vector2Int step in path)
        {
            if (_isFight)
            {
                yield break;
            }

            if (_isGold)
            {
                _isGold = false;
                _isGoHomeWithGold = true;
                break;
            }

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(MoveForOneCell(0.5f, step));
        }
        _isPathPassed = true;
    }

    private IEnumerator MoveForOneCell(float duration, Vector2Int step)
    {
        _isRun = true;
        _isStepActive = true;
        Vector3 newPosition = new Vector3(step.x, step.y, 0);
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;

        if(startingPos.x > step.x) // גûםוסעט ג מעהוכüםûי סונגטס
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if(startingPos.x < step.x)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        while (elapsedTime < duration)
        {
            transform.position = Vector3.MoveTowards(startingPos, newPosition, (elapsedTime / duration) * Vector3.Distance(startingPos, newPosition));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = newPosition;
        _currentPosition = step;
        _isStepActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isFight)
        {
            if (other.gameObject.layer == 7)
            {
                DwarfMiner dwarf = other.gameObject.GetComponent<DwarfMiner>();
                if (dwarf.Type != Type)
                {
                    _isGold = dwarf.ScaryGoHome();
                    _isGoldInBag = _isGold;
                }
            }

            if (other.gameObject.layer == 6)
            {
                DwarfWarrior dwarf = other.gameObject.GetComponent<DwarfWarrior>();
                if (dwarf != null && dwarf.Type != Type && !dwarf.IsFight)
                {
                    _isFight = true;
                    StartCoroutine(BattleWithDwarf(dwarf));
                }
            }
        }
        
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator BattleWithDwarf(DwarfWarrior otherDwarf)
    {
        _isRun = false; 
        otherDwarf.IsFight = true;

        yield return new WaitForSeconds(4.0f);
        bool thisDwarfWins;
        if (_level == otherDwarf.Level)
        {
            thisDwarfWins = Random.value > 0.5f;
        }
        else
        {
            thisDwarfWins = _level > otherDwarf.Level;
        }


        if (thisDwarfWins)
        {
            _level++;
            otherDwarf.IsGoHomeWithGold = false;
            _isPathPassed = true;
            _isGoHomeWithGold = true;
            otherDwarf.Die();
        }
        else
        {
            otherDwarf.Level++;
            otherDwarf.IsGoHomeWithGold = true;
            otherDwarf.IsPassPassed = true;
            _isGoHomeWithGold = false;
            Die();
        }
        _isFight = false;
        otherDwarf.IsFight = false;
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
        PlayAnimations(DwarfAnimationType.Attack, IsFight);
    }


}
