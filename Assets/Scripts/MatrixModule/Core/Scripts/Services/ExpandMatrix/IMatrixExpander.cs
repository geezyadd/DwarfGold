namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.ExpandMatrix {
    public interface IMatrixExpander {
        void ExpandMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, MatrixOperationDirection matrixOperationDirection, int number, TMatrixEntity fillEntity = default);
    }
}
