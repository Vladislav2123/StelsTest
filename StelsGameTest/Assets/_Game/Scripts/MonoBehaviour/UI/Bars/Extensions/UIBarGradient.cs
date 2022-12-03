using UnityEngine;
using UnityEngine.UI;

public class UIBarGradient : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Gradient _gradient;

    private UIBar _bar;

    private void Awake()
    {
        _bar = GetComponent<UIBar>();

        _bar.OnFillChangedEvent += PaintFill;
    }

    private void PaintFill()
    {
        _fillImage.color = _gradient.Evaluate(_bar.FillAmount);
    }
}
