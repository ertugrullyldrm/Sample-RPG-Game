using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils
{
    public static Skill FindSkillByName( List<Skill> skillList, string name )
    {
        foreach (Skill skill in skillList)
        {
            if (skill.name == name)
                return skill;
        }
        return null;
    }

    public static void LookAtYZ( Transform transform, Vector3 lookAtPosition)
    {
        float originalX = transform.rotation.eulerAngles.x;
        transform.LookAt(lookAtPosition);
        transform.rotation = Quaternion.Euler(originalX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        //return transform.rotation;
    }

    public static string NoSufix(string text, string prefix = "_")
    {
        return text == null || text == "" ? "" : text.Split(prefix)[0];
    }

    // ----------- - ANIMATIONS - -----------
    public static bool ContainsParam(Animator animator, string parameterName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName) return true;
        }
        return false;
    }

    public static AnimationClip FindAnimation(Animator animator, string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }

    public static IEnumerator CheckAnimationCompleted(Animator animator, string animation, Action OnComplete)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animation) == false);
        if (OnComplete != null)
            OnComplete();
    }

}
