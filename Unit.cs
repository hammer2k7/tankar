using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    //public Transform destination;


    
    public ParticleSystem exhaustSmoke;
    private NavMeshAgent navAgent;
    private Rigidbody rb;
    private Vector3 curpos, lastpos;
    public GameObject selectionRing;
    private bool attackMode;
    private bool isMoving;
    private GameObject enemyUnit;
    public float rotationSpeed;
    public ParticleSystem cannonFire;
    private bool isFiring;
    public float firingRate;
    public float firingDelay;
    private NavMeshObstacle navObstacle;
    

    // Start is called before the first frame update

    void Start()
    {
        
        selectionRing.SetActive(false);
        
        rb = this.GetComponent<Rigidbody>();
        isFiring = false;
        attackMode = false;
        //cannonFire.GetComponent<SpriteRenderer>().enabled = false;
        isMoving = false;
        
        
    }

    private void Awake()
    {
        navObstacle = this.GetComponent<NavMeshObstacle>();
        navObstacle.enabled = true;
        navAgent = this.GetComponent<NavMeshAgent>();
        navAgent.enabled = false;
    }

    void FireAnimation()
    {
        cannonFire.Play();
    }



    // Update is called once per frame
    public void Move(Transform t)
    {
        
        
        navObstacle.enabled = false;
        StartCoroutine(WaitBeforeMove(0.2f, t));

/*
        navAgent.enabled = true;
        isMoving = true;
        Debug.Log("Moving Now");

        rb.isKinematic = false;
        attackMode = false;
        navAgent.SetDestination(t.transform.position);
        exhaustSmoke.Play();
        //CancelInvoke();
        if (isFiring)
        {
            //cannonFire.GetComponent<SpriteRenderer>().enabled = false;
            isFiring = false;
        }
  */      
    }

    public void Attack(Transform t, GameObject enemy)
    {
        navObstacle.enabled = false;
        
        StartCoroutine(WaitBeforeAttack(0.2f, t, enemy));

        /*

        Debug.Log("Attacking Now");

        navAgent.enabled = true;
        isMoving = true;

        rb.isKinematic = false;
        navAgent.SetDestination(t.transform.position);
        exhaustSmoke.Play();
        attackMode = true;
        enemyUnit = enemy;

        Debug.Log("Attack mode");

        */
    }

    private void Update()
    {
            if (attackMode)
            {
                Debug.Log("Distance" + Vector3.Distance(this.gameObject.transform.position, enemyUnit.transform.position));
                if (Vector3.Distance(this.gameObject.transform.position, enemyUnit.transform.position) < 15.0f)
                {
                    Debug.Log("Near target");
                    if (isMoving)
                    {
                        navAgent.SetDestination(this.gameObject.transform.position);

                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }



                    /*rb.isKinematic = true;
                

                    StartCoroutine(ApplyStoppingForce());
                    */
                    if (IsFacingObject(enemyUnit.transform) && !isFiring)
                    {
                        //cannonFire.GetComponent<SpriteRenderer>().enabled = true;
                        InvokeRepeating("FireAnimation", firingDelay, firingRate);
                        isFiring = true;
                    }
                    else
                    {
                        RotateTowards(enemyUnit.transform);
                    }

                }
            }


    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
         transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }


    private bool IsFacingObject(Transform target)
    {
        // Check if the gaze is looking at the front side of the object
        Vector3 forward = transform.forward;
        Vector3 toOther = (target.position - transform.position).normalized;

        //if (Vector3.Dot(forward, toOther) < 0.7f)
        if (Vector3.Dot(forward, toOther) < 0.95f)
        {
            //Debug.Log("Not facing the object");
            return false;
        }

        //Debug.Log("Facing the object");
        return true;
    }

    IEnumerator WaitBeforeMove(float waittime, Transform t)
    {
        yield return new WaitForSeconds(waittime);
        navAgent.enabled = true;
        isMoving = true;
        Debug.Log("Moving Now");
        
        rb.isKinematic = false;
        attackMode = false;
        navAgent.SetDestination(t.transform.position);
        exhaustSmoke.Play();
        CancelInvoke();
        if (isFiring)
        {
            //cannonFire.GetComponent<SpriteRenderer>().enabled = false;
            isFiring = false;
        }
    }

    IEnumerator WaitBeforeAttack(float waittime, Transform t, GameObject enemy)
    {
        yield return new WaitForSeconds(waittime);

        Debug.Log("Attacking Now");
        
        navAgent.enabled = true;
        isMoving = true;
        
        rb.isKinematic = false;
        navAgent.SetDestination(t.transform.position);
        exhaustSmoke.Play();
        attackMode = true;
        enemyUnit = enemy;

        Debug.Log("Attack mode");
        

    }

    IEnumerator WaitBeforeStop(float waittime)
    {
        yield return new WaitForSeconds(waittime);

        Debug.Log("Stopping Now");

        navObstacle.enabled = true;


    }


    /* deprecated 
    IEnumerator ApplyStoppingForce()
    {
        var oldDrag = rb.drag;
        var oldMass = rb.mass;

        rb.drag = 2000;
        rb.mass = 5;

        yield return new WaitForSeconds(10f);

        rb.drag = oldDrag;
        rb.mass = oldMass;
        Debug.Log("Applied stopping force");
    }

    */

    private void FixedUpdate()
    {
        if (isMoving)
        {
            curpos = rb.position;
            if (curpos == lastpos)
            {
                exhaustSmoke.Stop();

                isMoving = false;
                navAgent.enabled = false;


                StartCoroutine(WaitBeforeStop(0.2f));

                
                Debug.Log("stopped");

            }
            lastpos = rb.position;
        }
    }

    public void ActivateSelection()
    {
        selectionRing.SetActive(true);
    }

    public void DeactivateSelection()
    {
        selectionRing.SetActive(false);
    }



}
