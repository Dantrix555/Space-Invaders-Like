using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject _player;
    public GameObject _enemy;
    public GameObject _actualEnemy;
    public GameObject _enemyBullet;
    private GameObject _actualBullet;
    private Vector3 _enemySpawnPosition;
    public float _speed;
    private float _randomEnemyShootRate;
    private bool _enemiesCanShoot;
    //This variable is to control the calls to invoke function when the repeat rate variable is changed
    private bool _activateRepeatRate;
    //The repeat rate is also used in the game controller script
    public float _repeatRate;
    //HUD controller is called when all enemies are destroyed
    public HUD_Controller _hudController;
    //The game controller is used when all enemies are destroyed
    public GameController _gameController;
    //This audio controller is to attach to the enemies then is destroyed from this object
    public AudioController _audioController;

    void Start()
    {
        //Invokes the movement repeatidily without the need of Update function
        _randomEnemyShootRate = 0.999f;
        _repeatRate = 0.3f;
        _activateRepeatRate = true;
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemiesCanShoot = false;
        transform.position = Vector3.zero;
        int x, y;
        for (x = -4; x <= 4; x++)
        {
            for (y = 3; y >= -1; y--)
            {
                _enemySpawnPosition = new Vector3(x, y + 0.65f, 0);
                _actualEnemy = Instantiate(_enemy, _enemySpawnPosition, Quaternion.identity, transform);
                //This line set the audioController to the child object, in this case the enemy
                _actualEnemy.GetComponent<EnemyController>()._audioController = _audioController;
            }
        }
        _enemiesCanShoot = true;
        InvokeMove();
    }

    void MoveEnemies()
    {
        transform.position += Vector3.right * _speed;
        foreach(Transform _enemy in transform)
        {
            //Verifies if some enemy is going out of boundary and change the direction of the spawner (all enemies basically)
            if(_enemy.position.x > 5.4f || _enemy.position.x < -5.4f)
            {
                _speed *= -1;
                transform.position += Vector3.down * 0.3f;
                //When the first enemy position out of the boundary, isn't needed to detect the others positions
                return;
            }

            if(_actualBullet == null)
            {
                _enemiesCanShoot = true;
            }

            //Random enemy shoot
            if(Random.value > _randomEnemyShootRate && _enemiesCanShoot && gameObject.activeSelf)
            {
                Vector3 _bulletSpawnPosition = new Vector3(_enemy.transform.position.x, _enemy.transform.position.y -0.4f, 0);
                _actualBullet = Instantiate(_enemyBullet, _bulletSpawnPosition, _enemy.transform.rotation);
                _actualBullet.GetComponent<EnemyShotController>()._audioController = _audioController;
                _enemiesCanShoot = false;
            }

            //Verifies if the enemy is in the player zone and player will lose one life
            if(_enemy.transform.position.y < -4.2)
            {
                Debug.Log("Player destroyed");
                _player.GetComponent<PlayerController>().UpdateLivesWhenHit();
                return;
            }
        }
        //Every time certain amount of enemies are destroyed, the call speed to MoveEnemies() is increased 
        if(transform.childCount == 22 && _activateRepeatRate && _repeatRate > 0.07f)
        {
            DecreaseRate(transform.childCount, 0.03f);
        }
        //When is reactivated the repeat rate is beacuse when another amount of enemies are destroyed is important to increase the MoveEnemies() call speed
        if(transform.childCount == 21)
        {
            _activateRepeatRate = true;
        }
        //Every time certain amount of enemies are destroyed, the call speed to MoveEnemies() is increased 
        if(transform.childCount == 5 && _activateRepeatRate && _repeatRate > 0.07f)
        {
            DecreaseRate(transform.childCount, 0.04f);
        }
        //When is reactivated the repeat rate is beacuse when another amount of enemies are destroyed is important to increase the MoveEnemies() call speed
        if(transform.childCount == 4)
        {
            _activateRepeatRate = true;
        }
        //Restart the enemies in case the player is alive
        if(transform.childCount == 0)
        {
            CancelInvoke();
            _gameController.Respawn();
        }
    }

    public void InvokeMove()
    {
        InvokeRepeating("MoveEnemies", 0.1f, _repeatRate);
    }

    void DecreaseRate(int _childObjects, float _decreaseValues)
    {
        CancelInvoke();
        _repeatRate -= _decreaseValues;
        _activateRepeatRate = false;
        InvokeMove();
    }
}
