using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class Claw_ISkill : MonoBehaviour, ISkill
{

    // Skill interface refering to original casted skill
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    // -- Custom skill variables
    public AnimationClip casterAnimationClip;
    public float skillStartTime = 1.5f;

    private void Start()
    {

        // Check skill for prevent bugs
        if ( SkillUtilities.isSkillNull(skill) == false)
            return;

        ExecuteCharacterAnimation();

    }


    public void ExecuteCharacterAnimation()
    {
        // Remove the prefix of animation, like SwordSlash_Animation to SwordSlash
        string animationName = skill.name;
        // Play the animation by its name in caster animator
        skill.caster.GetComponent<Animator>().Play(animationName);
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

        // Positionate this prefab on caster center
        transform.position = SkillUtilities.GetTargetCenterPosition(skill);

        // Start the particles system. If it has a mesh, shoud be enabled here
        GetComponent<ParticleSystem>().Play();

        // Apply this skill effect
        if (skill.target.GetComponent<CharacterCombat>() != null)
            skill.target.GetComponent<CharacterCombat>().TakeDamageFrom(skill.caster, skill.GetDamage());

    }

}
