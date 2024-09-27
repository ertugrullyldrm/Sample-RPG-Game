using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class SwordSlash_ISkill : MonoBehaviour, ISkill
{

    // Skill casted
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    // -- Custom skill variables
    public AnimationClip casterAnimationClip;
    public float skillStartTime = 1.5f;

    public float zOffset = 1.5f;
    Vector3 offset;

    private void Start()
    {

        // Check skill for prevent bugs
        if ( SkillUtilities.isSkillNull(skill) == false)
            return;
        SkillUtilities.ShowHUDCastMessage(skill);

        // Custom variables start
        offset = new Vector3(0, 0, zOffset);

        // Initiate this skill configs
        Initiate();

    }


    // --------- - ISkill interface functions - ---------

    public void Initiate()
    {

        // Positionate this prefab on caster center
        transform.position = SkillUtilities.GetCasterCenterPosition(skill, offset);

        // Always call the beginning of the character's animation
        ExecuteCharacterAnimation();
    }

    public void ExecuteCharacterAnimation()
    {
        // Remove the prefix of animation, like SwordSlash_Animation to SwordSlash
        string animationName = Utils.NoSufix( casterAnimationClip.name );
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

        // Start the particles system. If it has a mesh, shoud be enabled here
        GetComponent<ParticleSystem>().Play();

        // Apply this skill effect
        if (skill.target.GetComponent<CharacterCombat>() != null)
            skill.target.GetComponent<CharacterCombat>().TakeHitFrom(skill);

    }

}
