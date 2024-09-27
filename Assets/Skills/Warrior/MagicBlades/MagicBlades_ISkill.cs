
using UnityEngine;

public class MagicBlades_ISkill : MonoBehaviour, ISkill
{

    // Skill interface refering to original casted skill
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    public float skillStartTime = 1f;

    private void Start()
    {

        // Check skill for prevent bugs
        if ( SkillUtilities.isSkillNull(skill) == false)
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
        skill.caster.GetComponent<Animator>().Play("MagicBlades");
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

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy") && collider.GetComponent<CharacterCombat>() != null)
            collider.GetComponent<CharacterCombat>().TakeDamageFrom(skill.caster, skill.GetDamage());
    }

}
