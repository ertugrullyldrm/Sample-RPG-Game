using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class Fireball_ISkill : MonoBehaviour, ISkill
{

    // Skill interface refering to original casted skill
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    // -- This skill custom variables
    public AnimationClip casterAnimationClip;
    public float skillStartTime = 0.5f;    

    public float zOffset = 1.5f;
    Vector3 offset;
    bool isProjecting = false;

    private void Start()
    {

        // Check skill for prevent bugs
        if (SkillUtilities.isSkillNull(skill) == false)
            return;
        SkillUtilities.ShowHUDCastMessage(skill);

        // Custom variables start
        offset = new Vector3(0, 0, zOffset);

        // Initiate this skill configs
        Initiate();

    }
    
    // Update is commonly used for projectiles or skill that apply effects over the time
    private void FixedUpdate()
    {
        // Cancel update if its nothing projecting
        if (!isProjecting)
            return;

        // Cancel update and destroy object if there's target anymore
        if (skill.target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Change this prefab position to follow the target
        Vector3 moveDirection = Vector3.MoveTowards(transform.position, skill.target.position, skill.velocity * Time.deltaTime);
        moveDirection.y = SkillUtilities.GetCasterCenterY(skill);
        transform.position = moveDirection;

    }

    // --------- - ISkill interface functions - ---------

    public void Initiate()
    {

        // Disable render on start
        SwitchMeshRender(false);
        // Positionate this prefab on caster center
        transform.position = SkillUtilities.GetCasterCenterPosition(skill, offset);
        // Prefab look to the target
        Utils.LookAtYZ(transform, skill.target.position);

        // Always call the beginning of the character's animation
        ExecuteCharacterAnimation();
    }

    public void ExecuteCharacterAnimation()
    {
       
        // Remove the prefix of animation, like Fireball_Animation to Fireball
        string animationName = Utils.NoSufix(casterAnimationClip.name);
        // Play the animation by its name in caster animator
        skill.caster.GetComponent<Animator>().Play(animationName);
        // Wait characters animation time to executes the skill animation
        Invoke("ExecuteSkillAnimation", skillStartTime);
    }

    public void ExecuteSkillAnimation()
    {
        // Skill animation starts: show prefab mesh
        SwitchMeshRender(true);
        // Enable update to follow target
        isProjecting = true;
    }

    // --------- - Custom functions of this skill - ---------

    // Show/Hide mesh of this prefab
    void SwitchMeshRender(bool enable)
    {
        GetComponent<Renderer>().enabled = enable;
        GetComponent<Collider>().enabled = enable;
    }

    // Verify if this prefab collided with the target to apply his effect
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform == skill.target)
        {
            if (collider.GetComponent<CharacterCombat>() != null)
                collider.GetComponent<CharacterCombat>().TakeDamageFrom(skill.caster, skill.GetDamage());
            Destroy(gameObject);
        }
    }

}
