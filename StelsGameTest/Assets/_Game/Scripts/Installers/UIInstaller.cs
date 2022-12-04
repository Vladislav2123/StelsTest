using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private MapNoiseDisplay _mapNoiseDisplay;

    public override void InstallBindings()
    {
        Container.Bind<MapNoiseDisplay>().FromInstance(_mapNoiseDisplay);
    }
}