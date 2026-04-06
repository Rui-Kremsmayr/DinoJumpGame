using UnityEngine;

public class Dino : MonoBehaviour
{
    
    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _anim;
    [SerializeField] float _jumpForce = 10;

    bool _onGround = false;

    void OnEnable()
    {
        _playerInputManager.OnJump += StartGame;
    }


    void StartGame()
    {
        _playerInputManager.OnJump -= StartGame;
        _playerInputManager.OnJump += Jump;
        _playerInputManager.OnDuck += Duck;

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

    void Duck(bool duck)
    {
        if (_onGround)
            _anim.SetBool("Duck", duck);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(Tags.T_Ground))
        {
            _anim.SetBool("Jump", false);
            _onGround = true;
            _rb.linearVelocityY = 0;
        }
    }


}
