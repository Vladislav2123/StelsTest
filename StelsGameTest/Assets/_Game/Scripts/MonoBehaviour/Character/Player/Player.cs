using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public event Action OnVolumeChangedEvent;
    public event Action OnMaxVolumeEvent;

    [Header("Volume")]
    [SerializeField] private float _volumeUpSpeed = 3;
    [SerializeField] private float _volumeDownSpeed = 1;
    [SerializeField] private float _maxVolume = 10f;

    public CharacterMovement Movement { get; private set; }

    private float _volume;
    public float Volume
    {
        get => _volume;
        private set
        {
            value = Mathf.Clamp(value, 0, _maxVolume);
            if (value == _volume) return;

            _volume = value;

            OnVolumeChangedEvent?.Invoke();

            if (_volume == _maxVolume) OnMaxVolumeEvent?.Invoke();
        }
    }

    public float MaxVolume => _maxVolume;

    private void Awake()
    {
        Movement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        HandleVolume();
    }

    private void HandleVolume()
    {
        if (Movement.IsMoving) Volume += _volumeUpSpeed * Time.deltaTime;
        else Volume -= _volumeDownSpeed * 2 * Time.deltaTime;
    }
}
