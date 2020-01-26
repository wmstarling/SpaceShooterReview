using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _lowAmmoText;
    [SerializeField]
    private Text _noAmmoText;
    [SerializeField]
    private Image _fuelBar;
    private Player _player;
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }
        if(_player == null)
        {
            Debug.LogError("Player is Null on UI");
        }
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo Count: " + 15;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoText.text = "Ammo Count: " + ammoCount;
        if(ammoCount == 5)
        {
            StartCoroutine(LowAmmoRoutine());
        }
        if(ammoCount == 0)
        {
            StartCoroutine(NoAmmoRoutine());
        }
    }

    public void UpdateFuel(bool thrusterActive)
    {
        if(thrusterActive == true)
        {
            _fuelBar.fillAmount -= 0.01f;
            if(_fuelBar.fillAmount == 0f)
            {
                _player.ThrustActive(false);
            }
        }
        else
        {
            StartCoroutine(RefillFuelRoutine());
        }
        
    }
    IEnumerator LowAmmoRoutine()
    {
        _lowAmmoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        _lowAmmoText.gameObject.SetActive(false);
    }
    IEnumerator NoAmmoRoutine()
    {
        _noAmmoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        _noAmmoText.gameObject.SetActive(false);
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator RefillFuelRoutine()
    {
        while(_fuelBar.fillAmount != 1)
        {
            _fuelBar.fillAmount += 0.1f;
            yield return new WaitForSeconds(0.5f);
            if (_fuelBar.fillAmount == 1)
            {
                _player.ThrustActive(true);
            }
        }
    }
}
