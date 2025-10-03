using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_EndingTitleText;
    [SerializeField] private TextMeshProUGUI m_EndingDescriptionText;
    [SerializeField] private Transform m_EndingResourcesHolder;
    [SerializeField] private GameObject m_EndingResourcePrefab;

    private void Start()
    {
        EndingManager.Instance.OnEnding += SetEnding;
    }

    public void SetEnding(EndingSO ending)
    {
        m_EndingTitleText.text = ending.Title;
        m_EndingDescriptionText.text = ending.Description;
        SetEndingResources(ending.ResourceRequirements);
    }

    private void SetEndingResources(List<EndingResourceRequirement> resourceRequirements)
    {
        foreach (EndingResourceRequirement resourceRequirement in resourceRequirements)
        {
            GameObject endingResourceObject = Instantiate(m_EndingResourcePrefab, m_EndingResourcesHolder);
            EndingResourceUI endingResourceUI = endingResourceObject.GetComponent<EndingResourceUI>();
            endingResourceUI.SetEndingResource(resourceRequirement.ResourceType, resourceRequirement.Comparison, resourceRequirement.Value);
        }
    }
}
