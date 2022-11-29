using UnityEngine;

public class Racket : MonoBehaviour
{
    public float speed = 8f;

    protected Rigidbody2D _rigidbody;
    protected BoxCollider2D _collider;
    protected bool aiActive;

    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void ResetPosition()
    {
        _rigidbody.position = new Vector2(_rigidbody.position.x, 0.0f);
        _rigidbody.velocity = Vector2.zero;
    }

    public void FreeRacket()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.gravityScale = 4.0f;
        _collider.enabled = false;
        _rigidbody.AddForce(new Vector2(10, 10));
        _rigidbody.AddTorque(5);
    }

    public void DeactivateComputer()
    {
        aiActive = false;
    }
}
