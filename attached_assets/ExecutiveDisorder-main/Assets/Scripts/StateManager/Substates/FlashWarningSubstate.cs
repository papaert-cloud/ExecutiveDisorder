using UnityEngine;

public class FlashWarningSubstate : SubStateController
{
    [SerializeField] private GameObject m_FlashWarningHolder;
    protected override void OnEnter()
    {
        base.OnEnter();
        m_FlashWarningHolder.SetActive(true);
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_FlashWarningHolder.SetActive(false);
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }

    public override GameObject GetSubstateUI()
    {
        return m_FlashWarningHolder;
    }
}
