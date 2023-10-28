using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherBehaviour : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float force;
    [SerializeField] Transform target;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        StartPusher();
    }

    // Push Pusher to the target position
    void StartPusher()
    {
        Vector3 f = target.position - transform.position;
        f = f.normalized;
        f = f * force;
        rb.AddForce(f);
    }

    // Switch target when pusher is on other side
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pusher Limit"))
        {
            if (target == endPos)
                target = startPos;
            else
                target = endPos;
        }
    }
}
