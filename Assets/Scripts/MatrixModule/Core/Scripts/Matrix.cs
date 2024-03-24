using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RSG.Muffin.MatrixModule.Core.Scripts.Exceptions;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.ExpandMatrix;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixCropper;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixEntityReplacer;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixEntitySorter;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixInsertShaper;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixPathFinder;
using UnityEngine;

namespace RSG.Muffin.MatrixModule.Core.Scripts {
    public class Matrix<TMatrixEntity> : IMatrix<TMatrixEntity>, IEnumerator<TMatrixEntity> {
        public TMatrixEntity Current { get; } //IEnumerator property
        public List<MatrixSavableContainer<TMatrixEntity>> Rows { get; set; }

        private readonly IMatrixCropper _matrixCropper;
        private readonly IMatrixExpander _matrixExpander;
        private readonly IMatrixInsertShaper _matrixInsertShaper;
        private readonly IMatrixEntityReplacer _matrixEntityReplacer;
        private readonly IMatrixPathFinder _matrixPathFinder;
        private readonly IMatrixEntitySorter _matrixEntitySorter;

        public Matrix(int width, int height, IMatrixCropper matrixCropper, IMatrixExpander matrixExpander, IMatrixInsertShaper matrixInsertShaper, IMatrixEntityReplacer matrixEntityReplacer, IMatrixPathFinder matrixPathFinder, IMatrixEntitySorter matrixEntitySorter, TMatrixEntity fillEntity = default) {
            _matrixEntityReplacer = matrixEntityReplacer;
            _matrixInsertShaper = matrixInsertShaper;
            _matrixCropper = matrixCropper;
            _matrixExpander = matrixExpander;
            _matrixPathFinder = matrixPathFinder;
            _matrixEntitySorter = matrixEntitySorter;
            Rows = new List<MatrixSavableContainer<TMatrixEntity>>();
            CreateMatrix(width, height, fillEntity);
        }

        public List<TMatrixEntity> GetColumnById(int columnId) {
            if (Rows.Count == 0 || columnId < 0 || columnId >= Rows[0].Data.Count)
                throw new IndexOutOfRangeException($"Column index {columnId} is out of range.");

            return Rows.Select(row => row.Data[columnId]).ToList();
        }

        public List<TMatrixEntity> GetRowById(int rowId) {
            if (rowId < 0 || rowId >= Rows.Count)
                throw new IndexOutOfRangeException($"Row index {rowId} is out of range.");

            return Rows[rowId].Data;
        }

        public void CropMatrix(MatrixOperationDirection matrixOperationDirection, int number) =>
            _matrixCropper.CropMatrix(this, matrixOperationDirection, number);

        public void CropMatrixByRule(MatrixOperationDirection matrixOperationDirection, Predicate<TMatrixEntity> predicate) =>
            _matrixCropper.CropMatrixByRule(this, matrixOperationDirection, predicate);

        public int GetRowCount() =>
            Rows.Count;

        public void ExpandMatrix(MatrixOperationDirection matrixOperationDirection, int number, TMatrixEntity fillEntity = default) =>
            _matrixExpander.ExpandMatrix(this, matrixOperationDirection, number, fillEntity);

        public void InsertShapeMatrix(IMatrix<TMatrixEntity> shape, int x, int y) =>
            _matrixInsertShaper.InsertShapeMatrix(this, shape, x, y);

        public void SilentInsertShapeMatrix(IMatrix<TMatrixEntity> shape, int x, int y) =>
            _matrixInsertShaper.SilentInsertShapeMatrix(this, shape, x, y);

        public void ReplaceMatrixEntity(TMatrixEntity newEntity, Predicate<TMatrixEntity> predicate) =>
            _matrixEntityReplacer.ReplaceMatrixEntity(this, newEntity, predicate);

        public List<Vector2Int> PathFinding(Vector2Int start, Vector2Int end, Predicate<TMatrixEntity> predicate = null, bool random = false) =>
            _matrixPathFinder.PathFinding(this, start, end, predicate, random);

        public Node GetNeighborByDirection(Vector2Int position, Vector2Int direction) =>
            ValidateIndex(new Vector2(position.x  + direction.x, position.y + direction.y))
                ? new Node(position.x + direction.x, position.y - direction.y)
                : null;

        public List<TMatrixEntity> GetSortedMatrixEntities(Comparison<TMatrixEntity> comparison, bool isAscending) =>
            _matrixEntitySorter.GetSortedMatrixEntities(this, comparison, isAscending);

        public void SetValue(int x, int y, TMatrixEntity value) =>
            Rows[y].Data[x] = value;

