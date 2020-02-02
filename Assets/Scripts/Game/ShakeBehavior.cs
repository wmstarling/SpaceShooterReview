using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehavior : MonoBehaviour
{
    private Transform _transform;
    public float _shakeDuration = 0f; //duration of shake
    private float _shakeMag = 0.2f; //How shakey, magnitiude
    private float _dampSpeed = 1.0f; //how quickly it wears off

    Vector3 initialPostion; //initial position of the GameObject;

    private void Awake()
    {
        if (transform == null)
        {
            _transform = GetComponent(typeof(Transform)) as Transform; //get camera transform
        }
    }
    private void OnEnable()
    {
        initialPostion = transform.localPosition; //store init pos of camera
    }

    private void Update()
    {
        if(_shakeDuration > 0)
        {
            transform.localPosition = initialPostion + Random.insideUnitSphere * _shakeMag;

            _shakeDuration -= Time.deltaTime * _dampSpeed;
        }
        else
        {
            _shakeDuration = 0f;
            transform.localPosition = initialPostion;
        }
    }
    public void TriggerShake()
    {
        _shakeDuration = 0.5f;
    }
    public void DeadShake()
    {
        _shakeDuration = 2.0f;
    }
}
