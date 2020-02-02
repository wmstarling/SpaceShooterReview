using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    private bool _isSmartLaser = false;
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y >= 8f)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -8f)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    public void AssignSmartLaser()
    {
        _isSmartLaser = true;
    }

    public bool CheckEnemyLaser()
    {
        return _isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
        }
        else if(other.tag == "Player" && _isSmartLaser == true)
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
        }
        else if(other.tag == "Powerup" && _isEnemyLaser == true)
        {
            Destroy(this.gameObject);
        }
    }

    public bool isEnemyLaser()
    {
        return _isEnemyLaser;
    }
}
