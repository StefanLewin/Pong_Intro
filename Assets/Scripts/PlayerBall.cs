using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBall : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    
    public float speed = 400;
    public float dragFactor = 10.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Gamepad.current.leftStick.left.isPressed || Gamepad.current.dpad.left.isPressed)
        {
            _direction = Vector2.left;
        }
        else if (Gamepad.current.leftStick.right.isPressed || Gamepad.current.dpad.right.isPressed)
        {
            _direction = Vector2.right;
        }
        else
        {
            _direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (_direction.sqrMagnitude != 0)
        {
            _rigidbody.AddForce(_direction * this.speed);
            
        }
        else
        {
            _rigidbody.AddForce(new Vector2(-(_rigidbody.velocity.x * dragFactor), 0));
        }
    }


}
