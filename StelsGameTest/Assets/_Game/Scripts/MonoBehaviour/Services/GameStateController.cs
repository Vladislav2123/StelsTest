using UnityEngine;
using System;
using Zenject;

public enum GameState
{
    Menu, Playing, Win, Lose
}

public class GameStateController : MonoBehaviour
{
    public event Action<GameState> OnStateChangedEvent;
    public event Action OnMenuStateEvent;
    public event Action OnGameStartedEvent;
    public event Action OnGameEndedEvent;
    public event Action OnWinEvent;
    public event Action OnLoseEvent;

    [SerializeField] private GameState _state;

    [Inject] private StartWindow _startWindow;
    [Inject] private WinWindow _winWindow;
    [Inject] private LoseWindow _loseWindow;

    public bool IsPlaying { get; private set; }
    public GameState State
    {
        get => _state;
        set
        {
            if (value == _state) return;

            _state = value;

            StartState();
        }
    }

    private void Start()
    {
        StartState();
    }

    private void SetMenuState()
    {
        IsPlaying = false;
        Debug.Log("MenuState");
        _startWindow.TryShow();

        OnMenuStateEvent?.Invoke();
    }

    private void TryStartGame()
    {
        if (IsPlaying) return;

        _startWindow.TryHide();

        IsPlaying = true;
        OnGameStartedEvent?.Invoke();
    }

    private void TryWin()
    {
        if (IsPlaying == false) return;
        EndGame();

        _winWindow.TryShow();
        OnWinEvent?.Invoke();
    }

    private void TryLose()
    {
        if (IsPlaying == false) return;
        EndGame();

        _loseWindow.TryShow();
        OnLoseEvent?.Invoke();
    }

    private void EndGame()
    {
        IsPlaying = false;
        OnGameEndedEvent?.Invoke();
    }

    private void StartState()
    {
        switch (State)
        {
            case GameState.Menu:
                SetMenuState();
                break;

            case GameState.Playing:
                TryStartGame();
                break;

            case GameState.Win:
                TryWin();
                break;

            case GameState.Lose:
                TryLose();
                break;
        }

        OnStateChangedEvent?.Invoke(_state);
    }
}