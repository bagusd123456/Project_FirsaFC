using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using DG.Tweening;

public class BasePlayerController : MonoBehaviour
{
    public float rayDistance = 1f;
    public Rigidbody _rb;
    [SerializeField] private float _rollSpeed = 5f;
    private bool _isMoving;

    private bool _isOnPlatform;
    private Transform platformBody;
    private Vector3 lastPlatformPosition;

    public LayerMask WhatIsGround;
    public AnimationCurve animCurve;

    public float time;

    public bool canMove = true;
    public bool isDead = false;

    public InputAction playerInputAction;
    private Vector2 movementInput = Vector2.zero;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        if (canMove && !_isMoving && !isDead)
        {
            if (movementInput.x > 0.5f) Assemble(Vector3.right);
            if (movementInput.x < -0.5f) Assemble(Vector3.left);
            if (movementInput.y > 0.5f) Assemble(Vector3.forward);
            if (movementInput.y < -0.5f) Assemble(Vector3.back);
        }

        canMove = context.action.triggered && movementInput == Vector2.zero;
        //canMove = context.action.triggered && (movementInput.x == 0 || movementInput.y == 0);
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

        Quaternion targetRotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.eulerAngles.x / 90) * 90,
            Mathf.RoundToInt(transform.rotation.eulerAngles.y / 90) * 90,
            Mathf.RoundToInt(transform.rotation.eulerAngles.z / 90) * 90);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
            animCurve.Evaluate(time));

        //if (_isMoving) return;

        //if (canMove)
        //{
        //    if (movementInput.x > 0.5f) Assemble(Vector3.right);
        //    if (movementInput.x < -0.5f) Assemble(Vector3.left);
        //    if (movementInput.y > 0.5f) Assemble(Vector3.forward);
        //    if (movementInput.y < -0.5f) Assemble(Vector3.back);
        //}
    }

    void Assemble(Vector3 dir)
    {
        var GO = GetObjectAround(dir);
        if (GO != null)
        {
            if (GO.CompareTag("Collider"))
            {
                animator.SetTrigger("Stumble");
                PlayJumpAnimation();
                Debug.Log("Play Stumble Animation");
                return;
            }
        }

        canMove = false;
        var anchor = transform.position + (Vector3.down + dir) * 0.5f * transform.localScale.x;
        var axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;
        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            _rb.freezeRotation = true;
            yield return new WaitForSeconds(0.01f);
            
        }
        _rb.freezeRotation = false;
        _isMoving = false;

        // CHECK IF PLAYER IS ON TOP OF PLATFORM OR WATER
        if (GetSurfaceObject().CompareTag("DeadCollider"))
        {
            isDead = true;
            PlayDeadAnimation();
            Debug.Log("You Dead");
            UIScript.instance.PlayerLose();
            GetComponent<Animator>().SetBool("playerLose", true);
        }

        if (movementInput == Vector2.zero)
        {
            canMove = true;
        }
    }

    [ContextMenu("ClampRotation")]
    public void ClampRotation()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit info = new RaycastHit();

        if (Physics.Raycast(ray, out info, WhatIsGround))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal),
                animCurve.Evaluate(time));
        }
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

    private float SurfaceRotation()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit info = new RaycastHit();
        float angle = 0;
        if (Physics.Raycast(ray, out info, WhatIsGround))
        {
            angle = Vector3.Angle(transform.up, info.normal);
        }

        return angle;
    }

    public GameObject GetSurfaceObject()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit info = new RaycastHit();
        GameObject GO = null;

        if (Physics.Raycast(ray, out info, WhatIsGround))
        {
            GO = info.collider.gameObject;
        }

        return GO;
    }

    public GameObject GetObjectAround(Vector3 direction)
    {
        GameObject GO = null;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit info = new RaycastHit();

        if (Physics.Raycast(ray, out info, rayDistance, WhatIsGround))
        {
            GO = info.collider.gameObject;
        }

        return GO;
    }

    [ContextMenu("PlayDeadAnimation")]
    public void PlayDeadAnimation()
    {
        var mySequence = DOTween.Sequence();
        float endPosition = transform.position.y + 4;
        float endScale = transform.localScale.x + 1.2f;
        float startScale = transform.localScale.x;

        var tween = transform.DOMoveY(endPosition, 0.5f).SetEase(Ease.InOutSine).SetDelay(0.05f);
        //test.OnStart(() => { transform.DOScale(endScale, 0.5f); });
        //test.OnComplete(() => { transform.DOScale(startScale, 0.5f); });
        var tween2 = transform.DOMoveY(endPosition - 8, 0.25f).SetEase(Ease.OutFlash);

        mySequence.Append(tween);

        mySequence.Append(transform.DOScaleY(.6f, .2f));
        mySequence.Insert(.5f, transform.DOScaleX(1.2f, .2f));
        mySequence.Insert(.5f, transform.DOScaleZ(1.2f, .2f));

        mySequence.Append(tween2);
    }

    [ContextMenu("PlayJumpAnimation")]
    public void PlayJumpAnimation()
    {
        var mySequence = DOTween.Sequence();
        float endPosition = transform.position.y + 0.5f;
        float startPosition = transform.position.y;

        var tween = transform.DOMoveY(endPosition, 0.1f).SetEase(Ease.InSine);
        var tween2 = transform.DOMoveY(endPosition, 0.1f).SetEase(Ease.OutSine);
        mySequence.Append(tween);
        mySequence.Append(tween2);
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.left * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.right * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.forward * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.back * rayDistance);
    }
#endif
}
