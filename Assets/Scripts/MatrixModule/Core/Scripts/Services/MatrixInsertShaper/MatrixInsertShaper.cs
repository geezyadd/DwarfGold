using System;
using System.Collections.Generic;

namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixInsertShaper {
    public class MatrixInsertShaper : IMatrixInsertShaper {
        public void InsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null) {
            if (shape.GetRowCount() > matrix.GetRowCount() || shape.GetColumnCount() > matrix.GetColumnCount())
                throw new IndexOutOfRangeException($"Row or column count is out of range.");
            SilentInsertShapeMatrix(matrix, shape, x, y, matrixPredicate, shapePredicate);
        }

        public void SilentInsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null) {
            int ownY = y;
            for (int shapeY = 0; shapeY < shape.GetRowCount(); shapeY++) {
                List<TMatrixEntity> row = shape.GetRowById(shapeY);
                int ownX = x;
                foreach (TMatrixEntity entity in row) {
                    if (ownX >= matrix.GetColumnCount()) 
                        break;
                   
                    if ((shapePredicate == null || shapePredicate(entity)) && (matrixPredicate == null || matrixPredicate(matrix.GetValue(ownX, ownY))))
                        matrix.SetValue(ownX, ownY, entity);

                    ownX++;
                }

                ownY++;
                if (ownY == matrix.GetRowCount())
                    return;
            }
        }
        
        public bool TryInsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null) {
            if (shape.GetRowCount() > matrix.GetRowCount() || shape.GetColumnCount() > matrix.GetColumnCount())
                return false;
            SilentInsertShapeMatrix(matrix, shape, x, y, matrixPredicate, shapePredicate);
            return true;
        }
    }
}
