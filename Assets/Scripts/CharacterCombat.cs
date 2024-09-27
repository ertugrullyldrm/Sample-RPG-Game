using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{

    [HideInInspector]
    public Skill selectedSkill;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public bool isCasting = false;

    Animator animator;
    CharacterStatus characterStatus;
    Transform damageDisplay; // pensar numa forma otimizada

    protected void Start()
    {
        animator = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
        damageDisplay = GameObject.FindWithTag("Display").transform.Find("Damage").transform;
    }

    // Rever o conceito dessa função e se ela é relevante ela existir
    public bool TakeHitFrom(Skill skill)
    {
        //TakeDamage(skill.damage);
        return true;
    }

    public void TakeDamageFrom(Transform from, int damage)
    {
        characterStatus.currentLife -= damage;
        HUD.UpdateLifebar();
        DisplayDamage(damage);
        if (VerifyIsDead())
        {
            // Dont work on final build... why?
            //from.GetComponent<CharacterCombat>().EnemyIsDeadCallback(transform);
            Destroy(gameObject);
        }
    }

    public void ConsumeMana(int value)
    {
        characterStatus.currentMana -= value;
        HUD.UpdateManabar();
    }

    bool VerifyIsDead()
    {
        if (characterStatus.currentLife <= 0)
        {
            HUD.SetMessageDebug($"O alvo [{characterStatus.name}] está morto");
            return true;
        }
        return false;
    }

    void DisplayDamage(int value)
    {
        Transform instance = Instantiate(damageDisplay, transform);
        instance.gameObject.SetActive(true);
        instance.Find("Value").GetComponent<TextMeshProUGUI>().text = value.ToString();
        Vector3 position = transform.position;
        position.y = transform.GetComponent<Collider>().bounds.size.y;
        instance.transform.position = position;
    }

    public void EnemyIsDeadCallback( Transform enemy )
    {
        
        characterStatus.EarnExperience(enemy.GetComponent<CharacterStatus>().experienceLoot);

        // Check class passives and events
        if( GetComponent<PlayerCombat>())
        {
            GetComponent<PlayerCombat>().EventCallback("EnemyIsDead");
        }

    }

}
