using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    [SerializeField] private Character[] m_characters;

    public Character CurrentCharacter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate CharacterManager detected. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        //TODO: initializer
        SelectCharacter(m_characters[0]);
    }

    public void OpenCharacterSelector()
    {
        StateManager.Instance.SwitchState(AppState.Characters);
    }

    public Character[] GetCharacters()
    {
        return m_characters;
    }

    public void SelectCharacter(Character character)
    {
        CurrentCharacter = character;
    }
}
