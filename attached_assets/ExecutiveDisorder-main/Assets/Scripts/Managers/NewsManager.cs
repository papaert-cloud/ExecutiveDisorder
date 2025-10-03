using System;
using UnityEngine;

public class NewsManager : MonoBehaviour
{
    public static NewsManager Instance;

    [SerializeField] private GameObject MeMeUI;

    public Action<MediaReaction> OnReactionTaken;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate NewsManager detected. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void EnterMeMedia()
    {
        StateManager.Instance.SwitchState(AppState.FakeNews);
    }

    private void Start()
    {
        MeMeUI.GetComponent<MeMediaUI>().OnReactionTaken += OnReactionMade;
    }

    private void OnReactionMade(MediaReaction reaction) 
    {
        OnReactionTaken?.Invoke(reaction);
    }
}
