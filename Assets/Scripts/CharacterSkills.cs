using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSkills : MonoBehaviour
{

    public List<Skill> skills = new List<Skill>();


    private void Awake()
    {
        // Clona todas as habilidades na lista para não serem sobrescritas
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i] = Instantiate(skills[i]);
        }
    }

}
