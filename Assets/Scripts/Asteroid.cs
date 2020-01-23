using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;
    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn_Manager is NUll");
        }
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Asteroid is Null");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            _audioSource.Play();
            Destroy(this.gameObject, 1.0f);
        }
    }
}
