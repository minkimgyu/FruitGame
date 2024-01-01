using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _endPanel;
    [SerializeField] GameObject _clear;
    [SerializeField] GameObject _over;

    bool _gameClear = false;
    bool _gameOver = false;
    bool _gamePause = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trickle") == false) return;

        GameOver();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM("BGM2", true);
        SoundManager.Instance.SetBGMVolume(0.3f);
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsGameClear()
    {
        return _gameClear;
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }

    public bool IsPauseGame()
    {
        return _gamePause;
    }

    public void GameClear()
    {
        if (_gameClear == true) return;

        _clear.SetActive(true);
        _endPanel.SetActive(true);
        _gameClear = true;
    }

    public void GameOver()
    {
        if (_gameOver == true) return;

        _over.SetActive(true);
        _endPanel.SetActive(true);
        _gameOver = true;
    }

    public void PauseGame()
    {
        if (_gamePause == true) return;
        _gamePause = true;
    }

    public void ContinueGame()
    {
        if (_gamePause == false) return;
        _gamePause = false;
    }
}
