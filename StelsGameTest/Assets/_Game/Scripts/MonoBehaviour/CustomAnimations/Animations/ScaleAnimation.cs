using System;
using UnityEngine;
using DG.Tweening;

public class ScaleAnimation : CustomAnimation
{
    [SerializeField] private Transform _scalingTransform;
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private Vector3 _targetScale;

    public override void Play(Action callback = null)
    {
        TryKillAndCreateNewSequence();

        _scalingTransform.localScale = _startScale;

        Sequence.Append(
            _scalingTransform.DOScale(_targetScale, Properties.Duration)
            .SetEase(Properties.Ease, Properties.EaseOvershoot));

        PostProcessAnimation(callback);
    }

    public override void Stop()
    {
        TryKillSequence();
        _scalingTransform.localScale = _startScale;
    }
}
