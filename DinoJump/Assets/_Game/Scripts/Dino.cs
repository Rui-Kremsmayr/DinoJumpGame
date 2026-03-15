using UnityEngine;

namespace DinoGame
{
    public class Dino : MonoBehaviour
    {

        [SerializeField] PlayerInputManager _playerInputManager;
        [SerializeField] Rigidbody2D _rb;
        [SerializeField] Animator _anim;

        [Header("Values")]
        [SerializeField] float _jumpForce = 2;

        bool _onGround;


        void OnEnable()
        {
            _playerInputManager.OnJump += StartGame;
        }

        void StartGame()
        {
            _playerInputManager.OnJump -= StartGame;
            _playerInputManager.OnJump += Jump;
            _playerInputManager.OnDuck += Duck;

            GameManager.OnSetGameRunning?.Invoke(true);
            _onGround = true;

            Jump();
        }

        void Jump()
        {
            if (!_onGround)
                return;

            _onGround = false;
            _rb.AddForceY(_jumpForce, ForceMode2D.Impulse);
            _rb.linearVelocityY = Mathf.Clamp(_rb.linearVelocityY, 0, _jumpForce);
            _anim.SetBool("Jump", true);
        }

        void Duck(bool ducking)
        {
            if (_onGround)
                _anim.SetBool("Duck", ducking);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.T_Obstacle))
            {
                _anim.SetTrigger("Death");

                _playerInputManager.OnJump -= Jump;
                _playerInputManager.OnDuck -= Duck;

                GameManager.OnSetGameRunning?.Invoke(false);

                _playerInputManager.OnJump += StartGame;
            }        
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(Tags.T_Ground))
            {
                _anim.SetBool("Jump", false);
                _onGround = true;
                _rb.linearVelocityY = 0;
            }
        }


    }
}
