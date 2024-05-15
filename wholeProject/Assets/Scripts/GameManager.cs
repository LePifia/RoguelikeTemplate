using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int level;
    public int baseSeed;
    public int prevRoomPlayerHealth;
    public int prevRoomPlayerCoins;
    private Player player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        level = 1;
        baseSeed = PlayerPrefs.GetInt("Seed");
        Random.InitState(baseSeed);

        Generation.Instance.Generate();
        UI.instance.UpdateLevelText(level);

        player = FindObjectOfType<Player>();

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    public void GoToNextLevel()
    {
        prevRoomPlayerHealth = player.GetCurrentHealth();
        prevRoomPlayerCoins = player.GetCurrentCoins();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Game")
        {
            Destroy(gameObject);
            return;
        }

        player = FindObjectOfType<Player>();
        level++;
        baseSeed++;

        Generation.Instance.Generate();
        player.SetCoins(prevRoomPlayerCoins);
        player.SetHealth(prevRoomPlayerHealth);
       

        UI.instance.UpdateHealth(prevRoomPlayerHealth);
        UI.instance.UpdateCoinText(prevRoomPlayerCoins);
        UI.instance.UpdateLevelText(level);

    }
}
