using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.MatrixModule.Core.Scripts {
    public interface IMatrix<TMatrixEntity> : IEnumerable<TMatrixEntity> {
        List<Vector2Int> GetIndexes(Predicate<TMatrixEntity> predicate = null); 
        List<MatrixSavableContainer<TMatrixEntity>> Rows { get; set; }
        List<TMatrixEntity> GetColumnById(int columnId);
        List<TMatrixEntity> GetRowById(int rowId);
        void CropMatrix(MatrixOperationDirection matrixOperationDirection, int number);
        void CropMatrixByRule(MatrixOperationDirection matrixOperationDirection, Predicate<TMatrixEntity> predicate);
        void ExpandMatrix(MatrixOperationDirection matrixOperationDirection, int number, TMatrixEntity fillEntity = default);
        void SilentInsertShapeMatrix(IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null);
        void InsertShapeMatrix(IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null);
        bool TryInsertShapeMatrix(IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null);
        int GetRowCount();
        int GetColumnCount();
        void ReplaceMatrixEntity(TMatrixEntity newEntity, Predicate<TMatrixEntity> predicate);
        List<Vector2Int> PathFinding(Vector2Int start, Vector2Int end, Predicate<TMatrixEntity> predicate);
        Node GetNeighborByDirection(Vector2Int position, Vector2Int direction);
        List<TMatrixEntity> GetSortedMatrixEntities(Comparison<TMatrixEntity> comparison, bool isAscending);
        void SetValue(int x, int y, TMatrixEntity value);
        TMatrixEntity GetValue(int x, int y);
        void RemoveColumn(int columnIndex, int count);
        void RemoveRow(int rowIndex, int count);
        bool CheckForExpression(Predicate<TMatrixEntity> predicate, int columnIndex);
        List<Vector2Int> GetNeighbors(Vector2Int position, Predicate<TMatrixEntity> predicate = null);
        bool ValidatePredicate(Vector2Int position, Predicate<TMatrixEntity> predicate = null);
    }
}