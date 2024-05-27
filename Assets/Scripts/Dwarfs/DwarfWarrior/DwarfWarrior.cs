using RSG.Muffin.MatrixModule.Core.Scripts;
using System.Collections;
using System.Collections.Generic;
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
    private bool _isStepActive;
    private bool _isPathPassed;

    [Inject]
    public void Construct(MapGenerator mapGenerator, MapModel mapModel)
    {
        _mapGenerator = mapGenerator;
        _mapModel = mapModel;   
    }

    public void Start()
    {
        _currentPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        _matrixMap = _mapGenerator.GetMapMatrix();
        SetRandomPath();
        MoveToPoint(_matrixMap, _path);
    }

    private void SetRandomPath()
    {
        _path.Clear();
        _path = _matrixMap.PathFinding(_currentPosition, GetRandomPoint(), x => x.GetType() == typeof(MinedOre));
        if(_path.Count == 0) {
            SetRandomPath();
        }
    }

    private Vector2Int GetRandomPoint() // else
    {
        int pointCount = Random.Range(0, _mapModel.MinedOres.Count);
        return _mapModel.MinedOres[pointCount];
    }

    private bool PointValidator(Vector2Int point)
    {
        if(_matrixMap.GetValue(point.x, point.y).GetType() == typeof(BedRockOre))
        {
            return false;
        }

        return true;
    }

    public virtual void MoveToPoint(IMatrix<OreBase> mapMatrix, List<Vector2Int> path)
    {
        _matrixMap = mapMatrix;
        StartCoroutine(PathMover(path));
    }


    private IEnumerator PathMover(List<Vector2Int> path) //else
    {
        _isPathPassed = false;
        foreach (Vector2Int step in path)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(MoveForOneCell(0.5f, step));
        }
        _isPathPassed = true;
        SetRandomPath();
        MoveToPoint(_matrixMap, _path);
    }

    private void SetStepPassed(Vector2Int step)
    {
        _mapGenerator.SetOreMined(step);
    }

    private IEnumerator MoveForOneCell(float duration, Vector2Int step)
    {
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
        SetStepPassed(step);
        _isStepActive = false;
    }

}
