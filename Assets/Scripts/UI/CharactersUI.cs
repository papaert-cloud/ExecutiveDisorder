using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharactersUI : MonoBehaviour
{
    [SerializeField] private Image m_standInImage;
    [SerializeField] private Transform m_characterCardsHolder;
    [SerializeField] private GameObject m_characterCardPrefab;
    [SerializeField] private Transform m_initialResourcesHolder;
    [SerializeField] private GameObject m_initialResourcePrefab;
    [SerializeField] private ToggleGroup m_toggleGroup;
    [SerializeField] private Character m_currentCharacterData;
    [SerializeField] private TextMeshProUGUI m_characterNameText;

    [SerializeField] private List<TagSliderUI> m_initialResourcesList = new List<TagSliderUI>();

    private bool IsInitialized = false;

    private void Start()
    {
        TryInitialize();
    }

    private void TryInitialize()
    {
        if (IsInitialized) return;
        IsInitialized = true;
        foreach (Character character in CharacterManager.Instance.GetCharacters())
        {
            GameObject characterObject = Instantiate(m_characterCardPrefab, m_characterCardsHolder);
            CharacterCardUI characterCardUI = characterObject.GetComponent<CharacterCardUI>();
            characterCardUI.UpdateCharacterCardUI(character);
            Toggle characterToggle = characterObject.GetComponent<Toggle>();
            characterToggle.group = m_toggleGroup;
            characterToggle.onValueChanged.AddListener(OnCharacterCardSelected);
        }

        for(int i = 0; i < 4; i++)
        {
            GameObject initialResourceObject = Instantiate(m_initialResourcePrefab, m_initialResourcesHolder);
            TagSliderUI resourceSlider = initialResourceObject.GetComponent<TagSliderUI>();
            m_initialResourcesList.Add(resourceSlider);
        }
    }

    private void OnCharacterCardSelected(bool isSelected)
    {
        if(!isSelected) return;
        m_currentCharacterData = m_toggleGroup.GetFirstActiveToggle().GetComponent<CharacterCardUI>().CardCharacter;

        m_standInImage.sprite = m_currentCharacterData.FullBody;

        m_characterNameText.text = m_currentCharacterData.CharacterName;

        m_initialResourcesList[0].SetTagSliderUI("Popularity", (int)m_currentCharacterData.InitialPopularity);
        m_initialResourcesList[1].SetTagSliderUI("Stability", (int)m_currentCharacterData.InitialStability);
        m_initialResourcesList[2].SetTagSliderUI("Media", (int)m_currentCharacterData.InitialMedia);
        m_initialResourcesList[3].SetTagSliderUI("Economic", (int)m_currentCharacterData.InitialEconomic);
    }

    public void OnBackClicked()
    {
        StateManager.Instance.SwitchState(AppState.MainMenu);
    }

    public void SelectCurrentCharacter() 
    {
        CharacterManager.Instance.SelectCharacter(m_currentCharacterData);
    }
}
