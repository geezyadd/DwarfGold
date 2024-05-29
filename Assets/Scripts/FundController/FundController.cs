using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class FundController : MonoBehaviour
{
    [SerializeField] private int _dwarfCost;
    [SerializeField] private int _maxDwarfMinerCount;
    private bool _redHiring;
    private bool _blueHiring;
    private DwarfFactory _factory;
    private MapModel _mapModel;
    private bool _startSpawn;
    private int _currentRedFund = 0;
    private int _totalRedFund = 0;
    private int _currentBlueFund = 0;
    private int _totalBlueFund = 0;
    private int _redCount;
    private int _blueCount;
    private bool _isEndGame;
    private TeamType _winner;

    public bool IsTeamsMeets { get; set; }
    public int TotalRedFund
    {
        get =>
            _totalRedFund;
    }

    public TeamType Winner
    {
        get =>
            _winner;
    }

    public int TotalBlueFund
    {
        get =>
            _totalBlueFund;
    }

    public bool IsEndGame
    {
        get =>
            _isEndGame;
    }

    public int CurrentRedFund
    {
        get =>
            _currentRedFund;
    }

    public int CurrentBlueFund
    {
        get =>
            _currentBlueFund;
    }

    public void IncreaseFund(TeamType type)
    {
        if (type == TeamType.Red)
        {
            _currentRedFund++;
            _totalRedFund++;
        }

        if (type == TeamType.Blue)
        {
            _currentBlueFund++;
            _totalBlueFund++;
        }
        CheckGold();
    }

    [Inject]
    private void InjectDependencies(DwarfFactory factory, MapModel mapModel)
    {
        _factory = factory;
        _mapModel = mapModel;
    }


    void Start()
    {
        StartCoroutine(nameof(StartSpawn));
    }

    IEnumerator StartSpawn()
    {
       _factory.CreateDwarfMiner(TeamType.Red);
       _factory.CreateDwarfMiner(TeamType.Blue);
       yield return new WaitForSeconds(1f);
       _factory.CreateDwarfMiner(TeamType.Red);
       _factory.CreateDwarfMiner(TeamType.Blue);
       
       yield return new WaitForSeconds(1f);
       _factory.CreateDwarfMiner(TeamType.Red);
       _factory.CreateDwarfMiner(TeamType.Blue);
       
       yield return new WaitForSeconds(1f);
       _factory.CreateDwarfMiner(TeamType.Red);
       _factory.CreateDwarfMiner(TeamType.Blue);
        _redCount += 4;
        _blueCount += 4;
        _startSpawn = true;

    }

    private void Update()
    {
        if( _startSpawn)
        {
            if (IsTeamsMeets)
            {
                foreach(TeamType type in Enum.GetValues(typeof(TeamType)))
                {
                    if(type == TeamType.Red)
                    {
                        if(((_currentRedFund - _dwarfCost) > 0) && !_redHiring)
                        {
                            StartCoroutine(HiringWarriorOrMiner(type));
                            _redHiring = true;
                        }
        
                    }
        
                    if (type == TeamType.Blue)
                    {
                        if (((_currentBlueFund - _dwarfCost) > 0) && !_blueHiring)
                        {
                            StartCoroutine(HiringWarriorOrMiner(type));
                            _blueHiring = true;
                        }
        
                    }
                }
                
            }
            else
            {
                foreach (TeamType type in Enum.GetValues(typeof(TeamType)))
                {
                    if (type == TeamType.Red)
                    {
                        if (((_currentRedFund - _dwarfCost) > 0) && !_redHiring)
                        {
                            StartCoroutine(HiringMiner(type));
                            _redHiring = true;
                        }
        
                    }
        
                    if (type == TeamType.Blue)
                    {
                        if (((_currentBlueFund - _dwarfCost) > 0) && !_blueHiring)
                        {
                            StartCoroutine(HiringMiner(type));
                            _blueHiring = true;
                        }
        
                    }
                }
            }
        }

       
    }

    private void CheckGold()
    {
        int i = 0;
        foreach (Vector2Int value in _mapModel.NotMinedOres)
        {
            if(_mapModel.MapMatrix.GetValue(value.x, value.y).GetType() == typeof(GoldOre))
            {
                i++;
            }
        }
        if(i == 0) {
            bool isRedWin = _totalRedFund > _totalBlueFund;
            if (isRedWin)
            {
                _winner = TeamType.Red;
            }
            else
            {
                _winner = TeamType.Blue;
            }
            _isEndGame = true;
        }
    }

    IEnumerator HiringWarriorOrMiner(TeamType type)
    {
        int randomAction = UnityEngine.Random.Range(0, 2);
        yield return new WaitForSeconds(5);
        if (type == TeamType.Red) { 
            if(_redCount == _maxDwarfMinerCount)
            {
                randomAction = 1;
            }
        }
        if (type == TeamType.Blue) {
            if (_blueCount == _maxDwarfMinerCount)
            {
                randomAction = 1;
            }
    
        }
        switch (randomAction)
        {
            case 0:
                if (type == TeamType.Red)
                {
                    _factory.CreateDwarfMiner(TeamType.Red);
                }
    
                if (type == TeamType.Blue)
                {
                    _factory.CreateDwarfMiner(TeamType.Blue);
                }
                break;
            case 1:
                if (type == TeamType.Red)
                {
                    _factory.CreateDwarfWarrior(TeamType.Red);
                }
    
                if (type == TeamType.Blue)
                {
                    _factory.CreateDwarfWarrior(TeamType.Blue);
                }
                break;
           
        }
    
        if (type == TeamType.Red)
        {
            _currentRedFund -= _dwarfCost;
            _redHiring = false;
        }
    
        if (type == TeamType.Blue)
        {
            _currentBlueFund -= _dwarfCost;
            _blueHiring = false;
        }
    }
    
    IEnumerator HiringMiner(TeamType type)
    {
        yield return new WaitForSeconds(5);
        if (type == TeamType.Red)
        {
            _factory.CreateDwarfMiner(TeamType.Red);
        }
    
        if (type == TeamType.Blue)
        {
            _factory.CreateDwarfMiner(TeamType.Blue);
        }
    
    
        if (type == TeamType.Red)
        {
            _currentRedFund -= _dwarfCost;
            _redHiring = false;
        }
    
        if (type == TeamType.Blue)
        {
            _currentBlueFund -= _dwarfCost;
            _blueHiring = false;
        }
    }


}
