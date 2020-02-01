using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    private float _thrusterMultiplier = 3.0f;
    private float _speedPlusThrust = 4.0f;
    private float _speedNegative = 2.0f;
    [SerializeField]
    private bool _canThrust = true;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _splitPrefab;
    [SerializeField]
    private GameObject _homingPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private bool _fireAllowed = true;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private bool _isSpeedActive = false;
    private bool _isTripleShotActive = false;
    private bool _isSpeedNegative = false;
    private bool _isHomingActive = false;
    [SerializeField]
    private bool _isSplitActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private int _shieldHitCount = 3;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private int _score;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;
    private ShakeBehavior _shakeBehavior;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shakeBehavior = GameObject.Find("Main Camera").GetComponent<ShakeBehavior>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn_Manager is Null");
        }
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is Null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio source on player is Null");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
        if(_shakeBehavior == null)
        {
            Debug.LogError("Shake on Main Camera is Null");
        }
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKey(KeyCode.LeftShift) && _canThrust == true)
        {
            transform.Translate(direction * (_speed * _thrusterMultiplier) * Time.deltaTime);
            _uiManager.UpdateFuel(_canThrust);
        }
        else if(Input.GetKey(KeyCode.LeftShift) && _isSpeedActive == true && _canThrust == true)
        {
            transform.Translate(direction * (_speed * _speedPlusThrust) * Time.deltaTime);
            _uiManager.UpdateFuel(_canThrust);
        }
        else if(_isSpeedActive == true)
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        else if(_isSpeedNegative == true)
        {
            transform.Translate(direction * _speedNegative * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        //below line keeps players from going above or below a minimum or maximum on the specified axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.9f, 0), 0);

        if (transform.position.x >= 10.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(10.3f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
         _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _audioSource.Play();
        }
        else if(_isTripleShotActive == false && _fireAllowed == true && _isSplitActive == false && _isHomingActive == false)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _audioSource.Play();
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
            if (_ammoCount == 0)
            {
                _fireAllowed = false;
            }
        }
        else if(_isSplitActive == true)
        {
            Instantiate(_splitPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _audioSource.Play();
        }
        else if (_isHomingActive == true)
        {
            Instantiate(_homingPrefab, transform.position + new Vector3(0, 1.42f, 0), Quaternion.identity);
        }
    }
    public void Damage()
    {
        if(_isShieldActive == true)
        {
            switch (_shieldHitCount)
            {
                case 0:
                    _isShieldActive = false;
                    _shieldVisual.SetActive(false);
                    _shieldHitCount = 3;
                    break;
                case 1:
                    _shieldVisual.GetComponent<SpriteRenderer>().color = Color.red;
                    _shieldHitCount--;
                    break;
                case 2:
                    _shieldVisual.GetComponent<SpriteRenderer>().color = Color.magenta;
                    _shieldHitCount--;
                    break;
                case 3:
                    _shieldVisual.GetComponent<SpriteRenderer>().color = Color.yellow;
                    _shieldHitCount--;
                    break;
            }
            return;
        }
        _lives--;
        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
            _shakeBehavior.TriggerShake();
        }
        else if(_lives == 1)
        {
            _leftEngine.SetActive(true);
            _shakeBehavior.TriggerShake();
        }
        _uiManager.UpdateLives(_lives);
        if(_lives < 1)
        {
            _shakeBehavior.DeadShake();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void ShieldsActive()
    {
        _isShieldActive = true;
        _shieldVisual.SetActive(true);
        _shieldHitCount = 3;
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }
    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedActive()
    {
        _isSpeedActive = true;
        StartCoroutine(SpeedPowerDown());
    }
    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
    }
    public void SplitShotActive()
    {
        _isSplitActive = true;
        StartCoroutine(SplitShotPowerDown());
    }

    IEnumerator SplitShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSplitActive = false;
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    public void RefillAmmo()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
    }
    public void HealthPickup()
    {
        _lives++;
        if(_lives == 3)
        {
            _rightEngine.SetActive(false);
        }
        else if(_lives == 2)
        {
            _leftEngine.SetActive(false);
        }
        _uiManager.UpdateLives(_lives);
    }
    public void ThrustActive(bool activeThrust)
    {
        if(activeThrust == true)
        {
            _canThrust = true;
        }
        else
        {
            _canThrust = false;
            _uiManager.UpdateFuel(_canThrust);
        }
    }

    public void NegativeSpeed()
    {
        _isSpeedNegative = true;
        StartCoroutine(BadSpeedRoutine());
    }
    IEnumerator BadSpeedRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedNegative = false;
    }
    public void HomingActive()
    {
        StartCoroutine(HomingShotRoutine());   
    }
    IEnumerator HomingShotRoutine()
    {
        _isHomingActive = true;
        yield return new WaitForSeconds(5.0f);
        _isHomingActive = false;
    }
}
