using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CharacterCombat
{

    public bool isFighting = false;

    EnemyMove enemyMove;
    CharacterSkills characterSkills;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        enemyMove = GetComponent<EnemyMove>();
        characterSkills = GetComponent<CharacterSkills>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( isFighting)
        {
            base.selectedSkill = GetAvailableSkill();
            HandleAttack();
        }

        HandleEnemySkillbar();

    }

    void HandleAttack()
    {

        if (base.selectedSkill == null)
            return;
        if (GetComponent<EnemyState>().state == EnemyState.State.Casting)
        {
            base.selectedSkill = null;
            return;
        }

        base.selectedSkill.caster = transform;
        base.selectedSkill.target = base.target;

        bool isTargetInCasterRange = base.selectedSkill.IsTargetInCasterRange();

        if (isTargetInCasterRange == false)
        {
            enemyMove.followSelectedTarget = true;
        }

        if (base.selectedSkill.CanCastSkill())
        {

            enemyMove.followSelectedTarget = false;
            Utils.LookAtYZ(transform, base.selectedSkill.target.position);
            base.selectedSkill.CastSkill();
            StartCoroutine(GetComponent<EnemyState>().SwitchStateForDuration(EnemyState.State.Casting, EnemyState.State.Idle, base.selectedSkill.castingTime));
            base.selectedSkill = null;

        }
    }

    Skill GetAvailableSkill()
    {
        foreach( Skill skill in characterSkills.skills)
        {
            if( skill.countdownElapsed <= 0)
            {
                return skill;
            }
        }
        return null;
    }

    void HandleEnemySkillbar()
    {
        foreach( Skill skill in characterSkills.skills)
        {
            if( skill.countdownElapsed > 0)
            {
                skill.countdownElapsed -= Time.deltaTime;
            }
        }
    }

}
