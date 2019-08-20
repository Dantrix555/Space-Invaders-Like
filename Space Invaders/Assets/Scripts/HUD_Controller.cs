using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD_Controller : MonoBehaviour
{
    public Text _scoreText;
    public Text _highScoreText;
    public Text _livesText;
    public Text _gameOverText;
    public Text _retryText;
    private int _highScore;
    private bool _gameOver;

    void Start()
    {
        _highScore = PlayerPrefs.GetInt("HighScore");
    }

    void Update()
    {
        if(_gameOver)
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                Application.Quit();
            }
        }
    }

    public void UpdateLives(int _lives)
    {
        _livesText.text = "Lives = " + _lives.ToString();
    }

    public void UpdateScore(int _score)
    {
        _scoreText.text = "Score = " + _score.ToString();
    }

    public void UpdateHighScore(int _score)
    {
        //Verify if the high score was recorded
        if(_score > _highScore)
        {
            PlayerPrefs.SetInt("HighScore", _score);
            _highScore = PlayerPrefs.GetInt("HighScore");
        }
        _highScoreText.text = "High Score = " + _highScore;
    }

    public void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _retryText.gameObject.SetActive(true);
        _gameOver = true;
    }
}
