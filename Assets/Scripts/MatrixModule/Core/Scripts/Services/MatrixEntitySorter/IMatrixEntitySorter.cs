using System;
using System.Collections.Generic;

namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixEntitySorter {
    public interface IMatrixEntitySorter {
        List<TMatrixEntity> GetSortedMatrixEntities<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, Comparison<TMatrixEntity> comparison, bool isAscending);
    }
}
