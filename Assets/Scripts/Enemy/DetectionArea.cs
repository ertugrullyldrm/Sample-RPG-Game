using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{

    EnemyCombat enemyCombat;

    void Start()
    {
        enemyCombat = transform.GetComponent<EnemyCombat>();
    }

    private void OnTriggerStay(Collider collider)
    {

        // Prevent bug. I dont know why what is exactly
        if (enemyCombat == null)
            return;

        if( collider.CompareTag("Player"))
        {
            enemyCombat.isFighting = true;
            enemyCombat.target = collider.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (enemyCombat == null)
            return;

        if ( other.CompareTag("Player"))
        {
            enemyCombat.isFighting = false;
            enemyCombat.target = null;
        }
    }
}
