using UnityEngine;
using Zenject;

public class VolumeDisplay : UIPanel
{
    [SerializeField] private UIBar _bar;

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
    }
}
