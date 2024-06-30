using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class has System.Serializable to serve the Inspector Visibility
//This class holds the information, storing data of the player
[System.Serializable]
public class UserData
{
    //Each data member has a corresponding public property with a get and set accessor.
    //These properties provide a safe way to access and modify the underlying data members.
    
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

    //This public constructor is called when a new instance of the class is created. In this case, it sets some default values for the user data
    public UserData()
    {
        EquippedCharacter = CharacterType.Knight;
        CurrentCoins = 10000;
        BoughtCharacters.Add(CharacterType.Knight);
        BoughtCharacters.Add(CharacterType.Gunner);
        Username = "Hoang Long Dz";
    }
}
