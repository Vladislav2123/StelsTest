using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class UIBar : UIPanel
{
    public event Action OnFillChangedEvent;

    [SerializeField] private Image _fillImage;

    private float _fillAmount;

    public Image FillImage => _fillImage;
    public float FillAmount
    {
        get => _fillAmount;
        set
        {
            value = Mathf.Clamp01(value);
            if (value == _fillAmount) return;

            _fillAmount = value;
            RefreshImage();
            OnFillChangedEvent?.Invoke();
        }
    }

    public bool Animations { get; set; } = true;

    protected abstract void RefreshImage();
}