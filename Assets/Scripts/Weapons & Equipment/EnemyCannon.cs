
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCannon : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;

    private EnemyAgent enemyAgent;

    void Start()
    {
        enemyAgent = GetComponentInParent<EnemyAgent>();
        if (enemyAgent != null)
        {
            enemyAgent.OnPerformAttack.AddListener(Fire);
        }
    }

    void OnDestroy()
    {
        if (enemyAgent != null)
        {
            enemyAgent.OnPerformAttack.RemoveListener(Fire);
        }
    }
    
    public void Fire()
    {
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Rigidbody2D rigidBody = projectileInstance.GetComponent<Rigidbody2D>();
            //Debug.Log("currentHeat: " + currentHeat);
            if (rigidBody != null)
            {
                // Calculate the force vector
                Vector2 force = transform.up * projectileSpeed;

                // Apply the impulse force to the bullet's Rigidbody2D
                rigidBody.AddForce(force, ForceMode2D.Impulse);
            }
            
            Destroy(projectileInstance, projectileLifetime);
    }
    
    
}
