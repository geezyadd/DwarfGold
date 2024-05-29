using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DwarfFactory : MonoBehaviour
{
    [SerializeField] private GameObject DwarfMiner;
    [SerializeField] private GameObject DwarfWarrior;
    [SerializeField] private Transform _redSpawnPosition;
    [SerializeField] private Transform _blueSpawnPosition;
    private MapGenerator _mapGenerator;
    private IFactory<GameObject, Transform, GameObject> _customInjectedPrefabFactory;

    [Inject]
    private void InjectDependencies(MapGenerator mapGenerator, IFactory<GameObject, Transform, GameObject> customInjectedPrefabFactory)
    {
        _mapGenerator = mapGenerator;
        _customInjectedPrefabFactory = customInjectedPrefabFactory;
    }

    public void CreateDwarfMiner(TeamType type)
    {
        GameObject dwarf;
        if (type == TeamType.Red)
        {
            dwarf = _customInjectedPrefabFactory.Create(DwarfMiner, _redSpawnPosition);
            dwarf.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
        else
        {
            dwarf = _customInjectedPrefabFactory.Create(DwarfMiner, _blueSpawnPosition);
            dwarf.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }
        DwarfMiner dwarfMiner = dwarf.GetComponent<DwarfMiner>();
        dwarfMiner.Type = type;
    }

    public void CreateDwarfWarrior(TeamType type)
    {  
        GameObject dwarf;
        if (type == TeamType.Red)
        {
            dwarf = _customInjectedPrefabFactory.Create(DwarfWarrior, _redSpawnPosition);
            dwarf.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
        else
        {
            dwarf = _customInjectedPrefabFactory.Create(DwarfWarrior, _blueSpawnPosition);
            dwarf.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }
        dwarf.GetComponent<DwarfWarrior>().Type = type;
    }


}
