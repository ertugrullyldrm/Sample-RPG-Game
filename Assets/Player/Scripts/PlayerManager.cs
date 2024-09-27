using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    CharacterStatus characterStatus;

    private void Start()
    {
        characterStatus = GetComponent<CharacterStatus>();
        characterStatus.StartRegen();
    }

}
