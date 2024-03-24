using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixPathFinder {
    public interface IMatrixPathFinder {
        List<Vector2Int> PathFinding<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, Vector2Int startPosition, Vector2Int endPosition, Predicate<TMatrixEntity> predicate = null, bool random = false);
    }
}
