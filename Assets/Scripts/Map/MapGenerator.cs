using RSG.Muffin.MatrixModule.Core.Scripts;
using RSG.Muffin.MatrixModule.Core.Scripts.MatrixFactory;
using UnityEngine;
using Zenject;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _mapHolder;
    [SerializeField] private int _mapWidth;
    [SerializeField] private int _mapHeight;
    [SerializeField] private Sprite _rockOreSprite;
    [SerializeField] private Sprite _goldOreSprite;
    [SerializeField] private Sprite _bedRockOreSprite;
    [SerializeField] private Sprite _minedOreSprite;
    [SerializeField] private Color _styleColor;

    private IMatrix<OreBase> _mapMatrix;
    private MapModel _mapmodel;

    [Inject]

    private void InjectDependencies(MapModel mapModel)
    {
        _mapmodel = mapModel;
    }

    public void SetOreMined(Vector2Int step)
    {
        GameObject cell = _mapMatrix.GetValue(step.x, step.y).GetCell();
        _mapMatrix.SetValue(step.x, step.y, new MinedOre());
        _mapMatrix.GetValue(step.x, step.y).SetCell(cell);
        cell.GetComponent<SpriteRenderer>().sprite = _minedOreSprite;
        _mapmodel.AddMinedOre(step);
    }
    public IMatrix<OreBase> GetMapMatrix() { return _mapMatrix; }

    private void Awake()
    {
        _mapMatrix = new MatrixFactory<OreBase>().Create(_mapWidth, _mapHeight, new RockOre());
        for (int y = 0; y < _mapMatrix.GetRowCount(); y++)
        {
            for(int x = 0; x< _mapMatrix.Rows[0].Data.Count; x++)
            {
                GameObject cell = Instantiate(_cellPrefab, new Vector3(x, y), Quaternion.identity, _mapHolder.transform);
                SpriteRenderer renderer = cell.GetComponent<SpriteRenderer>();
                if (x % 2 == 0 && y % 2 == 0)
                {
                    _mapMatrix.SetValue(x, y, new BedRockOre());
                }
                else
                {
                    float randomValue = Random.Range(0f, 1f);

                    if (randomValue < 0.2f)
                    {
                        //cellColor = Color.yellow;
                        _mapMatrix.SetValue(x, y, new GoldOre());
                        _mapmodel.AddNotMinedOre(new Vector2Int(x, y));
                    }
                    else
                    {
                        //rock ore set
                    }
                }
                if(x == 1 && y == 0) 
                    _mapMatrix.SetValue(x, y, new MinedOre());
                

                if(x == _mapMatrix.Rows[0].Data.Count - 1 && y == _mapMatrix.GetRowCount() - 1)
                    _mapMatrix.SetValue(x, y, new MinedOre());

                SetRenderer(renderer, _mapMatrix.GetValue(x, y));
                _mapMatrix.Rows[y].Data[x].SetCell(cell);
            }
        }
    }

    private void SetRenderer(SpriteRenderer renderer, OreBase oreBase)
    {
        if (oreBase.GetType() == typeof(RockOre))
        {
            renderer.sprite = _rockOreSprite;
        }
        else if(oreBase.GetType() == typeof(GoldOre))
        {
            renderer.sprite = _goldOreSprite;
        }
        else
        {
            renderer.sprite = _bedRockOreSprite;
        }
        renderer.color = _styleColor;
    }
}
