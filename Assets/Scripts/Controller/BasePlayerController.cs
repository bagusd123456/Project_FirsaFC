using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BasePlayerController : MonoBehaviour
{
    [SerializeField] private float _rollSpeed = 5f;
    private bool _isMoving;

    private bool _isOnPlatform;
    private Transform platformBody;
    private Vector3 lastPlatformPosition;

    public Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOnPlatform)
        {
            Vector3 deltaPosition = platformBody.position - lastPlatformPosition;;
            _rb.position = _rb.position + deltaPosition;
            lastPlatformPosition = platformBody.position;
        }

        if (_isMoving) return;

        if (Input.GetKeyDown(KeyCode.A)) Assemble(Vector3.left);
        if (Input.GetKeyDown(KeyCode.D)) Assemble(Vector3.right);
        if (Input.GetKeyDown(KeyCode.W)) Assemble(Vector3.forward);
        if (Input.GetKeyDown(KeyCode.S)) Assemble(Vector3.back);


        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 0.5f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }

        
        //rb.MovePosition(q * (rb.transform.position - target.position) + target.position);
        
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;
        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            //Quaternion q = Quaternion.AngleAxis(_rollSpeed, transform.forward);
            //_rb.MoveRotation(_rb.transform.rotation * q);
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }

        _isMoving = false;
    }

    //Check if collider with movingPlatform
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            platformBody = other.gameObject.GetComponent<Transform>();
            lastPlatformPosition = platformBody.position;
            Debug.Log($"Velocity: {platformBody.transform}");
            _isOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            _isOnPlatform = false;
            platformBody = null;
        }
    }
}
