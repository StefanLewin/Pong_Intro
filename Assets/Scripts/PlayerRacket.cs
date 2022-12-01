using UnityEngine;

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
        if (Input.GetKey(KeyCode.W))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
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
