
using UnityEngine;

public class Charge_ISkill : MonoBehaviour, ISkill
{

    // Skill interface refering to original casted skill
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    public float skillStartTime = 1f;
    public float timeToCancel = 0.5f;

    Rigidbody rb;
    bool startCharge = false;
    Vector3 lastPosition = Vector3.zero;

    float timeToCancelElapsed = 0;

    private void Start()
    {

        // Check skill for prevent bugs
        if ( SkillUtilities.isSkillNull(skill) == false)
            return;
        SkillUtilities.ShowHUDCastMessage(skill);

        transform.position = skill.caster.position;
        transform.GetComponent<Collider>().bounds.Encapsulate(skill.caster.GetComponent<Collider>().bounds);

        // Initiate this skill configs
        ExecuteCharacterAnimation();

    }

    private void FixedUpdate()
    {
        if (startCharge)
        {

            if (Vector3.Distance(lastPosition, skill.caster.position) < 0.05f)
            {
                if (timeToCancelElapsed >= timeToCancel)
                {
                    HUD.SetMessageDebug("Tempo m�ximo parado alcan�ado. Habilidade cancelada.");
                    EndCharge();
                    return;
                }
                timeToCancelElapsed += Time.deltaTime;
            }

            lastPosition = skill.caster.position;

            if (Vector3.Distance(skill.caster.position, skill.target.position) < 2f)
            {
                SkillDamage();
                EndCharge();
                return;
            }

            rb = skill.caster.GetComponent<Rigidbody>();
            PlayerMove move = skill.caster.GetComponent<PlayerMove>();
            CharacterStatus characterStatus = skill.caster.GetComponent<CharacterStatus>();

            Vector3 moveDirection = skill.target.position;
            // Look to destiny point
            Utils.LookAtYZ(transform, moveDirection);
            moveDirection = (moveDirection - skill.caster.position).normalized;

            // Apply velocity to the axis
            moveDirection.x *= characterStatus.moveSpeed + skill.velocity;
            moveDirection.z *= characterStatus.moveSpeed + skill.velocity;

            // Update the velocity of the rigidbody
            Vector3 velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
            rb.velocity = velocity;
            //Vector3 casterPosition = skill.caster.position;
            Vector3 casterPosition = SkillUtilities.GetCasterCenterPosition(skill);
            transform.position = casterPosition;
        }
    }


    public void ExecuteCharacterAnimation()
    {
        // Play the animation by its name in caster animator
        skill.caster.GetComponent<Animator>().Play(skill.name);
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

        StartCharge();

    }

    void StartCharge()
    {
        startCharge = true;
        GetComponent<ParticleSystem>().Play();
    }

    void EndCharge()
    {
        startCharge = false;
        skill.caster.GetComponent<Animator>().Play("Idle");
        Destroy(gameObject);
    }

    void SkillDamage()
    {
        transform.position = SkillUtilities.GetTargetCenterPosition(skill);
        // Apply this skill effect
        if (skill.target.GetComponent<CharacterCombat>() != null)
            skill.target.GetComponent<CharacterCombat>().TakeDamageFrom(skill.caster, skill.GetDamage());
    }

}
