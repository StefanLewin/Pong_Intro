using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    
    public float speed = 100;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
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
    }


}
