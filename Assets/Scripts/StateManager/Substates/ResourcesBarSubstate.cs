using UnityEngine;

public class ResourcesBarSubstate : SubStateController
{
    [SerializeField] private GameObject m_resourcesBarHolder;
    protected override void OnEnter()
    {
        base.OnEnter();
        m_resourcesBarHolder.SetActive(true);
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_resourcesBarHolder.SetActive(false);
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }

    public override GameObject GetSubstateUI()
    {
        return m_resourcesBarHolder;
    }
}
