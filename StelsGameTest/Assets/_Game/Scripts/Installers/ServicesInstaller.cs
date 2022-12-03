using UnityEngine;
using Zenject;
using UnityEngine.AI;

public class ServicesInstaller : MonoInstaller
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private MapController _mapController;
    [SerializeField] private NavMeshSurface _navMesh;

    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromInstance(_inputHandler);
        Container.Bind<MapController>().FromInstance(_mapController);
        Container.Bind<NavMeshSurface>().FromInstance(_navMesh);
    }
}