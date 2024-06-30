using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class with its name can cause a little confusion, it only has one function which is to get the data of the current character the player is using
public class DataManager : Singleton<DataManager>
{
    public CharacterDataSO characterDataSO;
    
    //Lấy Data của nhân vật đang sử dụng
    public CharacterData GetCharacterData(CharacterType characterType)
    {
        for (int i = 0; i < characterDataSO.characterDataList.Count; i++)
        {
            if (characterDataSO.characterDataList[i].characterType == characterType)
            {
                return characterDataSO.characterDataList[i];
            }
        }
        return null;
    }

}
