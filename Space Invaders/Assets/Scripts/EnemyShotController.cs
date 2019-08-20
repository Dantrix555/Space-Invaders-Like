using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotController : MonoBehaviour
{
    //The speed must be negative to go Down
    public float _speed;
    //AudioController to call audioclip when needed
    public AudioController _audioController;

    void Start()
    {
        _audioController.PlayClip(_audioController._enemyShot);
    }

    void Update()
    {
        //We move the bullet until it collides with the screen limit
        if (transform.position.y > -4.5)
        {
            transform.position += new Vector3(0, _speed * Time.deltaTime, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //The bullet is a trigger to avoid an unwished move of the enemies by the collision
    void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.gameObject.tag == "Base")
        {
            _audioController.PlayClip(_audioController._baseDestroyed);
            _collision.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        //Destroy the bullet and decrease the lives amount
        if(_collision.gameObject.tag == "Player")
        {
            _audioController.PlayClip(_audioController._playerDeath);
            Destroy(gameObject);
            _collision.gameObject.GetComponent<PlayerController>().UpdateLivesWhenHit();
        }
        if(_collision.gameObject.tag == "Shot")
        {
            Destroy(_collision.gameObject);
            Destroy(gameObject);
        }
    }
}
