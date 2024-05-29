using UnityEngine;
using Zenject;

public class MapGeneratorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MapGenerator>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<DwarfWarrior>().AsSingle();
        Container.Bind<MapModel>().AsSingle();
        Container.Bind<DwarfFactory>().FromComponentInHierarchy().AsSingle();
        Container.Bind<FundController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IFactory<GameObject, Transform, GameObject>>()
                 .To<CustomInjectedPrefabFactory>()
                 .AsSingle();
    }
}
