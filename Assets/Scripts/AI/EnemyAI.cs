using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    [Header("Movement Settings")]

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;

    [SerializeField]
    private float attackDistance = 5f;

    

    [Header("Movement Speeds")]
    [SerializeField] float forwardThrustSpeed = 1.0f;
    [SerializeField] float backwardThrustSpeed = 1.0f;
    [SerializeField] float rightThrustSpeed = 1.0f;
    [SerializeField] float leftThrustSpeed = 1.0f;
    [SerializeField] float rotationSpeed = 200f;

    [Header("Movement Effects")]
    [SerializeField] ParticleSystem fowardThrustEffect;
    [SerializeField] ParticleSystem backwardThrustEffect1;
    [SerializeField] ParticleSystem backwardThrustEffect2;
    [SerializeField] ParticleSystem rightThrustEffect;
    [SerializeField] ParticleSystem leftThrustEffect;

    //Inputs sent from the Enemy AI to the Enemy controller
    // public UnityEvent<Vector2> OnMoveInput;
    // public UnityEvent<Vector2> OnLookInput;
    // public UnityEvent OnFireInput;
    // public UnityEvent OnFireCancel;

    Vector2 moveInput;

    Rigidbody2D rb2d;

    bool following = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    private void Update()
    {
        //Enemy AI movement based on Target availability
        if (aiData.currentTarget != null)
        {
            //Looking at the Target
            RotateTowardTarget();

            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        //Moving the Agent
        // OnMoveInput?.Invoke(moveInput);
    }

    private void FixedUpdate()
    {
        Thrust();
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
            Debug.Log("Stopping");
            moveInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                //Attack logic
                moveInput = Vector2.zero;
                //OnFireInput?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                //Chase logic
                moveInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                Debug.Log("Chasing - moveInput: " + moveInput);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }

        }

    }

    void RotateTowardTarget()
    {
        Vector3 direction = aiData.currentTarget.position - transform.position;
        direction.z = 0f;

        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Thrust()
    {
        if (moveInput.magnitude > 0)
        {
            Vector2 thrustDirection = transform.TransformDirection(moveInput).normalized;
            Vector2 thrustForce = Vector2.zero;
            //Debug.Log("isThrusting: " + playerAnimator.GetBool("isThrusting"));

            if (moveInput.y > 0)
            {
                thrustForce += Vector2.up * forwardThrustSpeed * moveInput.y;
                fowardThrustEffect.Play();
            }
            if (moveInput.y < 0)
            {
                thrustForce += Vector2.down * backwardThrustSpeed * Mathf.Abs(moveInput.y);
                backwardThrustEffect1.Play();
                backwardThrustEffect2.Play();
            }    
            if (moveInput.x < 0)
            {
                thrustForce += Vector2.left * leftThrustSpeed * Mathf.Abs(moveInput.x);
                leftThrustEffect.Play();
            }
            if (moveInput.x > 0)
            {
                thrustForce += Vector2.right * rightThrustSpeed * moveInput.x;
                rightThrustEffect.Play();
            }

            
            rb2d.AddForce(thrustDirection * thrustForce.magnitude, ForceMode2D.Force);
            
        }
        else
        {
            // Stop all thrust effects if no movement input
            fowardThrustEffect.Stop();
            backwardThrustEffect1.Stop();
            backwardThrustEffect2.Stop();
            leftThrustEffect.Stop();
            rightThrustEffect.Stop();
        }
    }
}