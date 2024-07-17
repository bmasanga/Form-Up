using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyAI2), typeof(EnemyAgent))]

public class State : MonoBehaviour
{
    
    public AIData aIData;
    protected EnemyAgentMover enemyAgentMover;
    protected EnemyAgentAnimations enemyAgentAnimations;
    protected EnemyAI2 enemyAI;
    protected EnemyAgent enemyAgent;

    public List<Transition> transitions = new List<Transition>();
    [SerializeField] protected ContextSolver movementDirectionSolver;
    [SerializeField] protected List<SteeringBehaviour> steeringBehaviours;

    protected Vector2 movementInput;
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    
    void Awake()
    {
        aIData = GetComponent<AIData>();
        enemyAgentMover = GetComponent<EnemyAgentMover>();
        enemyAgentAnimations = GetComponent<EnemyAgentAnimations>();
        enemyAI = GetComponent<EnemyAI2>();
        enemyAgent = GetComponent<EnemyAgent>();
    }

    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        foreach (Transition transition in transitions)
        {
            if (transition.condition.Test(aIData, enemyAgent))
            {
                transition.target.enabled = true;
                this.enabled = false;

                return;
            }
        }
    }

    [Serializable]
    public struct Transition
    {
        public Condition condition;
        public State target;
    }
}
