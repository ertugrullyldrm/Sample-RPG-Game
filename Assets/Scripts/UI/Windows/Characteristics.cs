using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Characteristics : Windows
{

    public TMP_InputField currentLife;
    public TMP_InputField maximumLife;
    public TMP_InputField currentMana;
    public TMP_InputField maximumMana;
    public TMP_InputField strength;
    public TMP_InputField intelligence;
    public TMP_InputField agility;
    public TMP_InputField lifeRegen;
    public TMP_InputField manaRegen;

    private void Start()
    {
        base.Start();

        currentLife = transform.Find("Life").Find("InputCurrent").GetComponent<TMP_InputField>();
        maximumLife = transform.Find("Life").Find("InputMaximum").GetComponent<TMP_InputField>();
        currentMana = transform.Find("Mana").Find("InputCurrent").GetComponent<TMP_InputField>();
        maximumMana = transform.Find("Mana").Find("InputMaximum").GetComponent<TMP_InputField>();
        strength = transform.Find("Strength").Find("Input").GetComponent<TMP_InputField>();
        intelligence = transform.Find("Intelligence").Find("Input").GetComponent<TMP_InputField>();
        agility = transform.Find("Agility").Find("Input").GetComponent<TMP_InputField>();
        lifeRegen = transform.Find("LifeRegen").Find("Input").GetComponent<TMP_InputField>();
        manaRegen = transform.Find("ManaRegen").Find("Input").GetComponent<TMP_InputField>();

        UpdateInfo();

    }

    void UpdateInfo()
    {
        currentLife.text = base.characterStatus.currentLife.ToString();
        maximumLife.text = base.characterStatus.maximumLife.ToString();
        currentMana.text = base.characterStatus.currentMana.ToString();
        maximumMana.text = base.characterStatus.maximumMana.ToString();
        strength.text = base.characterStatus.GetStatus("strength").ToString();
        intelligence.text = base.characterStatus.GetStatus("intelligence").ToString();
        agility.text = base.characterStatus.GetStatus("agility").ToString();
        lifeRegen.text = base.characterStatus.lifeRegen.ToString();
        manaRegen.text = base.characterStatus.manaRegen.ToString();
    }

    public void SetMain()
    {
        base.characterStatus.currentLife = int.Parse( currentLife.text );
        base.characterStatus.maximumLife = int.Parse( maximumLife.text );
        base.characterStatus.currentMana = int.Parse(currentMana.text);
        base.characterStatus.maximumMana = int.Parse(maximumMana.text);
        base.characterStatus.status["strength"]["points"] = int.Parse( strength.text );
        base.characterStatus.status["intelligence"]["points"] = int.Parse(intelligence.text);
        base.characterStatus.status["agility"]["points"] = int.Parse(agility.text);
        HUD.UpdateHealthbar();
    }

    public void SetSecondary()
    {
        base.characterStatus.lifeRegen = int.Parse( lifeRegen.text );
        base.characterStatus.manaRegen = int.Parse( manaRegen.text );
    }
}
