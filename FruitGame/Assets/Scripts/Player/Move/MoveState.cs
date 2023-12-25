using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class MoveState : State
{
    PlayerController _storedPlayer;

    public MoveState(PlayerController player)
    {
        _storedPlayer = player;
    }

    public override void OnStateUpdate()
    {
        if (_storedPlayer.IsGameOver() == true) return;

        MoveCloud();
    }

    void MoveCloud()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
               Input.mousePosition.y, -Camera.main.transform.position.z));

        if (point.x > _storedPlayer.MaxX)
        {
            point.x = _storedPlayer.MaxX;
        }
        else if (point.x < _storedPlayer.MinX)
        {
            point.x = _storedPlayer.MinX;
        }

        _storedPlayer.Cloud.position 
            = new Vector3(point.x, _storedPlayer.Cloud.position.y, _storedPlayer.Cloud.position.z);
    }

    public override void CheckStateChange()
    {
        if (Input.GetAxisRaw("Mouse X") == 0) // 마우스 움직임 정지 시
        {
            _storedPlayer.MovementFSM.SetState(PlayerController.MovementState.Stop);
        }
    }
}
