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

    bool GoToStopState()
    {
        bool isGameClear = _storedPlayer.IsGameClear.Invoke();
        bool IsGamePause = _storedPlayer.IsGamePause.Invoke();
        bool IsGameOver = _storedPlayer.IsGameOver.Invoke();
        bool nowMouseStop = Input.GetAxisRaw("Mouse X") == 0;

        return IsGameOver || isGameClear || IsGamePause || nowMouseStop;
    }

    public override void CheckStateChange()
    {
        // 일시 정지, 게임 오버, 게임 클리어시 강제로 Stop State로 이동
        if (GoToStopState()) // 추가로 마우스 움직임 정지 시
        {
            _storedPlayer.MovementFSM.SetState(PlayerController.MovementState.Stop);
        }
    }
}
