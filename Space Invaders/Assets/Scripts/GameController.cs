using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject _player;
    public GameObject _enemies;
    public GameObject[] _bases;
    private float _activateTime;
    //The AudioController is used because is needed to play the victory fanfare when the played destroy all enemies
    public AudioController _audioController;

    void Start()
    {
        _activateTime = 2f;
        Invoke("Spawn", _activateTime);
    }

    public void Spawn()
    {
        _player.gameObject.SetActive(true);
        _enemies.gameObject.SetActive(true);
        _player.transform.position = new Vector3(0, _player.transform.position.y, 0);
        _enemies.transform.position = Vector3.zero;
        _enemies.gameObject.GetComponent<EnemySpawner>().InvokeMove();
    }

    public void Respawn()
    {
        _audioController.PlayClip(_audioController._playerVictory);
        _player.gameObject.SetActive(false);
        _enemies.gameObject.SetActive(false);
        _enemies.GetComponent<EnemySpawner>().SpawnEnemies();
        foreach (GameObject _base in _bases)
        {
            _base.SetActive(true);
        }
        Invoke("Spawn", _activateTime);
    }

    public void Deactivate()
    {
        _player.gameObject.SetActive(false);
        _enemies.gameObject.GetComponent<EnemySpawner>().CancelInvoke();
        _enemies.gameObject.SetActive(false);
        if(_player.gameObject.GetComponent<PlayerController>()._lives > -1)
        {
            Invoke("Spawn", _activateTime);
        }
        else
        {
            _audioController.PlayClip(_audioController._gameOverSound);
            Destroy(_player);
            Destroy(_enemies);
        }
    }
}
