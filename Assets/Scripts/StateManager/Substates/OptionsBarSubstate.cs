using UnityEngine;

public class OptionsBarSubstate : SubStateController
{
    [SerializeField] GameObject m_optionBarHolder;

    protected override void OnEnter()
    {
        base.OnEnter();
        m_optionBarHolder.SetActive(true);
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_optionBarHolder.SetActive(false);
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }

    public override GameObject GetSubstateUI()
    {
        return m_optionBarHolder;
    }
}
