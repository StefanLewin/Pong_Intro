using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRacket : Racket
{
    private Vector2 _direction;
    private bool isFirstPlayer = false;

    private void Update()
    {
        if (isFirstPlayer)
        {
            PlayerOneControls();
        }
        else
        {
            PlayerTwoControls();
        }
    }

    private void PlayerOneControls()
    {
        if (Gamepad.current.leftStick.up.isPressed || Gamepad.current.dpad.up.isPressed)
        {
            _direction = Vector2.up;
        }
        else if (Gamepad.current.leftStick.down.isPressed || Gamepad.current.dpad.down.isPressed)
        {
            _direction = Vector2.down;
        }
        else
        {
            _direction = Vector2.zero;
        }
    }

    private void PlayerTwoControls()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            _direction = Vector2.up;
        }
        else if (Keyboard.current.downArrowKey.isPressed)
        {
            _direction = Vector2.down;
        }
        else
        {
            _direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if(_direction.sqrMagnitude != 0)
        {
            _rigidbody.AddForce(_direction * this.speed);
        }
    }

    public void setFirstPlayer()
    {
        isFirstPlayer = true;
    }
   
    
}
