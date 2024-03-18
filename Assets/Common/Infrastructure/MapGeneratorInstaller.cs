using Zenject;

public class MapGeneratorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MapGenerator>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<DwarfWarrior>().AsSingle();
    }
}
