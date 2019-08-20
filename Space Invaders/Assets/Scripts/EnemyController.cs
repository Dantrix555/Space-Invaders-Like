using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int _score;
    private GameObject _player;
    //AudioController to call audioclip when needed
    public AudioController _audioController;

    void Start()
    {
        _player = GameObject.Find("Player");
        _score = 100;
    }

    void OnCollisionEnter2D(Collision2D _other)
    {
        //Verify if the enemy has collide with the bullet, if it's true the enemy and bullet disapears
        if(_other.gameObject.tag == "Shot")
        {
            _audioController.PlayClip(_audioController._enemyDeath);
            _player.GetComponent<PlayerController>().UpdateScore(_score);
            Destroy(_other.gameObject);
            Destroy(gameObject);
        }
        if(_other.gameObject.tag == "Base")
        {
            _audioController.PlayClip(_audioController._baseDestroyed);
            _other.gameObject.SetActive(false);
        }
    }
}
