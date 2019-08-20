using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables to move the ship
    private float _moveDirection;
    public float _speed;
    //Variables for the instatiate of the bullet
    public GameObject _bullet;
    public float _yBulletInstancePosition;
    private Vector3 _bulletInstatiatePosition;
    //Variables to control the amount of bullets shot by the player
    private bool _canShoot;
    private float _waitTimeToshoot;
    private float _totalTimeWaited;
    //Variables for lives, score points and HUD
    public int _lives;
    private int _totalScore;
    public HUD_Controller _hudController;
    //GameController reference to desactivate objects when player is hit with an enemy shot or directly an enemy
    public GameController _gameController;
    //AudioController to call audioclip when needed
    public AudioController _audioController;

    void Start()
    {
        //Initialization of the values of wait time, shooting, lives and score
        _canShoot = true;
        _waitTimeToshoot = 0.45f;
        _totalTimeWaited = 0;
        _lives = 3;
        _totalScore = 0;
        _hudController.UpdateLives(_lives);
        _hudController.UpdateScore(_totalScore);
        _hudController.UpdateHighScore(_totalScore);
    }

    void Update()
    {
        Movement();
        Shoot();
    }

    void Movement()
    {

        _moveDirection = Input.GetAxis("Horizontal");
        //Clamp for the right side of the screen
        if ((_moveDirection > 0 && transform.position.x >= 5.3f))
        {
            transform.position = new Vector3(5.3f, transform.position.y, 0);
        }
        //Clamp for the left side of the screen
        else if ((_moveDirection < 0 && transform.position.x <= -5.3f))
        {
            transform.position = new Vector3(-5.3f, transform.position.y, 0);
        }
        //Move if the player isn't in the boundary position
        else if (_moveDirection != 0)
        {
            transform.position += new Vector3(_moveDirection *_speed * Time.deltaTime, 0, 0);
        }
    }

    void Shoot()
    {
        //Verify if the player had shot and the wait time has finished
        if(!_canShoot && Time.time -_totalTimeWaited > _waitTimeToshoot)
        {
            _canShoot = true;
            _totalTimeWaited = Time.time;
        }
        //Verify if the player can shoot
        if(Input.GetKeyDown(KeyCode.Space) && _canShoot)
        {
            _audioController.PlayClip(_audioController._playerShot);
            _bulletInstatiatePosition = new Vector3(transform.position.x, transform.position.y + _yBulletInstancePosition, 0);
            Instantiate(_bullet, _bulletInstatiatePosition, Quaternion.identity);
            _canShoot = false;
        }
    }

    public void UpdateScore(int _score)
    {
        _totalScore += _score;
        _hudController.UpdateScore(_totalScore);
        _hudController.UpdateHighScore(_totalScore);
    }

    void OnCollisionEnter2D(Collision2D _other)
    {
        if(_other.gameObject.tag == "Enemy")
        {
            UpdateLivesWhenHit();
        }
    }

    public void UpdateLivesWhenHit()
    {
        _audioController.PlayClip(_audioController._playerDeath);
        if (_lives > 0)
        {
            _lives--;
            _hudController.UpdateLives(_lives);
            _gameController.Deactivate();
        }
        else
        {
            _lives--;
            _hudController.GameOver();
            _gameController.Deactivate();
        }
    }
}
