using RSG.Muffin.MatrixModule.Core.Scripts;
using RSG.Muffin.MatrixModule.Core.Scripts.MatrixFactory;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _mapHolder;
    [SerializeField] private int _mapWidth;
    [SerializeField] private int _mapHeight;

    private Matrix<OreBase> _mapMatrix;

    private void Start()
    {
        _mapMatrix = new MatrixFactory<OreBase>().Create(_mapWidth, _mapHeight, new RockOre());
        Color cellColor;
        for (int y = 0; y < _mapMatrix.GetRowCount(); y++)
        {
            for(int x = 0; x< _mapMatrix.Rows[0].Data.Count; x++)
            {
                if(x % 2 == 0 && y % 2 == 0)
                {
                    cellColor = Color.black;
                    _mapMatrix.SetValue(x, y, new BedRockOre());
                }
                else
                {
                    float randomValue = Random.Range(0f, 1f);

                    if (randomValue < 0.2f)
                    {
                        cellColor = Color.yellow;
                        _mapMatrix.SetValue(x, y, new GoldOre());
                    }
                    else
                    {
                        cellColor = Color.gray;
                    }
                }
                
                GameObject cell = Instantiate(_cellPrefab, new Vector3(x, y), Quaternion.identity, _mapHolder.transform);
                cell.GetComponent<SpriteRenderer>().color = cellColor;
                _mapMatrix.Rows[y].Data[x].SetCell(cell);
            }
        }
    }
}
