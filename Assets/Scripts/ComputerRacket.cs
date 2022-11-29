using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerRacket : Racket
{
    public Ball ball;
    public bool isLeft;


    private void Start()
    {
        aiActive = true;
    }

    private void FixedUpdate()
    {
        if (isLeft && aiActive)
        {
            if (ball.GetVelocity().x < 0f)
            {
                BallTowardsRacket();
            }
            else
            {
                BallAwayFromPlayer();
            }
        } else if(!isLeft && aiActive)
        {
            if (ball.GetVelocity().x > 0f)
            {
                BallTowardsRacket();
            }
            else
            {
                BallAwayFromPlayer();
            }
        }
    }

    private void BallTowardsRacket()
    {
        if (ball.GetPosition().y > _rigidbody.position.y)
        {
            _rigidbody.AddForce(Vector2.up * speed);
        }
        else if (ball.GetPosition().y < _rigidbody.position.y)
        {
            _rigidbody.AddForce(Vector2.down * speed);
        }
    }

    private void BallAwayFromPlayer()
    {
        if (_rigidbody.position.y > 0f)
        {
            _rigidbody.AddForce(Vector2.down * speed);
        }
        else if (_rigidbody.position.y < 0f)
        {
            _rigidbody.AddForce(Vector2.up * speed);
        }
    }

    public void SetBall(Ball ball)
    {
        this.ball = ball;
    }

    public void SetLeft(bool left)
    {
        this.isLeft = left;
    }
}
