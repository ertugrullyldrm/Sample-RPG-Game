using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    
    float velocityToRun = 0.9f;

    Animator animator;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {

        bool isRunning = rb.velocity.magnitude > velocityToRun;
        animator.SetBool("run", isRunning);

    }
}
