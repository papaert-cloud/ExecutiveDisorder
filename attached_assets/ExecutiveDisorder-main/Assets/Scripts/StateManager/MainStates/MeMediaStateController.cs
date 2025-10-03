using UnityEngine;

public class MeMediaStateController : MainStateController
{
    [SerializeField] private GameObject m_MeMediaUI;
    protected override void OnEnter()
    {
        base.OnEnter();
        m_MeMediaUI.GetComponent<CanvasGroup>().alpha = 1;
        m_MeMediaUI.GetComponent<CanvasGroup>().interactable = true;
        m_MeMediaUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_MeMediaUI.GetComponent<CanvasGroup>().alpha = 0;
        m_MeMediaUI.GetComponent<CanvasGroup>().interactable = false;
        m_MeMediaUI.GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }
}
