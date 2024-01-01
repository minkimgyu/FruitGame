using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    abstract public class State : BaseState
    {
        public override void CheckStateChange() { }

        public override void OnMessageReceived() { }

        public override void OnMessageReceived(Trickle.Message message, Collision2D collision) { }
        // 여기에 메시지도 같이 전달 ㄱㄱㄱ

        public override void OnStateFixedUpdate() { }

        public override void OnStateLateUpdate() { }

        public override void OnStateCollision2DEnter(Collision2D collision) { }


        public override void OnStateEnter() { }

        public override void OnStateUpdate() { }

        public override void OnStateExit() { }
    }

    abstract public class BaseState
    {
        public abstract void CheckStateChange();

        public abstract void OnMessageReceived();

        public abstract void OnMessageReceived(Trickle.Message message, Collision2D collision); 
        // 여기에 메시지도 같이 전달 ㄱㄱㄱ

        public abstract void OnStateFixedUpdate();

        public abstract void OnStateLateUpdate();

        public abstract void OnStateCollision2DEnter(Collision2D collision);

        public abstract void OnSpawnRequested(); // 오브젝트가


        public abstract void OnStateEnter();

        public abstract void OnStateUpdate();

        public abstract void OnStateExit();
    }

    public class StateMachine<T>
    {
        Dictionary<T, BaseState> _stateDictionary = new Dictionary<T, BaseState>();

        T _currentStateName;

        public T CurrentStateName { get { return _currentStateName; } }

        //현재 상태를 담는 프로퍼티.
        BaseState _currentState;
        BaseState _previousState;

        public void Initialize(Dictionary<T, BaseState> stateDictionary)
        {
            _currentState = null;
            _previousState = null;

            _stateDictionary = stateDictionary;
        }

        public void OnCollision2DEnter(Collision2D collision)
        {
            if (_currentState == null) return;
            _currentState.OnStateCollision2DEnter(collision);
        }
       

        public void OnUpdate()
        {
            if (_currentState == null) return;
            _currentState.OnStateUpdate();
            _currentState.CheckStateChange();
        }

        public void OnFixedUpdate()
        {
            if (_currentState == null) return;
            _currentState.OnStateFixedUpdate();
        }

        public void OnLateUpdate()
        {
            if (_currentState == null) return;
            _currentState.OnStateLateUpdate();
        }

        public bool RevertToPreviousState()
        {
            return ChangeState(_previousState);
        }

        #region SetState

        public bool SetState(T stateName)
        {
            _currentStateName = stateName;
            return ChangeState(_stateDictionary[stateName]);
        }

        public bool SetState(T stateName, Trickle.Message message, Collision2D collision)
        {
            _currentStateName = stateName;
            return ChangeState(_stateDictionary[stateName], message, collision);
        }

        #endregion


        #region ChangeState

        bool ChangeState(BaseState state)
        {
            if (_stateDictionary.ContainsValue(state) == false) return false;

            if (_currentState == state) // 같은 State로 전환하지 못하게 막기
            {
                return false;
            }

            if (_currentState != null) //상태가 바뀌기 전에, 이전 상태의 Exit를 호출
                _currentState.OnStateExit();

            _previousState = _currentState;

            _currentState = state;


            if (_currentState != null) //새 상태의 Enter를 호출한다.
            {
                _currentState.OnStateEnter();
            }

            return true;
        }

        bool ChangeState(BaseState state, Trickle.Message message, Collision2D collision)
        {
            if (_stateDictionary.ContainsValue(state) == false) return false;

            if (_currentState == state) // 같은 State로 전환하지 못하게 막기
            {
                return false;
            }

            if (_currentState != null) //상태가 바뀌기 전에, 이전 상태의 Exit를 호출
                _currentState.OnStateExit();

            _previousState = _currentState;

            _currentState = state;


            if (_currentState != null) //새 상태의 Enter를 호출한다.
            {
                _currentState.OnMessageReceived(message, collision);
                _currentState.OnStateEnter();
            }

            return true;
        }

        #endregion
    }
}