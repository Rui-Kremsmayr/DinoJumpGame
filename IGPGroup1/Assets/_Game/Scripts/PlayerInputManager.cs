using System.IO.Ports;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    
    [SerializeField] InputActionReference _jumpAction;
    [SerializeField] InputActionReference _duckAction;

    [Header("Arduino Input")]
    [SerializeField] string _portName = "COM6";
    [SerializeField] int _baud = 9600;
    SerialPort _serialPort;

    public event System.Action OnJump;
    public event System.Action<bool> OnDuck;


    void Awake()
    {
        _serialPort = new(_portName, _baud);
        _serialPort.Open();
        _serialPort.ReadTimeout = 5;
        _serialPort.WriteTimeout = 5;
    }

    void OnEnable()
    {
        _jumpAction.action.performed += Jump;
        _duckAction.action.started += Duck;
        _duckAction.action.canceled += Duck;
    }

    void OnDisable()
    {
        _jumpAction.action.performed -= Jump;
        _duckAction.action.started -= Duck;
        _duckAction.action.canceled -= Duck;
    }

    void Update()
    {
        if (!_serialPort.IsOpen)
            return;

        string input = _serialPort.ReadLine();
        if (!string.IsNullOrEmpty(input))
            ParseInput(input);
    }


    void ParseInput(string input)
    {
        switch (input)
        {
            case "JUMP":
                OnJump?.Invoke();
                break;
            default:
                break;
        }
    }

    void OnDestroy()
    {
        if (_serialPort.IsOpen)
            _serialPort.Close();
    }


    void Jump(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    void Duck(InputAction.CallbackContext context)
    {
        if (context.started)
            OnDuck?.Invoke(true);
        else if (context.canceled)
            OnDuck?.Invoke(false);
    }


}
