using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;

    [SerializeField] private List<EndingSO> endings = new List<EndingSO>();

    public Action<EndingSO> OnEnding;

    private bool IsEnding = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate EndingManager detected. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResourcesManager.Instance.OnResourceChanged += OnResourceChange;
    }

    private void OnResourceChange(ResourceType resourceType, float amount)
    {
        CheckEndings();
    }

    private void CheckEndings()
    {
        Dictionary<ResourceType, float> resources = ResourcesManager.Instance.Resources;

        foreach (EndingSO ending in endings)
        {
            bool allRequirementsMet = true;

            foreach (EndingResourceRequirement req in ending.ResourceRequirements)
            {
                if (!Compare(resources[req.ResourceType], req.Comparison, req.Value))
                {
                    allRequirementsMet = false;
                    break;
                }
            }

            if (allRequirementsMet && !IsEnding)
            {
                IsEnding = true;
                TriggerEnding(ending);
                break; 
            }
        }
    }

    private bool Compare(float current, ComparisonType comparison, float target)
    {
        switch (comparison)
        {
            case ComparisonType.LessThan:
                return current < target;
            case ComparisonType.Equal:
                return Mathf.Approximately(current, target);
            case ComparisonType.NotEqual:
                return !Mathf.Approximately(current, target);
            case ComparisonType.GreaterThan:
                return current > target;
            default:
                return false;
        }
    }

    private void TriggerEnding(EndingSO ending)
    {
        StartCoroutine(StartEnd(ending));
        OnEnding?.Invoke(ending);
    }

    IEnumerator StartEnd(EndingSO ending) 
    {
        yield return null;
        StateManager.Instance.SwitchState(AppState.Ending);
    }
}