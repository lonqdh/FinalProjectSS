using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu = 0,
    Gameplay = 1,
    Finish = 2,
    LevelUp = 3,
    ChangingLevel = 4,
    Pause = 5,
}

//This class manages the current state of the game as well as does the loading data of the player when the game starts ( the player logging in/starting the game )

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameState state;
    public UserData UserData;

    private void Awake()
    {
        //setup tong quan game
        //setup data
        ChangeState(GameState.MainMenu);

        if (SaveManager.Instance.HasData<UserData>())
        {
            Debug.Log("Load Data");
            UserData = SaveManager.Instance.LoadData<UserData>();
            //Debug.Log(UserData.ToString());
        }
        else
        {
            Debug.Log("Created Data");
            UserData = new UserData();
            SaveManager.Instance.SaveData(UserData);
        }
        
    }


    public void ChangeState(GameState gameState)
    {
        state = gameState;
        Debug.Log(state);
    }

    public bool IsState(GameState gameState)
    {
        return state == gameState;
    }

}
