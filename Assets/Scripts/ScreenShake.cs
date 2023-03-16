using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    private Vector3 initialPosition;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 1f;

    private bool shakeOnce = false;

    private void Awake()
    {
        instance = this;

        initialPosition = transform.position;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            shakeOnce = !shakeOnce;
            if (shakeOnce)
                transform.localPosition = initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public static void TriggerShake(float duration, float magnitude)
    {
        instance.shakeDuration = duration;
        instance.shakeMagnitude = magnitude;
    }
}
