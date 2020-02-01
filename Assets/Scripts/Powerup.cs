using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0 = triple, 1 = speed, 2 = shields, 3 = ammo, 4 = negative speed, 5 = health, 6 = split shot, 7 = homing missile
    private int _powerupID;
    [SerializeField]
    private AudioClip _pickupSound;
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
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
                    default:
                        Debug.LogError("Invalid Selection for Powerup");
                        break;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
