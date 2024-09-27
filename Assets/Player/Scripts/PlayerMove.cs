using UnityEngine;
using TMPro;

/// <summary>
/// Main script for third-person movement of the character in the game.
/// Make sure that the object that will receive this script (the player) 
/// has the Player tag and the Character Controller component.
/// </summary>
public class PlayerMove : MonoBehaviour
{
     
    public float gravity = Constants.gravity;
    public bool followSelectedTarget = false;
    public float targetOffsetDistance = 2f;

    Rigidbody rb;
    CharacterStatus characterStatus;
    CharacterSkills characterSkills;
    PlayerState playerState;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterStatus = GetComponent<CharacterStatus>();
        characterSkills = GetComponent<CharacterSkills>();
        playerState = GetComponent<PlayerState>();
    }


    // Update is only being used here to identify keys and trigger animations
    void FixedUpdate()
    {

        Vector3 moveDirection = Vector3.zero;

        // No target, no follow. Helps to prevent bugs
        if (PlayerInput.selectedTarget == null)
            followSelectedTarget = false;

        // Move toward a selected target if is enabled
        if ( followSelectedTarget == true )
            moveDirection = PlayerInput.selectedTarget.position;

        // Move toward the clicked position 
        if (PlayerInput.pointClicked != Vector3.zero )
        {
            followSelectedTarget = false;
            moveDirection = PlayerInput.pointClicked;
        }

        // Add move direction to character controller
        Move(moveDirection);
        
    }

    public void Move( Vector3 moveDirection )
    {

        if ( playerState.CanMove() == false )
            return;

        // Dont move if is in the limit of offset
        //if( PlayerInput.hoveredTarget == PlayerInput.selectedTarget) // maybe check if is null?
        //if (PlayerInput.selectedTarget != null && PlayerInput.selectedTarget.CompareTag("Enemy") )
        //{
            
        //    float offsetDistance = Vector3.Distance(transform.position, PlayerInput.selectedTarget.position);
        //    if (offsetDistance < characterStatus.range)
        //        return;
            
        //}

        // Check if is not moved. Return and set state
        if ( moveDirection == Vector3.zero)
        {
            playerState.Set(0); // Idle
            return;
        }
        else
        {
            playerState.Set(1); // Moving
        }


        // Look to destiny point
        Utils.LookAtYZ(transform, moveDirection);
        moveDirection = (moveDirection - transform.position).normalized;

        // Apply velocity to the axis
        moveDirection.x *= characterStatus.moveSpeed;
        moveDirection.z *= characterStatus.moveSpeed;

        // Update the velocity of the rigidbody
        Vector3 velocity = new Vector3( moveDirection.x, rb.velocity.y, moveDirection.z );
        rb.velocity = velocity;
    }


}
