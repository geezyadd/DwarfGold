using System;

namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixInsertShaper {
    public interface IMatrixInsertShaper {
        void InsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null);
        void SilentInsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null);
        bool TryInsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y, Predicate<TMatrixEntity> matrixPredicate = null, Predicate<TMatrixEntity> shapePredicate = null);
    }
}
