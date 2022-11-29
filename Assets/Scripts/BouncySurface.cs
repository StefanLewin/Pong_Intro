using UnityEngine;

public class BouncySurface : MonoBehaviour
{
    public float strength;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            Vector2 normal = collision.GetContact(0).normal;
            ball.AddForce(-normal * strength);
        }
    }
}
