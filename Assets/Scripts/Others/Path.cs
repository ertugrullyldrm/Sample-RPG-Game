using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    public static Transform skillPool;

    private void Awake()
    {
        skillPool = GameObject.Find("SkillPool").transform;
    }
}
