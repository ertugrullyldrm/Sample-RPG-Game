using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


//[CreateAssetMenu(fileName = "Custom Item", menuName = "New Skill", order = 2)]
public class Skill : ScriptableObject
{

    public Transform skillPrefab = null;

    [HideInInspector]
    public Transform caster = null;
    [HideInInspector]
    public Transform target = null;

    [Header("Main settings")]
    public string name = "Skill";
    public Texture icon = null;
    public float countdown = 1;
    [Tooltip("Time it takes for the character to perform the skill action. During this period, he will not be able to walk.")]
    public float castingTime = 0;
    [HideInInspector] public float countdownElapsed = 0;
    public int manaCost = 0;
    [Space(10)]
    public bool isPassive = false;

    [Header("Range")]
    public int minRange = 2;
    public int maxRange = 2;
    
    [Header("Projectil")]
    public float velocity = 5;

    [Header("Damage and Heal")]
    public int minDamage = 0;
    public int maxDamage = 0;

    [Header("Attributes Scale")]
    public float strength = 0;
    public float intelligence = 0;
    public float agility = 0;
    public float vitality = 0;


    // Check if the target is in the of caster
    public bool IsTargetInCasterRange()
    {

        if (target == null || caster == null)
        {
            HUD.SetMessageDebug($"Verifque o alvo ou caster de [{name}]");
            return false;
        }

        float distance = Vector3.Distance(caster.position, target.position);

        if( Constants.DEV && caster.CompareTag("Player") )
            HUD.SetMessageDebug($"Skill [{name}] fora de alcance — Distancia [{distance.ToString("0.0")}] | Alcance {maxRange}");
        
        return distance <= maxRange;

    }

    // Return if skill countdown is minus or equals to zero
    public bool IsSkillReady()
    {

        bool isReady = countdownElapsed <= 0;
        
        if (Constants.DEV && !isReady)
            HUD.SetMessageDebug($"Skill [{name}] em recarga, {countdownElapsed.ToString("0.0")} de {countdown}");

        return isReady;

    }

    public bool HasMana()
    {
        return caster.GetComponent<CharacterStatus>().currentMana >= manaCost;
    }

    // Check if range and countdown is OK to be casted
    public bool CanCastSkill()
    {
        return IsTargetInCasterRange() && IsSkillReady() && HasMana();
    }

    // Uses the skill
    public void CastSkill()
    {
        caster.GetComponent<CharacterCombat>().ConsumeMana(manaCost);
        Transform instantiated = Instantiate( skillPrefab, Path.skillPool );
        instantiated.GetComponent<ISkill>().skill = this;
        countdownElapsed = countdown;
    }

    public int GetDamage()
    {

        if (caster == null)
            return 0;

        int baseDamage = UnityEngine.Random.Range(minDamage, maxDamage);
        
        int casterStrength = caster.GetComponent<CharacterStatus>().GetStatus("strength");
        int casterIntelligence = caster.GetComponent<CharacterStatus>().GetStatus("intelligence");
        int casterAgility = caster.GetComponent<CharacterStatus>().GetStatus("agility");
        
        float strengthDamage = casterStrength * strength;
        float intelligenceDamage = casterIntelligence * intelligence;
        float agilityDamage = casterAgility * agility;

        float finalDamage = baseDamage + ( strengthDamage + intelligenceDamage + agilityDamage );
        finalDamage = (float)Math.Floor(finalDamage);

        return (int)finalDamage;
        
    }

}
