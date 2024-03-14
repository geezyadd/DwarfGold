using System;
using System.Collections.Generic;

namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixInsertShaper {
    public class MatrixInsertShaper : IMatrixInsertShaper 
    {
        public void InsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y) {
            if (shape.GetRowCount() > matrix.Rows.Count || shape.GetRowById(0).Count >= matrix.Rows[0].Data.Count)
                throw new IndexOutOfRangeException($"Row or column count is out of range.");
            SilentInsertShapeMatrix(matrix, shape, x, y);
        }

        public void SilentInsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y) {
            int ownY = matrix.Rows.Count - 1 - y;
            for (int shapeY = 0; shapeY < shape.GetRowCount(); shapeY++) {
                List<TMatrixEntity> row = shape.GetRowById(shapeY);
                int ownX = x;
                foreach (TMatrixEntity entity in row) {
                    matrix.Rows[ownY].Data[ownX] = entity;

                    ownX++;
                    if (ownX == matrix.Rows[0].Data.Count)
                        break;
                }

                ownY--;
                if (ownY == matrix.Rows.Count)
                    return;
            }
        }
    }
}
