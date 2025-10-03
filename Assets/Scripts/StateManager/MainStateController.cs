using System;
using UnityEngine;

public abstract class MainStateController : MonoBehaviour
{
    public AppState AppState;
    // Events
    public event Action OnEntered;
    public event Action OnExited;
    public event Action OnDayStateSwitch;

    [SerializeField]
    protected GameObject[] mainScreens;

    [SerializeField]
    protected bool m_registerOnStart = true;

    //public event Action OnUpdated;
    //public event Action OnFixedUpdated;

    private void Start()
    {
        if(m_registerOnStart) RegisterState();
    }

    public virtual void RegisterState()
    {
        StateManager.Instance.RegisterMainState(AppState, this);
    }

    // Called when this sub-state is entered
    public virtual void Enter()
    {
        Debug.Log($"[Appstate] {AppState} Entered");
        OnEnter();
        OnEntered?.Invoke();
    }

    // Called when this sub-state is exited
    public virtual void Exit()
    {
        Debug.Log($"[Appstate] {AppState} Exited");
        OnExit();
        OnExited?.Invoke();
    }

    public virtual void Init()
    {
        SwitchScreenState(false);
    }

    public virtual void SwitchDayState(DayState dayState)
    {
        Debug.Log($"[Appstate] {AppState} SwitchDayState to {dayState}");
        OnSwitchDayState();
        OnDayStateSwitch?.Invoke();
    }

    //// Called every frame while this sub-state is active
    //public virtual void Tick()
    //{
    //    OnUpdate();
    //    OnUpdated?.Invoke();
    //}

    //// Called every physics update while this sub-state is active
    //public virtual void FixedTick()
    //{
    //    OnFixedUpdate();
    //    OnFixedUpdated?.Invoke();
    //}

    // Override these methods in your child classes for custom logic
    protected virtual void OnEnter()
    {
        SwitchScreenState(true);
    }
    protected virtual void OnExit()
    {
        SwitchScreenState(false);
    }

    protected void SwitchScreenState(bool isActive)
    {
        foreach (GameObject mainScreen in mainScreens)
        {
            mainScreen.SetActive(isActive);
        }
    }


    protected virtual void OnSwitchDayState()
    {
    }

    protected void SwitchSubStateState(SubAppState substate)
    {
        if (StateManager.Instance.ActiveSubStates.Contains(substate))
        {
            StateManager.Instance.ExitSubState(substate);
        }
        else
        {
            StateManager.Instance.EnterSubState(substate);
        }
    }

    //protected virtual void OnUpdate() { }
    //protected virtual void OnFixedUpdate() { }
}
