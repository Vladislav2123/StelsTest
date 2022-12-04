using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private StartWindow _startWindow;
    [SerializeField] private WinWindow _winWindow;
    [SerializeField] private LoseWindow _loseWindow;
    [SerializeField] private UIFade _uiFade;
    [SerializeField] private MapNoiseDisplay _mapNoiseDisplay;

    public override void InstallBindings()
    {
        Container.Bind<StartWindow>().FromInstance(_startWindow);
        Container.Bind<WinWindow>().FromInstance(_winWindow);
        Container.Bind<LoseWindow>().FromInstance(_loseWindow);
        Container.Bind<MapNoiseDisplay>().FromInstance(_mapNoiseDisplay);
        Container.Bind<UIFade>().FromInstance(_uiFade);
    }
}