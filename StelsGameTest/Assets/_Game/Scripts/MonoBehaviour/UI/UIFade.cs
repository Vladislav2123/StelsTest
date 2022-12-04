using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [SerializeField] private float _fadeDuration;
    [SerializeField] private Ease _fadeEase;
    [SerializeField] private Image _fadingImage;
    [SerializeField] private bool _fadeOnStart = true;

    private void Start()
    {
        if (_fadeOnStart) FadeOut();
    }

    public void FadeIn(float duration = -1)
    {
        SetFade(0);
        PlayFadeAnimation(1, duration);
    }

    public void FadeOut(float duration = -1)
    {
        SetFade(1);
        PlayFadeAnimation(0, duration);
    }

    private void PlayFadeAnimation(float fadeValue, float duration)
    {
        float tweenDuration = duration <= 0 ? _fadeDuration : duration;
        _fadingImage.DOFade(fadeValue, tweenDuration).SetEase(_fadeEase);
    }

    private void SetFade(float value)
    {
        Color color = _fadingImage.color;
        color.a = value;
        _fadingImage.color = color;
    }
}