using Zenject;

public class StartButton : UIButton
{
    [Inject] private GameStateController _gameStateController;

    protected override void OnClick()
    {
        _gameStateController.State = GameState.Playing;
    }
}
