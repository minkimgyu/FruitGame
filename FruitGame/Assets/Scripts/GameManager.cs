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

    bool _gameOver = false;

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }

    public void GameOver()
    {
        if (_gameOver == true) return;

        _over.SetActive(true);
        _endPanel.SetActive(true);
        _gameOver = true;
    }
}
