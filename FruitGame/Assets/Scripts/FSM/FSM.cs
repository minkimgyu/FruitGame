using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    abstract public class BaseState
    {
        public abstract void CheckStateChange();

        public abstract void OnMessageReceived(Fruit.Message message);

        public abstract void OnMessageReceived(Fruit.Message message, Collision2D collision);

        public abstract void OnMessageReceived(Fruit.Message message, Fruit fruit, Vector3 contactPos);


        public abstract void OnCollision2DEnterRequested(Collision2D collision);

        public abstract void OnCollision2DExitRequested(Collision2D collision);

        public abstract void OnReadyRequested();

        public abstract void OnDropRequested();

        public abstract void OnSpawnRequested();

        public abstract void OnLandRequested();

        public abstract void OnHighlightRequested();


        public abstract void OnStateEnter();

        public abstract void OnStateUpdate();

        public abstract void OnStateExit();
    }

    abstract public class State : BaseState
    {
        public override void CheckStateChange() { }


        public override void OnMessageReceived(Fruit.Message message) { }

        public override void OnMessageReceived(Fruit.Message message, Collision2D collision) { }
        
        public override void OnMessageReceived(Fruit.Message message, Fruit fruit, Vector3 contactPos) { }


        public override void OnCollision2DEnterRequested(Collision2D collision) { }

        public override void OnCollision2DExitRequested(Collision2D collision) { }

        public override void OnReadyRequested() { }

        public override void OnDropRequested() { }

        public override void OnSpawnRequested() { }

        public override void OnLandRequested() { }

        public override void OnHighlightRequested() { }

        public override void OnStateEnter() { }

        public override void OnStateUpdate() { }

        public override void OnStateExit() { }
    }

    public class StateMachine<T>
    {
        protected Dictionary<T, BaseState> _stateDictionary = new Dictionary<T, BaseState>();

        protected T _currentStateName;

        public T CurrentStateName { get { return _currentStateName; } }

        //현재 상태를 담는 프로퍼티.
        protected BaseState _currentState;
        protected BaseState _previousState;

        public void Initialize(Dictionary<T, BaseState> stateDictionary)
        {
            _currentState = null;
            _previousState = null;

            _stateDictionary = stateDictionary;
        }

        public void OnUpdate()
        {
            if (_currentState == null) return;

            _currentState.CheckStateChange();
            _currentState.OnStateUpdate();
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

        #endregion
    }

    public class TrickleStateMachine<T> : StateMachine<T>
    {
        public void OnCollision2DEnter(Collision2D collision)
        {
            if (_currentState == null) return;
            _currentState.OnCollision2DEnterRequested(collision);
        }

        public void OnCollision2DExit(Collision2D collision)
        {
            if (_currentState == null) return;
            _currentState.OnCollision2DExitRequested(collision);
        }

        public void OnReady() 
        {
            if (_currentState == null) return;
            _currentState.OnReadyRequested();
        }

        public void OnDrop() 
        {
            if (_currentState == null) return;
            _currentState.OnDropRequested();
        }

        public void OnSpawn() 
        {
            if (_currentState == null) return;
            _currentState.OnSpawnRequested();
        }

        public void OnLand()
        {
            if (_currentState == null) return;
            _currentState.OnLandRequested();
        }

        public void OnHighlight()
        {
            if (_currentState == null) return;
            _currentState.OnHighlightRequested();
        }


        #region SetState

        public bool SetState(T stateName, Fruit.Message message)
        {
            _currentStateName = stateName;
            return ChangeState(_stateDictionary[stateName], message);
        }

        public bool SetState(T stateName, Fruit.Message message, Collision2D collision)
        {
            _currentStateName = stateName;
            return ChangeState(_stateDictionary[stateName], message, collision);
        }

        public bool SetState(T stateName, Fruit.Message message, Fruit fruit, Vector3 contactPos)
        {
            _currentStateName = stateName;
            return ChangeState(_stateDictionary[stateName], message, fruit, contactPos);
        }

        #endregion


        #region ChangeState

        bool ChangeState(BaseState state, Fruit.Message message)
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
                _currentState.OnMessageReceived(message);
                _currentState.OnStateEnter();
            }

            return true;
        }

        bool ChangeState(BaseState state, Fruit.Message message, Collision2D collision)
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

        bool ChangeState(BaseState state, Fruit.Message message, Fruit fruit, Vector3 contactPos)
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
                _currentState.OnMessageReceived(message, fruit, contactPos);
                _currentState.OnStateEnter();
            }

            return true;
        }

        #endregion
    }
}