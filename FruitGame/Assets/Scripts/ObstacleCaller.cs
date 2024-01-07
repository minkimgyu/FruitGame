using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCaller : MonoBehaviour
{
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _endPoint;

    [SerializeField] Transform _minSpawnPoint;
    [SerializeField] Transform _maxSpawnPoint;

    [SerializeField] Transform _bird;

    [SerializeField] GameObject _stonePrefab;

    float xPos;
    bool nowSpawn = false;
    bool nowMove = false;

    public void OnBirdCallRequested()
    {
        nowMove = true;
        nowSpawn = true;
        _bird.transform.position = _startPoint.position;
        xPos = Random.Range(_minSpawnPoint.position.x, _maxSpawnPoint.position.x);
    }

    private void Update()
    {
        if (nowMove == false) return;

        _bird.position = Vector3.MoveTowards(_bird.position, _endPoint.position, Time.deltaTime * 3f);

        if (Mathf.Abs(_bird.position.x - _endPoint.position.x) <= 0.1f)
        {
            // 이동 루틴
            _bird.transform.position = _startPoint.position;
            nowMove = false;
        }


        if (nowSpawn == false) return;

        if (Mathf.Abs(_bird.position.x - xPos) <= 0.3f)
        {
            Instantiate(_stonePrefab, _bird.position, Quaternion.identity);
            nowSpawn = false;
        }
    }
}
