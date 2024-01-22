using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;

    private const string PLAYER_POS_X = "PlayerPositionX";
    private const string PLAYER_POS_Y = "PlayerPositionY";
    private const string TOTAL_COINS = "TotalCoins";

    private const string SAVE_FILE_PATH = "/save.json";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveJson();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadJson();
        }

        // Solo tiene que ver con PlayerPrefs
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Deleted");
            PlayerPrefs.DeleteAll();
        }
    }

    private void Save()
    {
        Debug.Log("Save!");
        
        // POSITION
        Vector3 pos = playerController.GetPosition();
        PlayerPrefs.SetFloat(PLAYER_POS_X, pos.x);
        PlayerPrefs.SetFloat(PLAYER_POS_Y, pos.y);
        Debug.Log($"Position: {pos}");

        // TOTAL COINS
        int totalCoins = playerController.GetTotalCoins();
        PlayerPrefs.SetInt(TOTAL_COINS, totalCoins);
        Debug.Log($"Total Coins: {totalCoins}");
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(PLAYER_POS_X))
        {
            Debug.Log("Load!");
            // POSITION
            float x = PlayerPrefs.GetFloat(PLAYER_POS_X);
            float y = PlayerPrefs.GetFloat(PLAYER_POS_Y);
            playerController.SetPosition(new Vector3(x, y, 0));
            Debug.Log($"Position: ({x}, {y})");

            // TOTAL COINS
            int totalCoins = PlayerPrefs.GetInt(TOTAL_COINS, 0);
            playerController.SetTotalCoins(totalCoins);
            Debug.Log($"Total Coins: {totalCoins}");
        }
        else
        {
            // No debería de ocurrir NUNCA
            Debug.LogError("No Data");
        }
        
    }


    private void SaveJson()
    {
        Debug.Log("Saved with JSON");
        Vector3 pos = playerController.GetPosition();
        int totalCoins = playerController.GetTotalCoins();

        SaveObject saveObject = new SaveObject
        {
            playerPosition = pos,
            coins = totalCoins
        };

        string savedObjectJson = JsonUtility.ToJson(saveObject);

        File.WriteAllText(Application.dataPath + SAVE_FILE_PATH, savedObjectJson);
    }

    private void LoadJson()
    {
        Debug.Log("Loaded with JSON");
        if (File.Exists(Application.dataPath + SAVE_FILE_PATH))
        {
            string savedObjectString = File.ReadAllText(Application.dataPath + SAVE_FILE_PATH);

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(savedObjectString);

            playerController.SetPosition(saveObject.playerPosition);
            playerController.SetTotalCoins(saveObject.coins);
        }
        else
        {
            // Aquí no tendríamos que caer nunca
            Debug.LogError("No save file");
        }
    }
}
