using UnityEngine;
using UnityEngine.UI;

public class CharacterCardUI : MonoBehaviour
{
    [SerializeField] private Image m_characterImage;
    [SerializeField] private GameObject m_characterInfoHolder;
    [SerializeField] private GameObject m_characterInfoPrefab;

    public Character CardCharacter;

    public void UpdateCharacterCardUI(Character characterData)
    {
        CardCharacter = characterData;
        m_characterImage.sprite = characterData.Portrait;
        CreateInfoUI(characterData);
    }

    private void CreateInfoUI(Character characterData)
    {
        GameObject governTitleUI = Instantiate(m_characterInfoPrefab, m_characterInfoHolder.transform);
        governTitleUI.GetComponent<InfoBoxUI>().SetInfoUI(characterData.GovernTitle);
        GameObject partyAffiliation = Instantiate(m_characterInfoPrefab, m_characterInfoHolder.transform);
        partyAffiliation.GetComponent<InfoBoxUI>().SetInfoUI(characterData.PartyAffiliation);
    }
}
