using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private InputActions _input;
    public InputActions Input
    {
        get
        {
            if (_input == null)
            {
                _input = new InputActions();
                Enabled = true;
            }

            return _input;
        }
    }

    private bool _enabled;
    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            if (value) Input.Enable();
            else Input.Disable();
        }
    }
}
