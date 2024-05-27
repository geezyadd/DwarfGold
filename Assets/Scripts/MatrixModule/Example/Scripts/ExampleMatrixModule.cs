using System;
using System.Collections.Generic;
using RSG.Muffin.MatrixModule.Core.Scripts;
using RSG.Muffin.MatrixModule.Core.Scripts.MatrixFactory;
using UnityEngine;
using UnityEngine.UI;

namespace RSG.Muffin.MatrixModule.Example.Scripts {
    public class ExampleMatrixModule : MonoBehaviour {
        [SerializeField] private Button _exampleCropMatrix;
        [SerializeField] private Button _exampleCropMatrixByRule;
        [SerializeField] private Button _exampleExpandMatrix;
        [SerializeField] private Button _exampleInsertShapeMatrix;
        [SerializeField] private Button _exampleMatrixEntityReplace;
        [SerializeField] private Button _exampleFindPath;
        [SerializeField] private Button _exampleGetNeighborByDirection;
        [SerializeField] private Button _exampleMatrixEntitySort;
        private IMatrix<int> _exampleMatrix = new MatrixFactory<int>().Create(5,5, 4);

        private void Start() {
            _exampleCropMatrix.onClick.AddListener(ExampleCropMatrix);
            _exampleCropMatrixByRule.onClick.AddListener(ExampleCropMatrixByRule);
            _exampleExpandMatrix.onClick.AddListener(ExampleExpandMatrix);
            _exampleInsertShapeMatrix.onClick.AddListener(ExampleInsertShapeMatrix);
            _exampleMatrixEntityReplace.onClick.AddListener(ExampleMatrixEntityReplace);
            _exampleFindPath.onClick.AddListener(ExampleFindPath);
            _exampleGetNeighborByDirection.onClick.AddListener(ExampleGetNeighborByDirection);
            _exampleMatrixEntitySort.onClick.AddListener(ExampleMatrixEntitySort);
            PrintMatrix(_exampleMatrix);
        }
        private void SetMatrixDefault() {
            _exampleMatrix = new MatrixFactory<int>().Create(5,5, 4);
        }

        private void ExampleMatrixEntitySort() {
            SetMatrixDefault();
            _exampleMatrix.SetValue(0, 1, 6);
            _exampleMatrix.SetValue(1, 1, 7);
            _exampleMatrix.SetValue(1, 2, 2);
            _exampleMatrix.SetValue(1, 3, 9);
            _exampleMatrix.SetValue(2, 3, 3);
            _exampleMatrix.SetValue(3, 3, 8);
            _exampleMatrix.SetValue(4, 3, 5);
            Debug.Log("Default matrix:");
            PrintMatrix(_exampleMatrix);

            int AscendingComparison(int x, int y) =>
                x.CompareTo(y);

            List<int> sorted = _exampleMatrix.GetSortedMatrixEntities(AscendingComparison, true);
            foreach (int value in sorted)
                Debug.Log(value);
        }

        private void ExampleGetNeighborByDirection() {
            _exampleMatrix.SetValue(0, 1,0);
            _exampleMatrix.SetValue(1, 1,0);
            _exampleMatrix.SetValue(1, 2,0);
            _exampleMatrix.SetValue(1, 3,0);
            _exampleMatrix.SetValue(2, 3,0);
            _exampleMatrix.SetValue(3, 3,0);
            _exampleMatrix.SetValue(4, 3,0);
            PrintMatrix(_exampleMatrix);
            Node value = _exampleMatrix.GetNeighborByDirection(new Vector2Int(2, 2), new Vector2Int(1, 0));
            Debug.Log(_exampleMatrix.GetValue(value.X,value.Y));
        }
        
        private void ExampleFindPath() {
            SetMatrixDefault();

            bool IsZero(int x) =>
                x != 0;

            _exampleMatrix.SetValue(0, 1,0);
            _exampleMatrix.SetValue(1, 1,0);
            _exampleMatrix.SetValue(1, 2,0);
            _exampleMatrix.SetValue(1, 3,0);
            _exampleMatrix.SetValue(2, 3,0);
            _exampleMatrix.SetValue(3, 3,0);
            Debug.Log("Default matrix:");
            PrintMatrix(_exampleMatrix);
            List<Vector2Int> path = _exampleMatrix.PathFinding(new Vector2Int(0, 0), new Vector2Int(4, 4), IsZero);
            Debug.Log("Path:");
            foreach (Vector2Int step in path) {
                Debug.Log("x = " + step.x + " y = " + step.y);
            }

        }

        private void ExampleMatrixEntityReplace() {
            SetMatrixDefault();
            Debug.Log("Default matrix:");
            PrintMatrix(_exampleMatrix);

            bool IsZero(int x) =>
                x == 4;

            _exampleMatrix.ReplaceMatrixEntity(5, IsZero);
            Debug.Log("==============MatrixEntityReplaced========================");
            PrintMatrix(_exampleMatrix);
            
        }

        private void ExampleExpandMatrix() {
            SetMatrixDefault();
            Debug.Log("Default matrix:");
            PrintMatrix(_exampleMatrix); 
            _exampleMatrix.ExpandMatrix(MatrixOperationDirection.Bottom, 2, 2);
            Debug.Log("==============ExpandMatrix========================");
            PrintMatrix(_exampleMatrix);
        }

        private void ExampleInsertShapeMatrix() {
            SetMatrixDefault();
            Debug.Log("Default matrix:");
            _exampleMatrix.SetValue(2,2, 2);
            PrintMatrix(_exampleMatrix); 
            IMatrix<int> exampleShape = new MatrixFactory<int>().Create(10,10, 1);
            exampleShape.SetValue(0, 0,0);
            Debug.Log("Shape:");
            PrintMatrix(exampleShape); 
            Predicate<int> isOne = x => x == 1;
            Predicate<int> isZero = x => x == 4;
            Debug.Log(_exampleMatrix.TryInsertShapeMatrix(exampleShape, 2, 2, null, isOne));
            Debug.Log("==============InsertShapeMatrix========================");
            PrintMatrix(_exampleMatrix);
        }

        private void ExampleCropMatrixByRule() {
            SetMatrixDefault();
            _exampleMatrix.SetValue(0, 4,0);
            _exampleMatrix.SetValue(1, 4,0);
            _exampleMatrix.SetValue(2, 4,0);
            _exampleMatrix.SetValue(3, 4,0);
            _exampleMatrix.SetValue(4, 4,0);
            Debug.Log("Default matrix:");
            PrintMatrix(_exampleMatrix);

            bool IsZero(int x) =>
                x == 0;

            _exampleMatrix.CropMatrixByRule(MatrixOperationDirection.Bottom, IsZero);
            Debug.Log("==============CropMatrixByRule========================");
            PrintMatrix(_exampleMatrix);
           
        }

        private void ExampleCropMatrix() {
            SetMatrixDefault();
            Debug.Log("Default matrix:");
            PrintMatrix(_exampleMatrix); 
            Debug.Log("==============CropMatrix========================");
            _exampleMatrix.CropMatrix(MatrixOperationDirection.Top, 1);
            PrintMatrix(_exampleMatrix);
        }

        private void PrintMatrix<T>(IMatrix<T> matrix) {
            for (int y = 0; y < matrix.GetRowCount(); y++) {
                MatrixSavableContainer<T> row = matrix.Rows[y];
                string matrixString = "";
                for (int x = 0; x < matrix.GetColumnCount(); x++) {
                    T item = row.Data[x];
                    matrixString += item + $"({x}, {y}) ";
                }

                Debug.Log(matrixString);
            }
        }
    }
}
