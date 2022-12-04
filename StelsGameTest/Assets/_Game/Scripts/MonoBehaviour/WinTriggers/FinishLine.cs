using UnityEngine;
using Zenject;

public class FinishLine : MonoBehaviour
{
    [Inject] private GameStateController _gameStateController;

    private void OnTriggerEnter(Collider other)
    {
        if (_gameStateController.IsPlaying == false) return;
        if (other.TryGetComponent(out Player player) == false) return;

        _gameStateController.State = GameState.Win;
    }
}
