using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour
{
    [SerializeField] private Button heroButton;
    [SerializeField] public CharacterData characterData;
    public Image heroImage;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charDescription;
    public TextMeshProUGUI charPrice;


    private void Start()
    {
        heroButton.onClick.AddListener(HeroButtonOnClick);
    }

    private void HeroButtonOnClick()
    {
        if (characterData != null && !HeroShopContent.Instance.userData.BoughtCharacters.Contains(characterData.characterType) && charPrice.text != "Equip" && charPrice.text != "Equipped")
        {
            BuyCharacter();
        }
        else
        {
            EquipCharacter();
        }
    }

    private void BuyCharacter()
    {
        if (HeroShopContent.Instance.userData.CurrentCoins >= characterData.characterCost)
        {
            HeroShopContent.Instance.userData.BoughtCharacters.Add(characterData.characterType);
            HeroShopContent.Instance.userData.CurrentCoins -= characterData.characterCost;
            SaveManager.Instance.SaveData(HeroShopContent.Instance.userData);
            HeroShopContent.Instance.ShowSkinAvailability(this); // Update all characters
            UIManager.Instance.currentCoinText.text = GameManager.Instance.UserData.CurrentCoins.ToString();
        }
    }

    private void EquipCharacter()
    {
        HeroShopContent.Instance.userData.EquippedCharacter = characterData.characterType;
        SaveManager.Instance.SaveData(HeroShopContent.Instance.userData);
        HeroShopContent.Instance.ShowSkinAvailability(this); // Update all characters
    }

}
