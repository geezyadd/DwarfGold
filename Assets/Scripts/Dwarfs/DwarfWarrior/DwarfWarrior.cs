using RSG.Muffin.MatrixModule.Core.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

public class DwarfWarrior : DwarfBase
{
    private Vector2Int _currentPosition;
    private List<Vector2Int> _path;
    private Matrix<OreBase> _mapMatrix;
    private MapGenerator _mapGenerator;

    [Inject]
    public void Construct(MapGenerator mapGenerator)
    {
        _mapGenerator = mapGenerator;
    }

    public void Start()
    {
        _currentPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        //_currentPoint = new Vector2Int(1, 0);
        _mapMatrix = _mapGenerator.GetMapMatrix();
        _path = _mapMatrix.PathFinding(_currentPosition, new Vector2Int(47, 57), x => x.GetType() != typeof(BedRockOre));
        MoveToPoint(_mapMatrix, _path);
    }

}
