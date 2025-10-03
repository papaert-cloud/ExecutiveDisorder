using UnityEngine;

public class CardDecsionSubstate : SubStateController
{
    [SerializeField] GameObject m_decisionCardHolder;

    protected override void OnEnter()
    {
        base.OnEnter();
        m_decisionCardHolder.SetActive(true);
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_decisionCardHolder.SetActive(false);
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }

    public override GameObject GetSubstateUI()
    {
        return m_decisionCardHolder;
    }
}
