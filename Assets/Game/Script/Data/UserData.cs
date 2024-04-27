using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    [SerializeField]
    private string username;
    public string Username { get => username; set { username = value; } }


    [SerializeField]
    private int equippedWeapon;
    public int EquippedWeapon { get => equippedWeapon; set { equippedWeapon = value; } }


    [SerializeField]
    private CharacterType equippedCharacter;
    public CharacterType EquippedCharacter { get => equippedCharacter; set { equippedCharacter = value; } }


    [SerializeField]
    private List<CharacterType> boughtCharacters = new List<CharacterType>();
    public List<CharacterType> BoughtCharacters { get => boughtCharacters; set { boughtCharacters = value; } }


    [SerializeField]
    private int currentCoins = 1000;
    public int CurrentCoins { get => currentCoins; set { currentCoins = value; } }

    public UserData()
    {
        EquippedCharacter = CharacterType.Knight;
        CurrentCoins = 10000;
        BoughtCharacters.Add(CharacterType.Knight);
        Username = "Hoang Long Dz";
    }
}
