using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private InputHandler _inputHandler;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 input = _inputHandler.Input.Player.Movement.ReadValue<Vector2>();
        if (input == Vector2.zero) return;

        _player.Movement.Move(input);
    }
}
