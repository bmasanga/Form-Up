using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] float targetLockTime = 2.0f;
    [SerializeField] float lockDurationAfterExit = 5.0f; // Time to keep the lock after the target leaves the collider
    [SerializeField] GameObject lockedTarget = null;

    Dictionary<GameObject, float> potentialTargets = new Dictionary<GameObject, float>();
    SpriteRenderer radar;
    float timeSinceExit = 0f;
    bool targetExited = false;

    void Awake()
    {
        radar = GetComponent<SpriteRenderer>();
        radar.color = Color.green;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (lockedTarget != null)
        {
            return; // Ignore new entries if we already have a locked target
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (!potentialTargets.ContainsKey(other.gameObject))
            {
                potentialTargets.Add(other.gameObject, 0f);
                radar.color = Color.yellow;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
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
            radar.color = Color.green; // Ensure color is green when no targets are within the collider
        }
    }

    void Update()
    {
        if (lockedTarget == null)
        {
            List<GameObject> keys = new List<GameObject>(potentialTargets.Keys);

            foreach (GameObject target in keys)
            {
                potentialTargets[target] += Time.deltaTime;

                if (potentialTargets[target] >= targetLockTime)
                {
                    lockedTarget = target;
                    radar.color = Color.red;
                    Debug.Log("Target locked: " + lockedTarget.name);
                    break;
                }
            }

            // Clean up targets that left the collider
            foreach (GameObject target in keys)
            {
                if (!target)
                {
                    potentialTargets.Remove(target);
                }
            }
            if (lockedTarget == null && potentialTargets.Count > 0)
            {
                radar.color = Color.yellow; // Change color to yellow if there are potential targets but none are locked
            }
        }
        else if (targetExited)
        {
            timeSinceExit += Time.deltaTime;
            if (timeSinceExit >= lockDurationAfterExit)
            {
                lockedTarget = null;
                radar.color = Color.green; // Change color to green when lock is reset
                targetExited = false;
            }
        }

    }

    public GameObject GetLockedTarget()
    {
        return lockedTarget;
    }

    public void ResetLockedTarget()
    {
        lockedTarget = null;
        radar.color = Color.green;
    }
}
