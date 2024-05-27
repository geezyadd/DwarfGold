using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FundController : MonoBehaviour
{
    public int _redFund = 0;
    public int _blueFund = 0;
    public int RedFund =>
        _redFund;
    public int BlueFund =>
        _blueFund;
    private DwarfFactory _factory;

    [Inject]
    private void InjectDependencies(DwarfFactory factory)
    {
        _factory = factory;
    }

    public void Increase(TeamType type, int value) {
        if(type == TeamType.Red)
        {
            _redFund += value;
        }
        else
        {
            _blueFund += value;
        }
    }
    public void Decrease(TeamType type, int value) {
        if (type == TeamType.Red)
        {
            _redFund -= value;
        }
        else
        {
            _blueFund -= value;
        }
    }

    void Start()
    {
        //StartCoroutine(nameof(SimpleTimer));
    }

    void Update()
    {
        
    }
    IEnumerator SimpleTimer()
    {
        _factory.CreateDwarfMiner(TeamType.Red);
        _factory.CreateDwarfMiner(TeamType.Blue);
       yield return new WaitForSeconds(1f);
       //_factory.CreateDwarfMiner(TeamType.Red);
       //_factory.CreateDwarfMiner(TeamType.Blue);
       //
       //yield return new WaitForSeconds(1f);
       //_factory.CreateDwarfMiner(TeamType.Red);
       //_factory.CreateDwarfMiner(TeamType.Blue);
       //
       //yield return new WaitForSeconds(1f);
       //_factory.CreateDwarfMiner(TeamType.Red);
       //_factory.CreateDwarfMiner(TeamType.Blue);

    }
}
