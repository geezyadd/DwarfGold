using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModel 
{
    private List<Vector2Int> _minedOres = new();
    private List<Vector2Int> _notMinedOres = new();

    public List<Vector2Int> MinedOres => _minedOres;
    public List<Vector2Int> NotMinedOres => _notMinedOres;

    public void AddMinedOre(Vector2Int minedOre)
    {
        _minedOres.Add(minedOre);
    }
    public void AddNotMinedOre(Vector2Int minedOre)
    {
        _notMinedOres.Add(minedOre);
    }
}
