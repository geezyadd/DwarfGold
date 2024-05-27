using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DwarfFactory : MonoBehaviour
{
    [SerializeField] private GameObject DwarfMiner;
    [SerializeField] private GameObject DwarfWarrior;
    [SerializeField] private Vector2Int _redSpawnPosition;
    [SerializeField] private Vector2Int _blueSpawnPosition;
    private MapGenerator _mapGenerator;

    [Inject]
    private void InjectDependencies(MapGenerator mapGenerator)
    {
        _mapGenerator = mapGenerator;
    }

    public void CreateDwarfMiner(TeamType type)
    {
        GameObject dwarf;
        if(type == TeamType.Red)
            dwarf = Instantiate(DwarfMiner, new Vector3(_redSpawnPosition.x, _redSpawnPosition.y, 0), Quaternion.identity);
        else
            dwarf = Instantiate(DwarfMiner, new Vector3(_blueSpawnPosition.x, _blueSpawnPosition.y, 0), Quaternion.identity);
        DwarfMiner dwarfMiner = dwarf.GetComponent<DwarfMiner>();
        dwarfMiner.Type = type;
    }

    public void CreateDwarfWarrior(TeamType type)
    {
        GameObject dwarf;
        if (type == TeamType.Red)
            dwarf = Instantiate(DwarfWarrior, new Vector3(_redSpawnPosition.x, _redSpawnPosition.y, 0), Quaternion.identity);
        else
            dwarf = Instantiate(DwarfWarrior, new Vector3(_blueSpawnPosition.x, _blueSpawnPosition.y, 0), Quaternion.identity);
        dwarf.GetComponent<DwarfWarrior>().Type = type;
    }


}
