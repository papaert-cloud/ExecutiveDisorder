using UnityEngine;

public class MainMenuStateController : MainStateController
{

    protected override void OnEnter()
    {
        base.OnEnter();
        StateManager.Instance.EnterSubState(SubAppState.MenuActions);
        StateManager.Instance.EnterSubState(SubAppState.ResourceBar);
        StateManager.Instance.EnterSubState(SubAppState.OptionsBar);
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }
}
