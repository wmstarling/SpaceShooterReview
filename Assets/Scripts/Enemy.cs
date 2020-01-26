﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Animator _animator;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canfire = -1;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if(_player == null)
        {
            Debug.LogError("Player is null in Enemy");
        }
        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Animator is Null in Enemy");
        }
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is Null");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
    }
    void Update()
    {

        CalculateEnemyMovement();

        if(Time.time > _canfire && _speed != 0)
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i <lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }
    void CalculateEnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -7f)
        {
            float randomX = Random.Range(-9f, 8.3f);
            transform.position = new Vector3(randomX, 6.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(_player != null)
            {
                _player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
