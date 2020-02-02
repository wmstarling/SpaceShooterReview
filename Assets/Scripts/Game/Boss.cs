using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private int _hitCount = 10;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is null on Boss");
        }
    }
    void Update()
    {
        CalculateBossMovement();
    }

    void CalculateBossMovement()
    {
        if (transform.position.y > 2.0f)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player.Damage();
            //damage player
        }
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _hitCount--;
            if(_hitCount == 0)
            {
                //Destroy Boss
                //
            }
        }
        if(other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _hitCount--;
            if(_hitCount == 0)
            {
                //Destroy Boss
            }
        }
    }
}
