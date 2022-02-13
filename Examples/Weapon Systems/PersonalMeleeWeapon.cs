using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/**
Author : Simon Campbell
Category: Player - Items/Inventory - Weapons - Usable

Notes: None

Changelog:  13/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/


///<summary>Concrete clas for all on foot melee weapons</summary>
public class PersonalMeleeWeapon : PersonalWeaponAbstract
{

    ///<summary>This weapon's stats.</summary>
    private PersonalMeleeWeaponData _weaponData;

    public override string AmmoString
    {
        get { return "∞"; }
    }


    ///<summary>Initializes the weapon with the appropriate inventory data.</summary>
    ///<param name="weapon">The inventory data to use with this weapon.</param>
    public void SetData(PersonalMeleeWeaponData weapon)
    {
        _weaponData = weapon;
        _weaponForce = weapon.MaxDamage * 1000;
        _timerInterval = weapon.FireRate;

        WeaponName = weapon.ItemName;
    }

    ///<summary>Method with the behaviour to perform a single firing of this weapon.</summary>
    public override void Attack()
    {
        //Prepare the weapon to fire.
        if (_timer >= 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        //Is the weapon prepared to fire, are we pressing the button? 
        if (_timer < 0 && Mouse.current.leftButton.isPressed)
        {
            //Reset the timer
            _timer = _timerInterval;

            //Have we hit anything that is eligible?
            if (Physics.Raycast(_firstPersonCamera.transform.position, _firstPersonCamera.transform.forward, out RaycastHit hit, _weaponRange, _interactionMask))
            {
                if (hit.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    //Calculate direction of impact force and apply it to the object.
                    Vector3 direction = hit.point - DirectivePlayer.Instance.PlayerCharacter.transform.position;
                    rb.AddForceAtPosition(direction.normalized * _weaponForce, hit.point);
                }

                //If this object can be dealt damage, do it.
                if (hit.transform.TryGetComponent<DestructibleEntity>(out DestructibleEntity dE))
                {
                    dE.DealDamage(50);//Current hardwired, to be adjusted during tuning.
                }
            }
        }
    }

    ///<summary>
    ///Reload method kept, in case of future melee weapons that need reloaded, i.e
    ///any sort of fueled melee weapon, think chainsaw.
    ///</summary>
    public override IEnumerator ReloadRoutine()
    {
        yield return null;
    }
}
