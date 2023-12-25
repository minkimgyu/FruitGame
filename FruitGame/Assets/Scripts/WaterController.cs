using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterController : MonoBehaviour
{
    Transform _water;

    float _yPos;

    [SerializeField] float _offset = 2f;
    [SerializeField] float _upSpeed = 5f;

    [SerializeField] float _decreaseAmound = 2f;

    [SerializeField] Transform _maxYPoint;
    [SerializeField] Transform _minYPoint;

    // merge �� �̺�Ʈ ������ �� ����
    // ���� ��踦 ������ ������ ������ ������ֱ�

    // �Ϲ����� ���¿����� �� ��� ä��

    public Action OnGameOverRequested;

    // Start is called before the first frame update
    void Start()
    {
        _water = transform;
        _yPos = _minYPoint.position.y;

        GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null) return;

        OnGameOverRequested = gameManager.GameOver;
    }

    public void OnWaterDecreaseRequested()
    {
        _yPos -= _decreaseAmound;
        if (_yPos < _minYPoint.position.y) _yPos = _minYPoint.position.y;
    }

    void UpdateYPos()
    {
        _yPos += _offset * Time.deltaTime;
        if (_yPos > _maxYPoint.transform.position.y)
        {
            _yPos = _maxYPoint.transform.position.y;
            OnGameOverRequested?.Invoke();
            // ��ĥ ��� ���� ����
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateYPos();

        float yPos = Mathf.Lerp(_water.transform.position.y, _yPos, _upSpeed * Time.deltaTime);
        _water.transform.position = new Vector3(_water.transform.position.x, yPos, 0);
    }
}
