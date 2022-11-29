using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 1f;
    public Transform target;
    [SerializeField] protected Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 newPos = new Vector3(target.position.x, -0.5f, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity,  FollowSpeed * Time.deltaTime);
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }
}
