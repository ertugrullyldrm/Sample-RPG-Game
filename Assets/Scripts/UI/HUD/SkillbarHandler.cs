using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SkillbarHandler : MonoBehaviour
{

    Transform hud;
    Transform player;
    Transform skillbar;

    CharacterStatus characterStatus;
    List<Skill> skills;

    const int skillbarSize = 9;
    const string skillbarSlotName = "SkillSlot_";
    float skillbarSlotSize = 65;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        hud = GameObject.FindWithTag("UI").transform.Find("HUD").transform;
        skillbar = hud.Find("Skillbar");
        skillbarSlotSize = skillbar.GetChild(0).GetComponent<RectTransform>().sizeDelta.magnitude;

        characterStatus = player.GetComponent<CharacterStatus>();
        skills = player.GetComponent<CharacterSkills>().skills;

        BuildSkillIcons();

    }

    private void FixedUpdate()
    {
        // Decides if is a better way call here or inside de "CastSkill" on combat script
        UpdateSkillbar();
    }

    public void UpdateSkillbar()
    {

        for (int i = 0; i < skills.Count; i++)
        {

            Skill skill = skills[i];

            if (skill.countdownElapsed <= 0)
                continue;
            
            skill.countdownElapsed -= Time.deltaTime;

            // Add a HUD script
            Transform slot = skillbar.GetChild(i);
            Color color = slot.Find("IconImage").GetComponent<RawImage>().color;
            if (skill.countdownElapsed > 0)
            {
                color.a = 0.3f;
                slot.Find("IconImage").GetComponent<RawImage>().color = color;
                slot.Find("TextTimer").GetComponent<TextMeshProUGUI>().text = skill.countdownElapsed.ToString("0.0");
                slot.Find("TextTimer").GetComponent<TextMeshProUGUI>().enabled = true;
                UpdateCountdownOverride( skill, i );
            }
            else
            {
                color.a = 1;
                slot.Find("IconImage").GetComponent<RawImage>().color = color;
                slot.Find("TextTimer").GetComponent<TextMeshProUGUI>().enabled = false;
                ResetCountdownOverride(i);
            }


            
        }
    }

    void BuildSkillIcons()
    {

        for (int i = 0; i < skillbarSize; i++)
        {

            if (i > skills.Count - 1)
                return;
            if (skills[i].icon == null)
                continue;

            var iconImage = skillbar.Find(skillbarSlotName + i).Find("IconImage").GetComponent<UnityEngine.UI.RawImage>();
            iconImage.texture = skills[i].icon;
            iconImage.enabled = true;

        }
    }

    void UpdateCountdownOverride( Skill skill, int slot )
    {
        float sizePercent = skill.countdownElapsed / skill.countdown;
        var countdownOverride = skillbar.Find(skillbarSlotName + slot).Find("CountdownOverride").GetComponent<UnityEngine.UI.Image>();
        countdownOverride.enabled = true;
        Vector2 newSize = countdownOverride.GetComponent<RectTransform>().sizeDelta;
        newSize.y = skillbarSlotSize * sizePercent - skillbarSlotSize;
        countdownOverride.GetComponent<RectTransform>().sizeDelta = newSize;
    }

    void ResetCountdownOverride( int slot )
    {
        var countdownOverride = skillbar.Find(skillbarSlotName + slot).Find("CountdownOverride").GetComponent<UnityEngine.UI.Image>();
        countdownOverride.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        countdownOverride.enabled = false;
    }

}
