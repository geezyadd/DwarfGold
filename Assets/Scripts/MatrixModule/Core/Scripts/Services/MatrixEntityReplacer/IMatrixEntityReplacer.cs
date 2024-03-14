using System;

namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixEntityReplacer {
    public interface IMatrixEntityReplacer {
        void ReplaceMatrixEntity<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, TMatrixEntity newEntity, Predicate<TMatrixEntity> predicate);
    }
}
