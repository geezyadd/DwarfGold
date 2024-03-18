using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldOre : OreBase
{
    private GameObject _cell;

    public override void SetCell(GameObject cell)
    {
        _cell = cell;
    }
    public override GameObject GetCell()
    {
        return _cell;
    }
    public override object Clone()
    {
        return new GoldOre();
    }

    public void Mine()
    {
        throw new System.NotImplementedException();
    }

    public void StartTunel()
    {
        throw new System.NotImplementedException();
    }

    public void StopTunel()
    {
        throw new System.NotImplementedException();
    }

   
}
