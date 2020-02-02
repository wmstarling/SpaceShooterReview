using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0 = triple, 1 = speed, 2 = shields, 3 = ammo, 4 = negative speed, 5 = health, 6 = split shot, 7 = homing missile, 8 = magnet
    private int _powerupID;
    [SerializeField]
    private AudioClip _pickupSound;
    [SerializeField]
    private Transform _target;
    private bool _magnetActive = false;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        if(_magnetActive == true)
        {

            transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if(transform.position.y <= -8.0f)
        {
            Destroy(this.gameObject);
        }        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
            if(player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.RefillAmmo();
                        break;
                    case 4:
                        player.NegativeSpeed();
                        break;
                    case 5:
                        player.HealthPickup();
                        break;
                    case 6:
                        player.SplitShotActive();
                        break;
                    case 7:
                        player.HomingActive();
                        break;
                    case 8:
                        player.MagnetActive();
                        break;
                    default:
                        Debug.LogError("Invalid Selection for Powerup");
                        break;
                }
                Destroy(this.gameObject);
            }
        }
        if(collision.tag == "Laser")
        {
            Laser lasers = collision.GetComponent<Laser>();
            if(lasers != null)
            {
                bool isEnemy = lasers.isEnemyLaser();
                if(isEnemy == true)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
    public void ActiveMagnet()
    {
        StartCoroutine(MagnetActiveRoutine());
    }
    IEnumerator MagnetActiveRoutine()
    {
        _magnetActive = true;
        yield return new WaitForSeconds(10.0f);
        _magnetActive = false;
    }
}
