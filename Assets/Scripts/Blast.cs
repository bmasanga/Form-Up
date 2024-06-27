using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
     [SerializeField] float damage = 10;
     [SerializeField] ParticleSystem hitEffect;

     void OnTriggerEnter2D(Collider2D other)
     {
          // if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
          // {
          //      Destroy(gameObject);
          // }    
          
          PlayHitEffect();

          Shield shield = other.GetComponent<Shield>();
          if (shield != null)
          {
               shield.TakeDamage(damage);
          }

          Destroy(gameObject);
     }

     void PlayHitEffect()
     {
          if(hitEffect != null)
          {
               ParticleSystem instance = Instantiate(hitEffect, transform.position, transform.rotation);
               Destroy(instance.gameObject, 2f);
          }
     }
}
