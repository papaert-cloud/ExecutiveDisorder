using System.Collections.Generic;
using UnityEngine;
using System; // Required for Enum.GetValues

public enum ResourceType
{
    Popularity,
    Stability,
    Media,
    Economic
}

public class ResourcesManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static ResourcesManager Instance { get; private set; }

    // --- Inspector Fields ---
    [Header("Initial Resource Values")]
    [Tooltip("Starting value for Popularity")]
    [SerializeField] private float initialPopularity = 50f; // Example starting value
    [Tooltip("Starting value for Stability")]
    [SerializeField] private float initialStability = 50f; // Example starting value
    [Tooltip("Starting value for Media Control")]
    [SerializeField] private float initialMedia = 50f; // Example starting value
    [Tooltip("Starting value for Economic Strength")]
    [SerializeField] private float initialEconomic = 50f; // Example starting value

    [Header("Current Resource Values (Read-Only)")]
    [SerializeField] private float currentPopularity; // Field to display current value in Inspector
    [SerializeField] private float currentStability;  // Field to display current value in Inspector
    [SerializeField] private float currentMedia;      // Field to display current value in Inspector
    [SerializeField] private float currentEconomic;   // Field to display current value in Inspector

    // --- Internal Data ---
    // The actual dictionary holding the resource values during runtime
    private Dictionary<ResourceType, float> m_resources;

    public Dictionary<ResourceType, float>  Resources => m_resources;

    //Resources to add when a decision is clicked, this is for waiting the hide animation
    private Dictionary<ResourceType, float> m_resourcesToAdd;

    public Action<ResourceType, float> OnResourceChanged;
    public Action<Dictionary<ResourceType, float>, bool> ResourcePreviewRequest;
    public Action StopResourcePreviewRequest;
    public Action StopPreviewOnDecisionRequest;

    // --- Unity Methods ---
    private void Awake()
    {
        // Singleton Initialization
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate ResourcesManager detected. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        InitializeResources();

        UpdateInspectorValues();
    }

    private void Start()
    {
        DecisionsManager.Instance.OnDecisionSelected += OnDecisionSelected;
    }

    private void Update()
    {
        UpdateInspectorValues();
    }

    // --- Public Methods ---

    /// <summary>
    /// Gets the current amount of a specific resource.
    /// </summary>
    /// <param name="type">The type of resource to query.</param>
    /// <returns>The current amount of the resource, or 0f if the type is invalid.</returns>
    public float GetResource(ResourceType type)
    {
        if (m_resources != null && m_resources.TryGetValue(type, out float value))
        {
            return value;
        }
        Debug.LogWarning($"Resource type {type} not found in dictionary.");
        return 0f; // Return a default or handle error appropriately
    }

    /// <summary>
    /// Adds a specified amount to a resource. Can be negative to subtract.
    /// </summary>
    /// <param name="type">The type of resource to modify.</param>
    /// <param name="amount">The amount to add (can be negative).</param>
    public void AddResource(ResourceType type, float amount)
    {
        if (m_resources != null && m_resources.ContainsKey(type))
        {
            m_resources[type] += amount;
            m_resources[type] = Mathf.Clamp(m_resources[type], 0f, 100f);

            OnResourceChanged?.Invoke(type, m_resources[type]);

            Debug.Log($"Added {amount} to {type}. New value: {m_resources[type]}");
        }
        else
        {
            Debug.LogWarning($"Cannot add resource: Type {type} not found.");
        }
    }

    /// <summary>
    /// Adds multiple resource values at once. Each entry modifies its corresponding resource.
    /// </summary>
    /// <param name="resourcesValuePair">A dictionary of resource types and values to add (can be negative).</param>
    public void AddResources(Dictionary<ResourceType, float> resourcesValuePair)
    {
        foreach (var pair in resourcesValuePair)
        {
            if (pair.Value == 0f) continue;
            AddResource(pair.Key, pair.Value);
        }
    }

    /// <summary>
    /// Sets a resource to a specific value.
    /// </summary>
    /// <param name="type">The type of resource to set.</param>
    /// <param name="value">The new value for the resource.</param>
    public void SetResource(ResourceType type, float value)
    {
        if (m_resources != null && m_resources.ContainsKey(type))
        {
            m_resources[type] = value;
            // Optional: Clamp resource values if needed
            // m_resources[type] = Mathf.Clamp(m_resources[type], 0f, 100f);
            Debug.Log($"Set {type} to {value}.");
            // No need to call UpdateInspectorValues() here because Update() does it every frame.
        }
        else
        {
            Debug.LogWarning($"Cannot set resource: Type {type} not found.");
        }
    }

    public void ResourcesHideCompleted()
    {
        AddResources(m_resourcesToAdd);
    }

    public void OnDecisionSelected(DecisionOption selectedOption)
    {
        m_resourcesToAdd = selectedOption.GetResourcesAffected();
        StopPreviewOnDecisionRequest?.Invoke();
    }

    /// <summary>
    /// Resource preview request for the ui and other things
    /// </summary>
    public void PreviewResources(Dictionary<ResourceType, float> resourceCosts, bool showValue)
    {
        ResourcePreviewRequest?.Invoke(resourceCosts, showValue);
    }

    /// <summary>
    /// Stop Resource preview for the ui and other things
    /// </summary>
    public void StopResourcePreview()
    {
        StopResourcePreviewRequest?.Invoke();
    }

    // --- Private Helper Methods ---

    /// <summary>
    /// Initializes the internal resource dictionary using the values set in the Inspector.
    /// </summary>
    private void InitializeResources()
    {
        m_resources = new Dictionary<ResourceType, float>
        {
            { ResourceType.Popularity, initialPopularity },
            { ResourceType.Stability, initialStability },
            { ResourceType.Media, initialMedia },
            { ResourceType.Economic, initialEconomic }
        };
    }


    /// <summary>
    /// Updates the Inspector fields to show the current values from the dictionary.
    /// Called by Update() to keep the Inspector view synchronized during runtime.
    /// </summary>
    private void UpdateInspectorValues()
    {
        // Safety check in case this runs before Awake finishes (unlikely but safe)
        if (m_resources == null) return;

        // Use TryGetValue for safety, though keys should always exist after InitializeResources
        m_resources.TryGetValue(ResourceType.Popularity, out currentPopularity);
        m_resources.TryGetValue(ResourceType.Stability, out currentStability);
        m_resources.TryGetValue(ResourceType.Media, out currentMedia);
        m_resources.TryGetValue(ResourceType.Economic, out currentEconomic);
    }
}