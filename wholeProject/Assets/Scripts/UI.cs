using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] GameObject[] hearts;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] GameObject keyIcon;
    [SerializeField] TextMeshProUGUI levelText;
    public RawImage miniMap;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateHealth(int health)
    {
        for (int x = 0; x <  hearts.Length; x++)
        {
            hearts[x].SetActive(x < health);
        }
    }

    public void UpdateCoinText(int coin)
    {
        coins.text = coin.ToString();
    }

    public void ToggleKeyIcon (bool toggle)
    {
        keyIcon.SetActive(toggle);
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = "Level " + level;
    }
}
