using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float damage = 10;

     public float GetDamage()
     {
          return damage;
     }

     public void Hit()
     {
          Destroy(gameObject);
     }

     void OnTriggerEnter2D(Collider2D other)
     {
          if(other.gameObject.layer == LayerMask.NameToLayer("Environment"))
          {
               Destroy(gameObject);
          }    
     }
}
