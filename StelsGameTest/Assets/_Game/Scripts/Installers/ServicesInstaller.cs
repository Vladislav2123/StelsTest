using UnityEngine;
using Zenject;
using UnityEngine.AI;

public class ServicesInstaller : MonoInstaller
{
    [SerializeField] private NavMeshSurface _navMesh;

    public override void InstallBindings()
    {
        Container.Bind<NavMeshSurface>().FromInstance(_navMesh);
    }
}