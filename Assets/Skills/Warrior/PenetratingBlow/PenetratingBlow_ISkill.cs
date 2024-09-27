using System;
using UnityEngine;

public class PenetratingBlow_ISkill : MonoBehaviour, ISkill
{

    // Skill interface refering to original casted skill
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    public float strengthScale = 0.3f;
    public float agilityScale = 0.4f;
    public float skillStartTime = 1f;

    int targetsHitCount = 0;

    private void Start()
    {
        // Check skill for prevent bugs
        if (SkillUtilities.isSkillNull(skill) == false)
            return;
        SkillUtilities.ShowHUDCastMessage(skill);

        transform.position = SkillUtilities.GetCasterCenterPosition(skill);
        Utils.LookAtYZ(transform, skill.target.position);

        // Initiate this skill configs
        ExecuteCharacterAnimation();

    }

    public void ExecuteCharacterAnimation()
    {
        // Play the animation by its name in caster animator
        skill.caster.GetComponent<Animator>().Play("PenetratingBlow");
        // Wait characters animation time to executes the skill animation
        Invoke("ExecuteSkillAnimation", skillStartTime);
    }

    public void ExecuteSkillAnimation()
    {

        // Cancel and destroy object if there's target anymore
        if (skill.target == null)
        {
            Destroy(gameObject);
            return;
        }

        GetComponent<Collider>().enabled = true;
        GetComponent<ParticleSystem>().Play();

        targetsHitCount++;
        skill.target.GetComponent<CharacterCombat>().TakeDamageFrom(skill.caster, GetCustomDamage() );

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (targetsHitCount == 2)
            return;

        if (collider.CompareTag("Enemy") && collider.GetComponent<CharacterCombat>() != null)
        {
            if (collider.transform == skill.target)
                return;

            targetsHitCount++;
            collider.GetComponent<CharacterCombat>().TakeDamageFrom(skill.caster, GetCustomDamage());
        }
    }

    int GetCustomDamage()
    {
        if (skill.caster == null)
            return 0;

        int baseDamage = UnityEngine.Random.Range(skill.minDamage, skill.maxDamage);

        int casterStrength = skill.caster.GetComponent<CharacterStatus>().GetStatus("strength");
        int casterAgility = skill.caster.GetComponent<CharacterStatus>().GetStatus("agility");

        float strengthDamage = casterStrength * strengthScale;
        float agilityDamage = casterAgility * agilityScale;

        float finalDamage = baseDamage + ( targetsHitCount == 1 ? strengthDamage  : agilityDamage);
        finalDamage = (float)Math.Floor(finalDamage);

        return (int) finalDamage;

    }

}
