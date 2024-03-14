using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DwarfBase : MonoBehaviour
{
    private bool _isStepActive = false;
    private float _stepTime;
    private Vector2Int _currentCell;


    private IEnumerator MoveToPoint(List<Vector2Int> path)
    {
        foreach(Vector2Int step in path) {
            yield return new WaitUntil(() => !_isStepActive);
            StartCoroutine(MoveForOneCell(step, _stepTime));
        }
    }

    private IEnumerator MoveForOneCell(Vector2Int step, float stepDelay)
    {
        _isStepActive = true;

        yield return new WaitForSeconds(stepDelay);
        //step
        _currentCell = step;
        _isStepActive = false;
    }

   


}
