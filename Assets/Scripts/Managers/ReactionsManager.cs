using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ReactionsManager : MonoBehaviour
{
    public static ReactionsManager Instance;

    [Header("Prefab")]
    [SerializeField] private ReactionWidget reactionPrefab;   // <-- assign your Reaction UI prefab here
    [SerializeField] private Canvas parentCanvas;             // optional; where to parent instances (defaults to this.transform)

    [Header("Sprites")]
    [SerializeField] private Sprite positiveSprite;
    [SerializeField] private Sprite negativeSprite;

    [Header("Anchors")]
    [SerializeField] private RectTransform positiveAnchor;
    [SerializeField] private RectTransform negativeAnchor;

    [Header("Bubble Local Offsets")]
    [SerializeField] private Vector2 positiveBubbleOffset = new Vector2(100f, 50f);
    [SerializeField] private Vector2 negativeBubbleOffset = new Vector2(-100f, 50f);

    [SerializeField] List<string> positiveReactions;
    [SerializeField] List<string> negativeReactions;

    public Action OnReactionFinished; // fires after each individual instance completes

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    /// <summary>Spawns a positive reaction instance.</summary>
    public void PlayPositiveReaction(string message = "Default")
    {
        if(message.Equals("Default"))
        {
            int randomIndex = UnityEngine.Random.Range(0, positiveReactions.Count);
            message = positiveReactions[randomIndex];
        }
        SpawnReaction(positiveSprite, positiveAnchor, positiveBubbleOffset, message);
    }

    /// <summary>Spawns a negative reaction instance.</summary>
    public void PlayNegativeReaction(string message = "Default")
    {
        if (message.Equals("Default"))
        {
            int randomIndex = UnityEngine.Random.Range(0, negativeReactions.Count);
            message = negativeReactions[randomIndex];
        }
        SpawnReaction(negativeSprite, negativeAnchor, negativeBubbleOffset, message);
    }

    private void SpawnReaction(Sprite sprite, RectTransform anchor, Vector2 bubbleOffset, string message)
    {
        if (reactionPrefab == null)
        {
            Debug.LogWarning("[ReactionsManager] Reaction prefab not assigned.");
            return;
        }
        if (anchor == null)
        {
            Debug.LogWarning("[ReactionsManager] Anchor not assigned for this reaction.");
            return;
        }

        // Parent under canvas (or under this manager if no canvas provided)
        Transform parent = parentCanvas != null ? parentCanvas.transform : transform;

        // Instantiate and play
        var widget = Instantiate(reactionPrefab, parent);
        widget.gameObject.SetActive(true);
        widget.Play(sprite, anchor, bubbleOffset, message, () => OnReactionFinished?.Invoke());
    }
}
