using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObjectBehaviour : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public bool timeToStop = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        if (timeToStop)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = -Vector3.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser Limit"))
        {
            timeToStop = true;
            rb.isKinematic = true;
        }

        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            UIScript.instance.PlayerLose();
        }
    }
}
