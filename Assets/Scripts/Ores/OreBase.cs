
using System;
using UnityEngine;

public abstract class OreBase : ICloneable
{
    private GameObject _cell;

    public virtual object Clone()
    {
        return this;
    }

    public virtual void SetCell(GameObject cell)
    {
        _cell = cell;
    }
    public virtual GameObject GetCell()
    {
        return _cell;
    }

    void Mine() { }
    void StartTunel() { }
    void StopTunel() { }

}
