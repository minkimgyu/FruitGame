using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform _endPoint;

    Action<Vector3> OnDropRequested;
    Action OnSpawnRequested;
    Func<float, bool> IsFruitYPosAboveLine;

    Timer _dropTimer;
    bool _canDrop;

    [SerializeField] float _minX = -14;
    [SerializeField] float _maxX = 14.5f;

    [SerializeField] float _delayTime;
    [SerializeField] Transform _cloud;

    [SerializeField] GameObject _endPanel;
    [SerializeField] GameObject _clear;
    [SerializeField] GameObject _over;

    private void Start()
    {
        Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        if (spawner == null) return;

        OnDropRequested = spawner.DropNextFruit;
        OnSpawnRequested = spawner.SpawnNextDropFruit;
        IsFruitYPosAboveLine = spawner.IsFruitYPosAboveLine;

        _canDrop = true;
        _dropTimer = new Timer();

        OnSpawnRequested?.Invoke();
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void CheckGameOver()
    {
        bool canGameOver = IsFruitYPosAboveLine(_endPoint.position.y);
        if (canGameOver == true)
        {
            _over.SetActive(true);
            _endPanel.SetActive(true);
        }
    }

    void MoveCloud()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
               Input.mousePosition.y, -Camera.main.transform.position.z));

        if (point.x > _maxX)
        {
            point.x = _maxX;
        }
        else if(point.x < _minX)
        {
            point.x = _minX;
        }

        _cloud.position = new Vector3(point.x, _cloud.position.y, _cloud.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCloud();

        _dropTimer.Update();

        if (_dropTimer.IsFinish)
        {
            CheckGameOver();  // 체크하고 스폰

            OnSpawnRequested?.Invoke();
            _canDrop = true;
            _dropTimer.Reset();
        }

        if (_canDrop == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

            if (point.x > _maxX)
            {
                point.x = _maxX;
            }
            else if (point.x < _minX)
            {
                point.x = _minX;
            }

            OnDropRequested?.Invoke(new Vector3(point.x, _endPoint.position.y, point.z));
            _dropTimer.Start(_delayTime);
            _canDrop = false;
        }
    }
}
