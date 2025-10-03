using System;
using UnityEngine;

public abstract class SubStateController : MonoBehaviour
{
    public SubAppState SubAppState;

    // Events
    public event Action OnEntered;
    public event Action OnExited;
    public event Action OnDayStateSwitch;

    [SerializeField]
    protected bool m_registerOnStart = true;

    private void Start()
    {
        if (m_registerOnStart) RegisterState();
    }

    public virtual void RegisterState()
    {
        StateManager.Instance.RegisterSubState(SubAppState, this);
    }

    // Called when this sub-state is entered
    public virtual void Enter()
    {
        Debug.Log($"{this.GetType().Name} Entered");
        OnEnter();
        OnEntered?.Invoke();
    }

    // Called when this sub-state is exited
    public virtual void Exit()
    {
        Debug.Log($"{this.GetType().Name} Exited");
        OnExit();
        OnExited?.Invoke();
    }

    public virtual void SwitchDayState(DayState dayState)
    {
        Debug.Log($"[Appstate] {SubAppState} SwitchDayState to {dayState}");
        OnSwitchDayState();
        OnDayStateSwitch?.Invoke();
    }

    protected virtual void OnSwitchDayState() { }

    // Override these methods in your child classes for custom logic
    protected virtual void OnEnter() 
    {
        Debug.Log($"[Appstate] Enetered Substate {SubAppState}");
    }
    protected virtual void OnExit() 
    {
        Debug.Log($"[Appstate] Exited Substate {SubAppState}");
    }

    public virtual GameObject GetSubstateUI() { return null; }
}
