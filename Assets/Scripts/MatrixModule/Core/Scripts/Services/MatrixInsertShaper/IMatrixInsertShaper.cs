namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixInsertShaper {
    public interface IMatrixInsertShaper {
        void InsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y);
        void SilentInsertShapeMatrix<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, IMatrix<TMatrixEntity> shape, int x, int y);
    }
}
