using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{

    public string name = "None";

    public int level = 1;
    [SerializeField] float experienceAccumulated = 0;
    public float experienceLoot = 0;

    [Header("Health")]
    public int currentLife = 100;
    [HideInInspector] public int maximumLife = 100;
    public int currentMana = 50;
    [HideInInspector] public int maximumMana = 50;
    public float regenTimeSeconds = 1.5f;
    public int lifeRegen = 1;
    public int manaRegen = 2;

    [Header("Attack basis")]
    public float range = Constants.rangeDistanceOffset;
    public float attackSpeed = 0.3f;
    public float moveSpeed = 5;

    [Header("Attributes")]
    public Dictionary<string, Dictionary<string, int>> status = new Dictionary<string, Dictionary<string, int>>() {
        { "strength", new Dictionary<string, int>() {
            { "points", 0 },
            { "bonus", 0 },
            { "modifier", 0 }
        }},
        { "intelligence", new Dictionary<string, int>() {
            { "points", 0 },
            { "bonus", 0 },
            { "modifier", 0 }
        }},
        { "agility", new Dictionary<string, int>() {
            { "points", 0 },
            { "bonus", 0 },
            { "modifier", 0 }
        }},
        { "vitality", new Dictionary<string, int>() {
            { "points", 0 },
            { "bonus", 0 },
            { "modifier", 0 }
        }}
    };

    private Dictionary<int, int> experienceTable = new Dictionary<int, int>()
    {
        { 1, 0 },
        { 2, 100 },
        { 3, 200 },
        { 4, 300 },
        { 5, 400 },
        { 6, 500 }
    };

    private void Start()
    {
        HUD.UpdateHealthbar();
    }

    public void StartRegen()
    {
        // Check a possibility to stop this
        StartCoroutine(RecoverRegen());
        IEnumerator RecoverRegen()
        {
            yield return new WaitForSeconds(regenTimeSeconds);
            RecoverLifeAndMana(lifeRegen, manaRegen);
            StartRegen();
        }
    }

    public int GetStatus(string statusName)
    {
        return status[statusName]["points"] + status[statusName]["bonus"] + status[statusName]["modifier"];
    }

    public void AddBonus(string statusName, int value, float time)
    {
        StartCoroutine(AddBonus(statusName, value, time, true));
    }
    private IEnumerator AddBonus(string statusName, int value, float time, bool thisClass = true)
    {
        status[statusName]["bonus"] += value;
        yield return new WaitForSeconds(time);
        RemoveBonus(statusName, value);
    }

    public void RemoveBonus(string statusName, int value = -1)
    {
        if( value == -1)
            status[statusName]["bonus"] = 0;
        else
            status[statusName]["bonus"] -= value;

        if( status[statusName]["bonus"] < 0 )
            status[statusName]["bonus"] = 0;
    }

    public void RecoverLifeAndMana( int lifeValue, int manaValue)
    {
        RecoverLife(lifeValue);
        RecoverMana(manaValue);
    }

    public void RecoverLife(int value)
    {

        if (currentLife >= maximumLife)
            return;

        if (value + currentLife > maximumLife)
        {
            value = maximumLife - currentLife;
        }

        currentLife += value;
        HUD.UpdateLifebar();

    }

    public void RecoverMana( int value )
    {

        if (currentMana >= maximumMana)
            return;

        if( value + currentMana> maximumMana)
        {
            value = maximumMana - currentMana;
        }

        currentMana+= value;
        HUD.UpdateLifebar();

    }

    public void EarnExperience(float value)
    {
        // Adiciona a quantidade de experiência ganha à variável experience
        experienceAccumulated += value;

        // Verifica se o personagem alcançou ou ultrapassou a quantidade de experiência necessária para o próximo nível
        if (experienceAccumulated >= experienceTable[level+1] )
        {
            // Incrementa o nível do personagem
            // Subtrai a experiência necessária para alcançar o próximo nível da experiência atual
            LevelUp();
            experienceAccumulated -= experienceTable[level];
        }

        HUD.UpdateExperiencebar(experienceAccumulated, experienceTable[level+1]);

    }

    void LevelUp()
    {
        level++;
        Debug.Log("Subiu de level");
        HUD.UpdateLevelContent(level);
    }

}
