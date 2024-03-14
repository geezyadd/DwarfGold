using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockOre : OreBase
{

    public override object Clone()
    {
        return new RockOre();
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
