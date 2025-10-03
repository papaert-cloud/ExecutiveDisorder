using UnityEngine;

public class GameStartStateController : MainStateController
{
    private WarningPulse _flashPulse; // cache so we don't repeatedly look it up

    protected override void OnEnter()
    {
        base.OnEnter();

        StateManager.Instance.EnterSubState(SubAppState.FlashWarning);

        var flashUI = StateManager.Instance.GetSubstateUI(SubAppState.FlashWarning);
        if (flashUI != null)
        {
            _flashPulse = flashUI.GetComponent<WarningPulse>();
            if (_flashPulse != null)
            {
                _flashPulse.OnAnimFinish += OnAnimFinish;
            }
            else
            {
                Debug.LogWarning("FlashWarning UI exists, but no WarningPulse component found.");
            }
        }
        else
        {
            Debug.LogWarning("FlashWarning UI not found.");
        }

        StateManager.Instance.EnterSubState(SubAppState.OptionsBar);
    }

    protected override void OnExit()
    {
        base.OnExit();

        if (_flashPulse != null)
        {
            _flashPulse.OnAnimFinish -= OnAnimFinish;
            _flashPulse = null;
        }
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }

    private void OnAnimFinish()
    {
        StateManager.Instance.SwitchState(AppState.MainMenu);
        DecisionsManager.Instance.StartDecisions();
    }
}
