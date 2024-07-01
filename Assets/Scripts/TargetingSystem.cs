using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] float targetLockTime = 2.0f;
    [SerializeField] float lockDurationAfterExit = 5.0f; // Time to keep the lock after the target leaves the collider
    [SerializeField] GameObject lockedTarget = null;

    Dictionary<GameObject, float> potentialTargets = new Dictionary<GameObject, float>();
    TargetingLaser targetingLaser;
    Agent agent;
    
    float timeSinceExit = 0f;
    bool targetExited = false;

    void Awake()
    {
        targetingLaser = GetComponentInChildren<TargetingLaser>();
        targetingLaser.SetLaserColor(Color.green);
        agent = GetComponentInParent<Agent>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!agent.GetIsTargeting()) return; // Ignore if not targeting
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (lockedTarget == null)
            {
                // Increment lock time for this target
                if (potentialTargets.ContainsKey(other.gameObject))
                {
                    potentialTargets[other.gameObject] += Time.deltaTime;
                }
                else
                {
                    potentialTargets[other.gameObject] = 0f;
                }

                // Check if target lock time is reached
                if (potentialTargets[other.gameObject] >= targetLockTime)
                {
                    lockedTarget = other.gameObject;
                    targetingLaser.SetLaserColor(Color.red);
                    Debug.Log("Target locked: " + lockedTarget.name);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!agent.GetIsTargeting()) return; // Ignore if not targeting
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (potentialTargets.ContainsKey(other.gameObject))
            {
                potentialTargets.Remove(other.gameObject);
            }

            if (lockedTarget == other.gameObject)
            {
                targetExited = true;
                timeSinceExit = 0f; // Reset the exit timer
            }
        }
        if (potentialTargets.Count == 0 && lockedTarget == null)
        {
            ResetLockedTarget();
        }
    }

    void Update()
    {
        if (!agent.GetIsTargeting())
        {
            potentialTargets.Clear();
            ResetLockedTarget();
            targetingLaser.SetLaserEnabled(false);
            return;

        }

        targetingLaser.SetLaserEnabled(true);

        // Handle target exiting logic
        if (lockedTarget != null && targetExited)
        {
            timeSinceExit += Time.deltaTime;
            if (timeSinceExit >= lockDurationAfterExit)
            {
                ResetLockedTarget();
                targetExited = false;
            }
        }

        // Clean up destroyed targets
        List<GameObject> keys = new List<GameObject>(potentialTargets.Keys);
        foreach (GameObject target in keys)
        {
            if (target == null)
            {
                potentialTargets.Remove(target);
            }
        }

        // Change color to yellow if there are potential targets but none are locked
        if (lockedTarget == null && potentialTargets.Count > 0)
        {
            targetingLaser.SetLaserColor(Color.yellow);
        }
    }

    public GameObject GetLockedTarget()
    {
        return lockedTarget;
    }

    public void ResetLockedTarget()
    {
        lockedTarget = null;
        targetingLaser.SetLaserColor(Color.green);
        //Debug.Log("Resetting");
    }
}
