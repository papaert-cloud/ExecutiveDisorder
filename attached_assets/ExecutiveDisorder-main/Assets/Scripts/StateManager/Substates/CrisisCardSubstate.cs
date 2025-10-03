using UnityEngine;

public class CrisisCardSubstate : SubStateController
{
    [SerializeField] private GameObject m_crisisCardHolder;
    protected override void OnEnter()
    {
        base.OnEnter();
        m_crisisCardHolder.SetActive(true);
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_crisisCardHolder.SetActive(false);
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }

    public override GameObject GetSubstateUI()
    {
        return m_crisisCardHolder;
    }
}
