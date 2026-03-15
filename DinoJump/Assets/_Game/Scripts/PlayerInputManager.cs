using UnityEngine;
using UnityEngine.InputSystem;
using System.IO.Ports;

namespace DinoGame
{
    public class PlayerInputManager : MonoBehaviour
    {

        [Header("Input")]
        [SerializeField] InputActionReference _jumpAction;
        [SerializeField] InputActionReference _duckAction;

        [Header("Arduino Connection")]
        [SerializeField] string _portName = "COM6";
        [SerializeField] int _baud = 9600;
        SerialPort _serialPort;
        
        public event System.Action OnJump;
        public event System.Action<bool> OnDuck;
        
        void Awake()
        {
            _serialPort = new(_portName, _baud);
            _serialPort.DtrEnable = true;
            _serialPort.Open();
            _serialPort.ReadTimeout = 5;
        }

        void OnEnable()
        {
#if UNITY_EDITOR
            _jumpAction.action.performed += Jump;
            _duckAction.action.started += Duck;
            _duckAction.action.canceled += Duck;
#endif

            GameManager.OnSetGameRunning += HandleDeath;
        }

        void OnDisable()
        {
#if UNITY_EDITOR
            _jumpAction.action.performed -= Jump;
            _duckAction.action.started -= Duck;
            _duckAction.action.canceled -= Duck;
#endif

            GameManager.OnSetGameRunning -= HandleDeath;
        }

        void OnDestroy()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }

#if UNITY_EDITOR

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

#endif

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
                case "DUCK":
                    OnDuck?.Invoke(true);
                    break;
                case "NO_DUCK":
                    OnDuck?.Invoke(false);
                    break;
                default:
                    break;
            }
        }

        void HandleDeath(bool gameRunning)
        {
            if (_serialPort.IsOpen && !gameRunning)
                _serialPort.WriteLine("DEATH.");
        }

    }
}
