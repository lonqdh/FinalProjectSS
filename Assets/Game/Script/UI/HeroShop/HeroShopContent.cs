using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroShopContent : Singleton<HeroShopContent>
{
    public CharacterDataSO characterDatabase;
    public UserData userData;
    [SerializeField] private HeroButton heroBtnPrefab;
    [SerializeField] private Transform heroItemParent;
    public List<CharacterData> charactersList = new List<CharacterData>();
    public List<HeroButton> heroesButtonList = new List<HeroButton>();

    private void Start()
    {
        userData = GameManager.Instance.UserData;
    }

    public void SpawnCharacters()
    {
        foreach(HeroButton obj in heroesButtonList)
        {
            Destroy(obj.gameObject);
        }

        heroesButtonList.Clear();

        for (int i = 0; i < characterDatabase.characterDataList.Count; i++)
        {
            HeroButton heroButton = Instantiate(heroBtnPrefab, heroItemParent);
            heroButton.characterData = characterDatabase.characterDataList[i];
            heroButton.charName.SetText(characterDatabase.characterDataList[i].characterType.ToString());
            // Check if the character is not owned
            ShowSkinAvailability(heroButton);
            heroesButtonList.Add(heroButton);
        }
    }

    //public void ShowSkinAvailability(HeroButton heroBtn)
    //{
    //    //for(int i = 0; i < heroesButtonList.Count; i++)
    //    //{
    //        if (!GameManager.Instance.UserData.BoughtCharacters.Contains(heroBtn.characterData.characterType))
    //        {
    //            heroBtn.charPrice.SetText(heroBtn.characterData.characterCost.ToString());
    //        }
    //        else
    //        {
    //            // Check if the character is equipped
    //            if (GameManager.Instance.UserData.EquippedCharacter == heroBtn.characterData.characterType)
    //            {
    //                heroBtn.charPrice.SetText("Equipped");
    //            }
    //            else
    //            {
    //                // If the character is owned but not equipped, display "Equip"
    //                heroBtn.charPrice.SetText("Equip");
    //            }
    //        }
    //}


    public void ShowSkinAvailability(HeroButton selectedHeroBtn = null)
    {
        foreach (HeroButton heroBtn in heroesButtonList)
        {
            if (!GameManager.Instance.UserData.BoughtCharacters.Contains(heroBtn.characterData.characterType))
            {
                heroBtn.charPrice.SetText(heroBtn.characterData.characterCost.ToString());
            }
            else
            {
                // Check if the character is equipped
                if (GameManager.Instance.UserData.EquippedCharacter == heroBtn.characterData.characterType)
                {
                    heroBtn.charPrice.SetText("Equipped");
                }
                else
                {
                    // If the character is owned but not equipped, display "Equip"
                    heroBtn.charPrice.SetText("Equip");
                }
            }
        }

        // Update the selected hero button separately
        if (selectedHeroBtn != null)
        {
            // Check if the character is equipped
            if (GameManager.Instance.UserData.EquippedCharacter == selectedHeroBtn.characterData.characterType)
            {
                selectedHeroBtn.charPrice.SetText("Equipped");
            }
            else
            {
                // If the character is owned but not equipped, display "Equip"
                selectedHeroBtn.charPrice.SetText("Equip");
            }
        }
    }

}