        public TMatrixEntity GetValue(int x, int y) =>
            Rows[y].Data[x];

        public void RemoveColumn(int columnIndex, int count) {
            foreach (MatrixSavableContainer<TMatrixEntity> row in Rows)
                row.Data.RemoveRange(columnIndex, count);
        }

        public void RemoveRow(int rowIndex, int count) =>
            Rows.RemoveRange(rowIndex, count);

        public bool CheckForExpression(Predicate<TMatrixEntity> predicate, int columnIndex) {
            foreach (MatrixSavableContainer<TMatrixEntity> row in Rows)
                if(!predicate(row.Data[columnIndex]))
                    return false;

            return true;
        }

        public List<Vector2Int> GetNeighbors(Vector2Int position, Predicate<TMatrixEntity> predicate = null) {
            List<Vector2Int> neighbors = new();
            if (ValidateIndex(new Vector2(position.x, position.y - 1)) && ValidatePredicate(new Vector2Int(position.x, position.y - 1), predicate))
                neighbors.Add(new Vector2Int(position.x, (position.y - 1)));
        
            if (ValidateIndex(new Vector2(position.x, position.y + 1)) && ValidatePredicate(new Vector2Int(position.x, position.y + 1), predicate))
                neighbors.Add(new Vector2Int(position.x, (position.y + 1)));
        
            if (ValidateIndex(new Vector2(position.x - 1, position.y)) && ValidatePredicate(new Vector2Int(position.x - 1, position.y), predicate))
                neighbors.Add(new Vector2Int(position.x - 1, position.y));
        
            if (ValidateIndex(new Vector2(position.x + 1, position.y)) && ValidatePredicate(new Vector2Int(position.x + 1, position.y), predicate))
                neighbors.Add(new Vector2Int(position.x + 1, position.y));
        
            return neighbors;
        }

        public List<Vector2Int> GetNeighborsRandomly(Vector2Int position, Predicate<TMatrixEntity> predicate = null)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            // Add neighbors in a random order
            List<Vector2Int> possibleNeighbors = new List<Vector2Int> {
                new Vector2Int(position.x, position.y - 1),
                new Vector2Int(position.x, position.y + 1),
                new Vector2Int(position.x - 1, position.y),
                new Vector2Int(position.x + 1, position.y)
            };

            possibleNeighbors = possibleNeighbors.OrderBy(x => UnityEngine.Random.value).ToList();

            foreach (Vector2Int possibleNeighbor in possibleNeighbors)
            {
                if (ValidateIndex(new Vector2(possibleNeighbor.x, possibleNeighbor.y)) && ValidatePredicate(possibleNeighbor, predicate))
                {
                    neighbors.Add(possibleNeighbor);
                }
            }

            return neighbors;
        }

        public bool ValidatePredicate(Vector2Int position, Predicate<TMatrixEntity> predicate = null) =>
            predicate == null || predicate(Rows[position.y].Data[position.x]);

        private bool ValidateIndex(Vector2 position) => 
            position.y >= 0 && position.y < Rows.Count && position.x >= 0 && position.x < Rows[0].Data.Count;
        
        private void CreateMatrix(int width, int height, TMatrixEntity fillEntity = default) {
            if(!typeof(TMatrixEntity).IsValueType && fillEntity is not ICloneable)
                throw new NotValidMatrixEntityTypeException(typeof(TMatrixEntity));

            for (int y = 0; y < height; y++) {
                List<TMatrixEntity> row = new();
                for (int x = 0; x < width; x++)
                    if (typeof(TMatrixEntity).IsValueType)
                        row.Add(fillEntity);
                    else
                        row.Add((TMatrixEntity)((ICloneable)fillEntity).Clone());

                Rows.Add(new MatrixSavableContainer<TMatrixEntity> {
                    Data = row
                });
            }
        }
        
        //IEnumerator functions

        public bool MoveNext() =>
            false;
        
        public void Reset() { }

        object IEnumerator.Current =>
            Current;
        
        public void Dispose() { }
        
        IEnumerator<TMatrixEntity> IEnumerable<TMatrixEntity>.GetEnumerator() => 
            GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
        
        private IEnumerator<TMatrixEntity> GetEnumerator() {
            foreach (MatrixSavableContainer<TMatrixEntity> row in Rows)
                foreach (TMatrixEntity entity in row.Data)
                    yield return entity;
        }
    }
    
    [Serializable]
    public class MatrixSavableContainer<TMatrixEntity> {
        public List<TMatrixEntity> Data;
        
        public IEnumerator<TMatrixEntity> GetEnumerator() =>
            Data.GetEnumerator();
    }
}