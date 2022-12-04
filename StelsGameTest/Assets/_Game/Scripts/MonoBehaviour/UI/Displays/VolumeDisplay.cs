using UnityEngine;
using Zenject;

public class VolumeDisplay : UIPanel
{
    [SerializeField] private UIBar _bar;
    [SerializeField] private float _hideDelay = 1;

    [Inject] private MapController _mapController;

    private Player Player => _mapController.Map.Player;

    private void Awake()
    {
        _mapController.OnGeneratedEvent += SubscribePlayer;
    }

    private void SubscribePlayer()
    {
        Player.OnVolumeChangedEvent += DisplayVolume;
        DisplayVolume();
    }

    private void DisplayVolume()
    {
        _bar.FillAmount = Player.Volume / Player.MaxVolume;

        if (Player.Volume == 0) TryHide();
        else TryShow();
    }
}
