using UnityEngine;

public class MenuActionsSubstate : SubStateController
{
    [SerializeField] GameObject m_menuActionsHolder;

    protected override void OnEnter()
    {
        base.OnEnter();
        m_menuActionsHolder.SetActive(true);
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_menuActionsHolder.SetActive(false);
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }

    public override GameObject GetSubstateUI()
    {
        return m_menuActionsHolder;
    }
}
