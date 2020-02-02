using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _speed = 4.0f;
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }
}
