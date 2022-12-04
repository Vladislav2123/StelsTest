using UnityEngine;
using Zenject;

public class RestartButton : UIButton
{
    [SerializeField] private bool _completeLevel;

    [Inject] private LevelsManager _levelsManager;

    protected override void OnClick()
    {
        if (_completeLevel) _levelsManager.CompleteLevel();
        _levelsManager.Restart();
    }
}
