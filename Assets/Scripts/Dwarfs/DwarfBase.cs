using RSG.Muffin.MatrixModule.Core.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DwarfBase : MonoBehaviour
{
    //private bool _isStepActive = false;
    //private Matrix<OreBase> _matrixMap;
    //
    //
    //public virtual void MoveToPoint(Matrix<OreBase> mapMatrix, List<Vector2Int> path)
    //{
    //    _matrixMap = mapMatrix;
    //    StartCoroutine(PathMover(path));
    //}
    //
    //
    //private IEnumerator PathMover(List<Vector2Int> path)
    //{
    //    foreach (Vector2Int step in path)
    //    {
    //        //Debug.LogError(step.x + " " + step.y);
    //        yield return new WaitForSeconds(0.5f);
    //        StartCoroutine(MoveForOneCell(0.5f, step));
    //    }
    //}
    //
    //private void SetStepRed(Vector2Int step)
    //{
    //    _isStepActive = true;
    //    GameObject cell = _matrixMap.Rows[step.y].Data[step.x].GetCell();
    //    SpriteRenderer renderer = cell.GetComponent<SpriteRenderer>();
    //    renderer.color = Color.red;
    //    _matrixMap.Rows[step.y].Data[step.x].GetCell().GetComponent<SpriteRenderer>().color = Color.red;
    //    //_currentCell = step;
    //    _isStepActive = false;
    //}
    //
    //private IEnumerator MoveForOneCell(float duration, Vector2Int step)
    //{
    //    Vector3 newPosition = new Vector3(step.x, step.y, 0);
    //    float elapsedTime = 0;
    //    Vector3 startingPos = transform.position;
    //
    //    while (elapsedTime < duration)
    //    {
    //        transform.position = Vector3.MoveTowards(startingPos, newPosition, (elapsedTime / duration) * Vector3.Distance(startingPos, newPosition));
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //
    //    transform.position = newPosition;
    //    SetStepRed(step);
    //}




}
