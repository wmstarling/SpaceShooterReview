using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Transform _target;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _smartLaser;
    private float _fireRate = 3.0f;
    private float _canfire = -1;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private bool _isShieldActive = true;
    [SerializeField]
    private int _enemyID; //enemy = 0, shield = 1, aggro = 2, smart = 3, avoid = 4
    private Laser _laser;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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

            if (_enemyID == 3)
            {
                float xPos = Mathf.Abs(transform.position.x - _target.position.x);
                float yPos = transform.position.y;
                if(xPos < 10.0f && yPos < 2)
                {
                    Debug.Log("Shoot Smart Laser");
                    GameObject smartyLaser = Instantiate(_smartLaser, new Vector3(transform.position.x, (transform.position.y + 1.52f), 0), Quaternion.identity);
                    Laser lasers = smartyLaser.GetComponent<Laser>();
                    lasers.AssignSmartLaser();
                    _fireRate = Random.Range(3f, 7f);
                    _canfire = Time.time + _fireRate;
                } 
            }
            else
            {
                _fireRate = Random.Range(3f, 7f);
                _canfire = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
        }
    }
    void CalculateEnemyMovement()
    {
        if (transform.position.y <= -7f)
        {
            float randomX = Random.Range(-9f, 8.3f);
            transform.position = new Vector3(randomX, 6.5f, 0);
        }
        if(_enemyID == 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
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
            if(_isShieldActive == true && _enemyID == 1)
            {
                _isShieldActive = false;
                _shieldVisual.SetActive(false);
            }
            else
            {
                _animator.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            } 
        }
        if (other.tag == "Laser" && _enemyID != 4)
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            if (_isShieldActive == true && _enemyID == 1)
            {
                _isShieldActive = false;
                _shieldVisual.SetActive(false);
            }
            else
            {
                _animator.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
        }
        else if(other.tag == "Laser" && _enemyID == 4)
        {
                float laserX = other.transform.position.x;
                float Xdifference = transform.position.x - laserX;
                if (Xdifference < -1.2f) //if X difference is negative move left
                {
                    transform.Translate(Vector3.left * _speed * Time.deltaTime);
                }
                else if (Xdifference < 1.2f) //if X different is positive move right
                {
                    transform.Translate(Vector3.right * _speed * Time.deltaTime);
                }
        }
        if(other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _player = GameObject.Find("Player").GetComponent<Player>();
            if(_player != null)
            {
                _player.AddScore(10);
            }
            else
            {
                Debug.LogError("Player null in missile on enemy");
            }
            if(this.gameObject == null)
            {
                Debug.LogError("Own gameobj on enemy is null");
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
