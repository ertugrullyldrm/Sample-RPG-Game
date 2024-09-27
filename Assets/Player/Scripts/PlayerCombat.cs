using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : CharacterCombat
{

    PlayerMove playerMove;
    CharacterSkills characterSkills;
    List<Skill> skillsList;
    List<Skill> passiveSkills;

    void Start()
    {
        base.Start();
        playerMove = GetComponent<PlayerMove>();
        characterSkills = GetComponent<CharacterSkills>();
        
        skillsList = characterSkills.skills;
        foreach (Skill skill in skillsList)
            if( skill.isPassive)
            {
                skill.caster = transform;
                skill.target = transform;
                passiveSkills.Add(skill);
            }

    }

    void Update()
    {
        
        // Get keys to define if is a casting
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                if (skillsList.Count < i || Input.GetKeyDown(KeyCode.Alpha0) )
                    return;

                int input = i - 1;


                if (skillsList[input].isPassive)
                    continue;

                base.selectedSkill = skillsList[input];
            }
        }

        //ActivatePassiveSkills();

        if (PlayerInput.selectedTarget == null || PlayerInput.selectedTarget.CompareTag("Enemy") == false)
            return;

        HandleAttack();
        
    }

    void HandleAttack()
    {

        if (base.selectedSkill == null)
            return;
        if (GetComponent<PlayerState>().state == PlayerState.State.Casting)
        {
            HUD.SetMessageDebug("Você ainda está castando uma skill, aguarde...");
            base.selectedSkill = null;
            return;
        }

        base.selectedSkill.caster = transform;
        base.selectedSkill.target = PlayerInput.selectedTarget;

        base.target = PlayerInput.selectedTarget;

        bool isTargetInCasterRange = base.selectedSkill.IsTargetInCasterRange();
        //bool isSkillReady = combatHandler.selectedSkill.IsSkillReady();

        if (isTargetInCasterRange == false)
        {
            playerMove.followSelectedTarget = true;
        }

        if (base.selectedSkill.CanCastSkill())
        {
            // Para de seguir o inimigo
            playerMove.followSelectedTarget = false;
            // Vira o jogador em direção ao alvo
            Utils.LookAtYZ(transform, base.selectedSkill.target.position);
            // Inicia o trigger da skill
            base.selectedSkill.CastSkill();
            // Alterna os estados de casting para que o jogador pare de se mover
            StartCoroutine(GetComponent<PlayerState>().SwitchStateForDuration(PlayerState.State.Casting, PlayerState.State.Idle, base.selectedSkill.castingTime));
            // Limpa a skill do cache
            base.selectedSkill = null;
        }
    }

    public void EventCallback(string callback)
    {
        if(callback == "EnemyIsDead")
        {
            // Filter by class, etc...
            foreach (Skill skill in passiveSkills)
            {
                if (skill.name == "Ivigorating Booty" && skill.CanCastSkill() )
                    skill.CastSkill();
            }
        }
    }

}
