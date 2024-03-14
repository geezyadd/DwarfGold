using System;

namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixCropper {
    public interface IMatrixCropper {
        void CropMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, MatrixOperationDirection matrixOperationDirection, int number);
        void CropMatrixByRule<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, MatrixOperationDirection matrixOperationDirection, Predicate<TMatrixEntity> predicate);
    }
}
