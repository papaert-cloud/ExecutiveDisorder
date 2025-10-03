using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// No need for [SerializeField] on enum
public enum VFXType
{
    Confeti,
    RedFlash
}

[Serializable]
public struct EffectStruct
{
    public VFXType key;
    public Effect value;
}

[Serializable]
public struct Effect
{
    public GameObject prefab;
    public bool IsUI;
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [Header("Catalog")]
    [SerializeField] private List<EffectStruct> m_Effects = new List<EffectStruct>();

    [Header("Defaults")]
    [Tooltip("Optional parent for spawned VFX (e.g., a 'VFX_Root' under your scene or Canvas).")]
    [SerializeField] private Transform m_DefaultParent;
    [Tooltip("Default lifetime (seconds) for spawned VFX if not specified.")]
    [SerializeField] private float m_DefaultLifetime = 5f;
    [Tooltip("Optional parent for spawned VFX (e.g., a 'VFX_Root' under your scene or Canvas).")]
    [SerializeField] private Transform m_DefaultUIParent;

    private readonly Dictionary<VFXType, Effect> m_vfxDictionary = new Dictionary<VFXType, Effect>();

    // --- Queue of spawned items scheduled for cleanup ---
    private struct SpawnedVfx
    {
        public GameObject go;
        public float deathTime;
    }
    private readonly Queue<SpawnedVfx> m_cleanupQueue = new Queue<SpawnedVfx>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // Build dictionary with duplicate protection
        for (int i = 0; i < m_Effects.Count; i++)
        {
            var effect = m_Effects[i];
            if (effect.value.prefab == null)
            {
                Debug.LogWarning($"Effect [{i}] with key {effect.key} has a null prefab, skipping.");
                continue;
            }

            if (!m_vfxDictionary.ContainsKey(effect.key))
                m_vfxDictionary.Add(effect.key, effect.value);
            else
                Debug.LogWarning($"Duplicate VFX key '{effect.key}' at index {i}. Skipping.");
        }
    }

    private void Update()
    {
        // Destroy any items whose time has come (FIFO)
        while (m_cleanupQueue.Count > 0 && Time.time >= m_cleanupQueue.Peek().deathTime)
        {
            var item = m_cleanupQueue.Dequeue();
            if (item.go != null) Destroy(item.go);
        }
    }

    // --- Public API ---

    /// <summary>
    /// Quick spawn using defaults. Spawns at (0,0,0) with identity rotation and default parent.
    /// </summary>
    public GameObject PlayVFX(VFXType vfx)
        => PlayVFX(vfx, Vector3.zero, Quaternion.identity, m_DefaultParent, m_DefaultLifetime);

    public GameObject PlayUIVFX(VFXType vfx)
        => PlayVFX(vfx, Vector3.zero, Quaternion.identity, m_DefaultUIParent, m_DefaultLifetime);

    /// <summary>
    /// Spawn with position/rotation and optional parent/lifetime.
    /// </summary>
    public GameObject PlayVFX(
        VFXType vfx,
        Vector3 position,
        Quaternion rotation,
        Transform parent = null,
        float lifetime = -1f)
    {
        if (!m_vfxDictionary.TryGetValue(vfx, out var effect) || effect.prefab == null)
        {
            Debug.LogWarning($"Trying to play VFX '{vfx}' but it's missing or has a null prefab.");
            return null;
        }

        if (lifetime <= 0f) lifetime = m_DefaultLifetime;

        // Correct Instantiate signature: (prefab, position, rotation, parent)
        var instance = Instantiate(effect.prefab, position, rotation, parent != null ? parent : m_DefaultParent);

        // Enqueue for timed cleanup
        m_cleanupQueue.Enqueue(new SpawnedVfx
        {
            go = instance,
            deathTime = Time.time + lifetime
        });

        return instance;
    }


    /// <summary>
    /// Convenience overload: spawn at a UI/world transform's position, parented under it.
    /// </summary>
    public GameObject PlayVFX(VFXType vfx, Transform atTransform, float lifetime = -1f)
    {
        if (atTransform == null)
            return PlayVFX(vfx); // fall back to defaults

        return PlayVFX(vfx, atTransform.position, atTransform.rotation, atTransform, lifetime);
    }
}
