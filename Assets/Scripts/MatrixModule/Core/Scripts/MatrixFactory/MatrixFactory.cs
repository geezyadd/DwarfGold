using RSG.Muffin.MatrixModule.Core.Scripts.Services.ExpandMatrix;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixCropper;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixEntityReplacer;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixEntitySorter;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixInsertShaper;
using RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixPathFinder;

namespace RSG.Muffin.MatrixModule.Core.Scripts.MatrixFactory {
    public class MatrixFactory<TMatrixEntity> 
    {
        public Matrix<TMatrixEntity> Create(int width, int height, TMatrixEntity fillEntity) =>
            new(width, height, new MatrixCropper(), new MatrixExpander(), new MatrixInsertShaper(), new MatrixEntityReplacer(), new MatrixPathFinder(),new MatrixEntitySorter(), fillEntity);
    }
}
