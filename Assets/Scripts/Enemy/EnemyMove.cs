using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    public float gravity = Constants.gravity;
    public float targetOffsetDistance = 2f;

    public float minRestTime = 3f;
    public float maxRestTime = 6f;
    float restTime;


    bool isPatrolling = true;
    public bool followSelectedTarget = false;

    bool reachDestiny = true;
    bool moveToDestiny = false;

    Rigidbody rb;
    CharacterStatus characterStatus;
    EnemyAI enemyAI;
    EnemyCombat enemyCombat;
    Transform player;

    Vector3 destiny;
    Vector3 moveDirection;
    Bounds patrolAreaBounds;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterStatus = GetComponent<CharacterStatus>();
        enemyAI = GetComponent<EnemyAI>();
        enemyCombat = GetComponent<EnemyCombat>();
        player = GameObject.FindWithTag("Player").transform;

        patrolAreaBounds = transform.Find("PatrolArea").GetComponent<Renderer>().bounds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        moveDirection = Vector3.zero;

        if (enemyCombat.isFighting)
        {
            if (followSelectedTarget)
            {
                Move(enemyCombat.target.position);
            }

            return;

        }

        if( isPatrolling )
        {
            Patrol();
        }

    }


    void Patrol()
    {

        if (reachDestiny == true)
        {
            reachDestiny = false;
            Move(Vector3.zero); // stop character controller velocity

            restTime = Random.Range(minRestTime, maxRestTime);

            destiny = GetNewDestiny();
            Invoke("EnableMoveToDestiny", restTime);
        }

        if (moveToDestiny == true)
        {
            Move(destiny);
        }

        if (Vector3.Distance(transform.position, destiny) < 0.9f)
        {
            moveToDestiny = false;
            reachDestiny = true;
        }

    }

    void Move(Vector3 moveDirection)
    {

        if (CanRun() == false)
            return;

        if(enemyCombat.target != null)
        {
            float offsetDistance = Vector3.Distance(transform.position, enemyCombat.target.position);
            if (offsetDistance < characterStatus.range)
                return;
        }

        if (moveDirection != Vector3.zero)
        {
            Utils.LookAtYZ(transform, moveDirection);
            moveDirection = (moveDirection - transform.position).normalized;
        }

        // Apply velocity to the axis
        moveDirection.x *= characterStatus.moveSpeed;
        moveDirection.z *= characterStatus.moveSpeed;

        // Update the velocity of the rigidbody
        Vector3 velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        rb.velocity = velocity;
    }

    void EnableMoveToDestiny()
    {
        moveToDestiny = true;
    }

    Vector3 GetNewDestiny()
    {
        Vector3 randomPointInPatrolArea = new Vector3(
                Random.Range(patrolAreaBounds.min.x, patrolAreaBounds.max.x),
                patrolAreaBounds.center.y, // keep original height
                Random.Range(patrolAreaBounds.min.z, patrolAreaBounds.max.z)
            );
        return randomPointInPatrolArea;
    }

    private void OnCollisionEnter(Collision collision)
    {

        // Reset destiny path if hit in something
        if ( isPatrolling )
        {
            // Ao bater um obstáculo ele cria uma nova rota, mas a nova rota pode continuar
            // sendo em um ponto que ele não alcança. Precisava rever isso aqui
            //destiny = GetNewDestiny();
            reachDestiny = true;
        }
    }

    bool CanRun()
    {
        return enemyCombat.isCasting == false;
    }
    
}
