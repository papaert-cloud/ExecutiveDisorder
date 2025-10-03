using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }

    [SerializeField]
    private AppState _activeState = AppState.None;
    [SerializeField]
    private List<SubAppState> _activeSubStates = new List<SubAppState>();
    [SerializeField]
    private DayState _currentDayState = DayState.Morning;

    private Dictionary<AppState, MainStateController> _mainStateControllers = new Dictionary<AppState, MainStateController>();
    private Dictionary<SubAppState, SubStateController> _subStateControllers = new Dictionary<SubAppState, SubStateController>();

    [SerializeField]
    private List<AppStateSubStatePair> _permittedSubStates = new List<AppStateSubStatePair>();
    [SerializeField]
    private List<AppStateDayStateRequirement> _dayStateRequirements = new List<AppStateDayStateRequirement>();
    [SerializeField]
    private List<ElusiveSubStateRule> _elusiveSubStateRules = new List<ElusiveSubStateRule>();


    public AppState ActiveState => _activeState;
    public List<SubAppState> ActiveSubStates => _activeSubStates;
    public DayState CurrentDayState => _currentDayState;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        //TODO: init maybe change to another class
        StartCoroutine(TestInit());
    }

    IEnumerator TestInit()
    {
        yield return new WaitForSeconds(1);
        SwitchState(AppState.GameStart);
        SetDayState(DayState.Morning);
        AudioManager.Instance.PlayAmbience(AudioManager.SoundType.RelaxingPark);
    }

    public void SetDayState(DayState newDayState)
    {
        _currentDayState = newDayState;
        Debug.Log($"DayState changed to {_currentDayState}");

        //Update CurrentMainState
        GetCurrentMainStateController().SwitchDayState(newDayState);
        //TODO: Notify Substates too?
    }

    public void RegisterMainState(AppState state, MainStateController controller)
    {
        if (!_mainStateControllers.ContainsKey(state))
        {
            _mainStateControllers.Add(state, controller);
            controller.Init();
        }
    }

    public void RegisterSubState(SubAppState subState, SubStateController controller)
    {
        if (!_subStateControllers.ContainsKey(subState))
        {
            _subStateControllers.Add(subState, controller);
        }
    }

    public void SwitchState(AppState newState)
    {
        Debug.Log($"Switching from {_activeState} to {newState}");

        if (!IsDayStateAllowed(newState))
        {
            Debug.LogWarning($"[StateManager] Cannot switch to {newState} during {_currentDayState}.");
            return;
        }

        // Exit previous main state
        foreach (var mainController in _mainStateControllers.Values)
        {
            if (mainController.AppState == _activeState)
                mainController.Exit();
        }

        // Exit all active sub-states
        foreach (var subState in _activeSubStates.ToList())
        {
            ExitSubState(subState);
        }

        _activeSubStates.Clear();
        _activeState = newState;

        if (_activeState == AppState.None)
        {
            Debug.Log("No active main state. All UI disabled.");
            return;
        }

        if (_mainStateControllers.TryGetValue(_activeState, out var controller))
        {
            controller.Enter();
        }

        Debug.Log($"New active state: {_activeState}");
    }

    public void EnterSubState(SubAppState subState)
    {
        var allowed = FindAllowedSubStates(_activeState);
        if (allowed == null)
        {
            Debug.LogWarning($"[StateManager] State {_activeState} not registered for sub-state transitions.");
            return;
        }

        if (!allowed.subStates.Contains(subState))
        {
            Debug.LogWarning($"[StateManager] Sub-state {subState} is not permitted in {_activeState}.");
            return;
        }

        if (!_subStateControllers.TryGetValue(subState, out var controller))
        {
            Debug.LogWarning($"[StateManager] No controller found for sub-state {subState}.");
            return;
        }

        var rule = _elusiveSubStateRules.FirstOrDefault(r => r.triggerSubState == subState);
        if (rule != null)
        {
            foreach (var subToExit in rule.subStatesToExit)
            {
                if (_activeSubStates.Contains(subToExit))
                {
                    ExitSubState(subToExit);
                }
            }
        }

        if (!_activeSubStates.Contains(subState))
        {
            _activeSubStates.Add(subState);
            controller.Enter();
        }
    }

    public void ExitSubState(SubAppState subState)
    {
        if (!_activeSubStates.Contains(subState))
        {
            Debug.LogWarning($"[StateManager] Attempted to exit sub-state {subState}, but it is not active.");
            return;
        }

        if (_subStateControllers.TryGetValue(subState, out var controller) && controller != null)
        {
            controller.Exit();
        }

        _activeSubStates.Remove(subState);
    }

    /// <summary>
    /// Returns the UI GameObject for the most recently entered active sub-state,
    /// or null if there are no active sub-states or the controller is missing.
    /// </summary>
    public GameObject GetCurrentSubstateUI()
    {
        if (_activeSubStates == null || _activeSubStates.Count == 0)
        {
            Debug.LogWarning("[StateManager] GetCurrentSubstateUI: No active sub-states.");
            return null;
        }

        // Most recently added = last in list
        var top = _activeSubStates[_activeSubStates.Count - 1];

        if (_subStateControllers.TryGetValue(top, out var controller) && controller != null)
        {
            // If your SubStateController exposes a specific UI root, replace .gameObject with that:
            // e.g., return controller.GetUIRoot();
            return controller.GetSubstateUI();
        }

        Debug.LogWarning($"[StateManager] GetCurrentSubstateUI: Controller for {top} not found or null.");
        return null;
    }

    /// <summary>
    /// Returns the UI GameObject for a specific sub-state if it is registered (active or not),
    /// or null if the controller is missing.
    /// </summary>
    public GameObject GetSubstateUI(SubAppState subState)
    {
        if (_subStateControllers.TryGetValue(subState, out var controller) && controller != null)
        {
            // Swap for controller.GetUIRoot() if your controller exposes a root UI object.
            return controller.GetSubstateUI();
        }
        Debug.LogWarning($"[StateManager] GetSubstateUI: Controller for {subState} not found or null.");
        return null;
    }

    private AppStateSubStatePair FindAllowedSubStates(AppState state)
    {
        return _permittedSubStates.FirstOrDefault(pair => pair.appState == state);
    }

    private bool IsDayStateAllowed(AppState state)
    {
        var requirement = _dayStateRequirements.FirstOrDefault(req => req.appState == state);
        if (requirement == null || requirement.allowedDayStates == null || requirement.allowedDayStates.Count == 0)
        {
            // No requirement means allowed anytime
            return true;
        }

        return requirement.allowedDayStates.Contains(_currentDayState);
    }

    public AppState GetActiveState()
    {
        Debug.Log($"Current active state: {_activeState}");
        return _activeState;
    }

    public MainStateController GetCurrentMainStateController()
    {
        if (_mainStateControllers.TryGetValue(_activeState, out var controller))
        {
            return controller;
        }

        Debug.LogWarning($"[StateManager] No MainStateController found for active state {_activeState}");
        return null;
    }
}

[Serializable]
public enum AppState
{
    None,
    MainMenu,
    FakeNews,
    Characters,
    Profile,
    Ending,
    GameStart
}

[Serializable]
public enum SubAppState
{
    // Define your substates here
    MenuActions,
    CardDecision,
    CrisisDecision,
    ResourceBar,
    OptionsBar,
    FlashWarning,
}

[Serializable]
public enum DayState
{
    Morning,
    Afternoon,
    Night
}

[Serializable]
public class AppStateSubStatePair
{
    public AppState appState;
    public List<SubAppState> subStates;
}

[Serializable]
public class AppStateDayStateRequirement
{
    public AppState appState;
    public List<DayState> allowedDayStates;
}

[Serializable]
public class ElusiveSubStateRule
{
    public SubAppState triggerSubState;            // The one being entered
    public List<SubAppState> subStatesToExit;      // Ones to auto-exit when trigger is entered
}

