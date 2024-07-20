using System;
using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{
    [SerializeField] Vector3 rotationDirection;
    [SerializeField] float rotationSpeed;
    [SerializeField] Ease easing;

    void Start ()
    {
        transform.DORotate(rotationDirection, rotationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(easing)
            .SetSpeedBased();
    }
}
