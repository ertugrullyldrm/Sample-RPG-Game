using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillUtilities
{

    public static bool isSkillNull(Skill skill)
    {
        if (skill == null)
        {
            HUD.SetMessageDebug("-- Missing skill reference");
            return false;
        }
        return true;
    }

    public static float GetCasterCenterY(Skill skill)
    {
        if (skill.caster.GetComponent<Collider>() == null)
            return skill.caster.position.y;
        return skill.caster.GetComponent<Collider>().bounds.size.y / 2f;
    }
    public static float GetTargetCenterY(Skill skill)
    {
        if (skill.caster == null)
            return 0;
        if (skill.caster.GetComponent<Collider>() == null)
            return skill.caster.position.y;
        return skill.caster.GetComponent<Collider>().bounds.size.y / 2f;
    }

    public static Vector3 GetCasterCenterPosition(Skill skill)
    {
        Vector3 newPosition = skill.caster.position;
        newPosition.y = GetCasterCenterY(skill);
        return newPosition;
    }
    public static Vector3 GetCasterCenterPosition(Skill skill, Vector3 offset)
    {
        return GetCasterCenterPosition(skill) + offset;
    }

    public static Vector3 GetTargetCenterPosition(Skill skill)
    {
        Vector3 newPosition = skill.target.position;
        newPosition.y = GetTargetCenterY(skill);
        return newPosition;
    }
    public static Vector3 GetTargetCenterPosition(Skill skill, Vector3 offset)
    {
        return GetCasterCenterPosition(skill) + offset;
    }

    public static void ShowHUDCastMessage(Skill skill)
    {
        if (skill.caster.CompareTag("Player"))
            HUD.SetMessageDebug($"-- Casting skill [{skill.name}] on target [{skill.target.name}]");
    }

    public static void DefaultEndExecution(Skill skill)
    {
        if (skill.caster.CompareTag("Player"))
        {
            skill.caster.GetComponent<PlayerState>().Set(0);
        } 
    }

}
