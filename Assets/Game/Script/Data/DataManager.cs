using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public CharacterDataSO characterDataSO;

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
