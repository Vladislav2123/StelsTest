using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MapNoiseDisplay : UIPanel
{
    [SerializeField] private RawImage _textureImage;

    [Inject] private MapController _mapController;

    public void DisplayNoise(Texture2D texture)
    {
        _textureImage.texture = texture;
    }

    public void OnGenerateButton()
    {
        _mapController.GenerateMap();
    }
}
