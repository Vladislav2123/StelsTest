using UnityEngine;
using Zenject;
using UnityEngine.AI;

public class ServicesInstaller : MonoInstaller
{
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private NavMeshSurface _navMesh;

    public override void InstallBindings()
    {
        Container.Bind<MapGenerator>().FromInstance(_mapGenerator);
        Container.Bind<NavMeshSurface>().FromInstance(_navMesh);
    }
}