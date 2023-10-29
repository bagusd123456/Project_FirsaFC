using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    Vector3 defaultPosition;
    Vector3 targetPosition;
    public Vector3 offset;
    public Transform targetTransform;
    public float duration;

    bool canAnimate = true;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Sequence s = DOTween.Sequence();
        s.SetUpdate(UpdateType.Normal, true);
        s.Append(targetTransform.DOMove(targetPosition, duration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null, eventData);
        Sequence s = DOTween.Sequence();
        s.SetUpdate(UpdateType.Normal, true);
        s.Append(targetTransform.DOMove(defaultPosition, duration));
    }

    private void OnEnable()
    {
        canAnimate = true;
    }

    private void OnDisable()
    {
        targetTransform.position = defaultPosition;
        canAnimate = false;
    }

    private void Awake()
    {
        defaultPosition = targetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = targetTransform.position + offset;
    }
}
