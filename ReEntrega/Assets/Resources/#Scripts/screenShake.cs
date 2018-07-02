using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenShake : MonoBehaviour
{

    public enum shakersPrefabs { softShot, medShot, hardShot, softColl, hardColl, explosion };

    public shakersPrefabs shakers;
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration;
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount;
    public float decreaseFactor;

    float _shakeDuration;
    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        initShakeType();
        _shakeDuration = shakeDuration;

        originalPos = camTransform.localPosition;
    }



    void initShakeType()
    {
        if (shakers == shakersPrefabs.softShot)
        {
            shakeDuration = 0.05f;
            shakeAmount = 0.015f;
            decreaseFactor = 1;
        }
        if (shakers == shakersPrefabs.medShot)
        {
            shakeDuration = 0.05f;
            shakeAmount = 0.03f;
            decreaseFactor = 1;
        }
        if (shakers == shakersPrefabs.hardShot)
        {
            shakeDuration = 0.05f;
            shakeAmount = 0.04f;
            decreaseFactor = 1;
        }
        if (shakers == shakersPrefabs.softColl)
        {
            shakeDuration = 0.15f;
            shakeAmount = 0.01f;
            decreaseFactor = 1;

        }
        if (shakers == shakersPrefabs.hardColl)
        {
            shakeDuration = 0.15f;
            shakeAmount = 0.02f;
            decreaseFactor = 1;
        }
        if (shakers == shakersPrefabs.explosion)
        {
            shakeDuration = 0.15f;
            shakeAmount = 0.15f;
            decreaseFactor = 1f;

        }




    }
    void Update()
    {
        if (_shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            _shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            enabled = false;
            camTransform.localPosition = originalPos;
        }
    }
}