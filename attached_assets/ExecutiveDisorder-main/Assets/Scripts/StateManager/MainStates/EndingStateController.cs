using UnityEngine;

public class EndingStateController : MainStateController
{
    [SerializeField] private GameObject m_endingUI;
    protected override void OnEnter()
    {
        base.OnEnter();
        m_endingUI.GetComponent<CanvasGroup>().alpha = 1;
        m_endingUI.GetComponent<CanvasGroup>().interactable = true;
        m_endingUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_endingUI.GetComponent<CanvasGroup>().alpha = 0;
        m_endingUI.GetComponent<CanvasGroup>().interactable = false;
        m_endingUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }
}
