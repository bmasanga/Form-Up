using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private IWeapon weapon;
    Agent agent;

    private void Awake()
    {
        weapon = GetComponent<IWeapon>();

        if (weapon == null)
        {
            Debug.LogError("No weapon found on player.");
        }

        agent = GetComponentInParent<Agent>();
        if (agent == null)
        {
            Debug.LogError("Agent component not found on parent.");
        }
    }

    private void FixedUpdate()
    {
        if (agent.GetIsFiring() && weapon != null)
        {
            weapon.Fire();
            Debug.Log("Weapon Controller firing!");
        }    
    }
}
