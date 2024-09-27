using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public static GameManager gameManager;

    public static Transform messages;
    public static Transform messagesHoveredTarget;
    public static Transform messagesSelectedTarget;
    public static Transform messagesDebug;

    public static Transform hud;
    public static Transform healthbar;
    public static RectTransform lifebar;
    public static TextMeshProUGUI currentLife;
    public static TextMeshProUGUI maximumLife;
    public static RectTransform manabar;
    public static TextMeshProUGUI currentMana;
    public static TextMeshProUGUI maximumMana;
    public static RectTransform levelPanel;
    public static TextMeshProUGUI levelText;
    public static RectTransform levelbarValue;

    public static float lifebarOriginalWidth = 0;
    public static float lifebarOriginalHeight = 0;
    public static float manabarOriginalWidth = 0;
    public static float manabarOriginalHeight = 0;
    public static float levelbarOriginalWidth = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        hud = GameObject.FindWithTag("UI").transform.Find("HUD").transform;

        messages = hud.Find("Messages");
        messagesHoveredTarget = messages.Find("HoveredTarget");
        messagesSelectedTarget = messages.Find("SelectedTarget");
        messagesDebug = messages.Find("Debug");

        healthbar = hud.Find("Healthbar").transform;
        lifebar = healthbar.Find("Life").Find("Lifebar").GetComponent<RectTransform>();
        manabar = healthbar.Find("Mana").Find("Manabar").GetComponent<RectTransform>();
        currentLife = healthbar.Find("Life").Find("Text").Find("Current").GetComponent<TextMeshProUGUI>();
        maximumLife = healthbar.Find("Life").Find("Text").Find("Maximum").GetComponent<TextMeshProUGUI>();
        currentMana = healthbar.Find("Mana").Find("Text").Find("Current").GetComponent<TextMeshProUGUI>();
        maximumMana = healthbar.Find("Mana").Find("Text").Find("Maximum").GetComponent<TextMeshProUGUI>();
        levelPanel = hud.Find("LevelPanel").GetComponent<RectTransform>();
        levelText = levelPanel.Find("LevelContent").Find("Text").GetComponent<TextMeshProUGUI>();
        levelbarValue = levelPanel.Find("Levelbar").Find("LevelbarValue").GetComponent<RectTransform>();

        lifebarOriginalWidth = lifebar.sizeDelta.x;
        lifebarOriginalHeight = lifebar.sizeDelta.y;
        manabarOriginalWidth = manabar.sizeDelta.x;
        manabarOriginalHeight = manabar.sizeDelta.y;
        levelbarOriginalWidth = levelPanel.Find("Levelbar").Find("LevelbarBackground")
            .GetComponent<RectTransform>().sizeDelta.x;

        // Prevent stop the player. Check to fix it later
        if (gameManager == null)
            return;
        UpdateHealthbar();

    }

    public static void UpdateHealthbar()
    {
        UpdateLifebar();
        UpdateManabar();
    }

    public static void UpdateLifebar()
    {
        if (gameManager == null)
            return;
        int currentLifeNumber = gameManager.player.GetComponent<CharacterStatus>().currentLife;
        currentLife.text = currentLifeNumber.ToString();
        int maximumLifeNumber = gameManager.player.GetComponent<CharacterStatus>().maximumLife;
        maximumLife.text = maximumLifeNumber.ToString();
        float x = lifebarOriginalWidth *  ( (float)currentLifeNumber / (float)maximumLifeNumber );
        lifebar.sizeDelta = new Vector2( x , lifebar.sizeDelta.y);
    }

    public static void UpdateManabar()
    {
        if (gameManager == null)
            return;
        int currentManaNumber = gameManager.player.GetComponent<CharacterStatus>().currentMana;
        currentMana.text = currentManaNumber.ToString();
        int maximumManaNumber = gameManager.player.GetComponent<CharacterStatus>().maximumMana;
        maximumMana.text = maximumManaNumber.ToString();
        float x = manabarOriginalWidth * ((float)currentManaNumber / (float)maximumManaNumber);
        manabar.sizeDelta = new Vector2(x, manabar.sizeDelta.y);
    }

    public static void UpdateExperiencebar(float experienceAccumulated, int nextLevelExperience)
    {
        float experienceProgress = experienceAccumulated / nextLevelExperience;
        float newBarSize = experienceProgress * levelbarOriginalWidth;
        levelbarValue.sizeDelta = new Vector2(newBarSize, levelbarValue.sizeDelta.y);
    }

    public static void UpdateLevelContent(int level)
    {
        levelText.text = level.ToString();
    }

    public static void SetHoveredTarget( string message)
    {
        messagesHoveredTarget.GetComponent<TextMeshProUGUI>().text = message;
    }
    public static void SetSelectedTarget( string message)
    {
        messagesSelectedTarget.GetComponent<TextMeshProUGUI>().text = message;
    }

    public static void SetMessageDebug( string message)
    {
        messagesDebug.GetComponent<TextMeshProUGUI>().text = message;
    }

}
