using UnityEngine;
using Zenject;

public class VolumeDisplay : UIPanel
{
    [SerializeField] private UIBar _bar;
    [SerializeField] private float _hideDelay = 1;

    [Inject] private MapController _mapController;
    [Inject] private GameStateController _gameStateController;

    private Player Player => _mapController.Map.Player;

    private void OnEnable()
    {
        _mapController.OnGeneratedEvent += SubscribePlayer;
        _gameStateController.OnGameEndedEvent += Hide;
    }
    private void OnDisable()
    {
        _mapController.OnGeneratedEvent -= SubscribePlayer;
        _gameStateController.OnGameEndedEvent -= Hide;
    }

    private void SubscribePlayer()
    {
        Player.OnVolumeChangedEvent += DisplayVolume;
        DisplayVolume();
    }

    private void DisplayVolume()
    {
        if (_gameStateController.IsPlaying == false) return;

        _bar.FillAmount = Player.Volume / Player.MaxVolume;

        if (Player.Volume == 0) TryHide();
        else TryShow();
    }
}
