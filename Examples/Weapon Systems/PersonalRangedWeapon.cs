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


///<summary>Concrete class for all on foot ranged weapons.</summary>
public class PersonalRangedWeapon : PersonalWeaponAbstract
{
 
    ///<summary>This weapon's stats and firing information.</summary>
    private PersonalRangedWeaponData _weaponData;

    ///<summary>The amount of ammo currently loaded into the weapon.</summary>
    public int CurrentCapacity;

    ///<summary>The maximu amount of ammo that can be loaded into this weapon at once.</summary>
    public int MaximumCapacity;

    ///<summary>Grabs the appropriate display string for the ammo.<summary>
    public override string AmmoString
    {
        get
        {
            return $"{CurrentCapacity} / {MaximumCapacity}";
        }
    }

    ///<summary>Initializes the weapon with Player Data</summary>
    ///<param name-"fpwc">The First Person Weapon Controller</param>
    public override void Initialize(FirstPersonWeaponContainer fpwc)
    {
        _weaponRange = 1000; //Currently hard-wired. To be adjusted with weapon range.
        _interactionMask = fpwc.interactionMask;
        _firstPersonCamera = fpwc.FirstPersonCamera.transform;
    }

    ///<summary>Initializes the weapon with the appropriate inventory data.</summary>
    ///<param name="weapon">The inventory data to use with this weapon.</param>
    public void SetData(PersonalRangedWeaponData weapon)
    {
        _weaponData = weapon;
        _weaponForce = weapon.MaxDamage * 1000; //Currently hard-wired. To be adjusted during tuning.
        _timerInterval = weapon.FireRate;

        MaximumCapacity = weapon.MaxCapacity;
        CurrentCapacity = weapon.MaxCapacity;
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

        //Determine how this weapon fires.
        switch (_weaponData.FireMode)
        {
            case PersonalRangedWeaponData.FiringMode.SemiAutomatic: FireSemiAutomatic(); break;
            case PersonalRangedWeaponData.FiringMode.BurstFire: FireBurst(); break;
            case PersonalRangedWeaponData.FiringMode.Automatic: FireAutomatic(); break;
        }

    }

    ///<summary>Behaviour for a weapon that fires a single shot each time the button is pressed.</summary>
    void FireSemiAutomatic()
    {
        //Are we pressing the appropriate button, is the weapon ready to fire and has ammo?
        if (_timer < 0 && Mouse.current.leftButton.wasPressedThisFrame && CurrentCapacity > 0)
        {
            //Decrease ammo, reset the timer.
            CurrentCapacity--;
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

                //If  this object can be dealt damage, do it.
                if (hit.transform.TryGetComponent<DestructibleEntity>(out DestructibleEntity dE))
                {
                    dE.DealDamage(50);//Damage count currently hardwired, to be adjusted during tuning.
                }
            }
        }

        //If we've run out of ammo, reload.
        if (CurrentCapacity == 0)
        {
            Reload();
        }
    }

    ///<summary>Behaviour for a weapon that fires in a burst of 3 shots each time the button is pressed.</summary>
    void FireBurst()
    {
        //Is the weapon prepared to fire, are we pressing the button, and does it have enough ammo? 
        if (_timer < 0 && Mouse.current.leftButton.wasPressedThisFrame)
        {
            //If we have less ammo than the burst count, we reload.
            if (CurrentCapacity < 3)
            {
                Reload();
            }
            else
            {
                //Reset weapon timer.
                _timer = _timerInterval;

                //Fire in a burst of three, this should be updated to allow for a delay between shots.
                for (int i = 0; i < 3; i++)
                {
                    //Have we hit an object thats eligible?
                    if (Physics.Raycast(_firstPersonCamera.transform.position, _firstPersonCamera.transform.forward, out RaycastHit hit, _weaponRange, _interactionMask))
                    {

                        if (hit.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
                        {
                            //Calculate direction of impact force and apply it to the object.
                            Vector3 direction = hit.point - DirectivePlayer.Instance.PlayerCharacter.transform.position;
                            rb.AddForceAtPosition(direction.normalized * _weaponForce, hit.point);
                        }

                        //If  this object can be dealt damage, do it.
                        if (hit.transform.TryGetComponent<DestructibleEntity>(out DestructibleEntity dE))
                        {
                            dE.DealDamage(50);//Damage count currently hardwired, to be adjusted during tuning.
                        }
                    }
                }
            }
        }
    }

    ///<summary>Behaviour for a weapon that fires continously while  the button is held down.</summary>
    void FireAutomatic()
    {
        //Is the weapon prepared to fire, are we pressing the button, and does it have enough ammo? 
        if (_timer < 0 && Mouse.current.leftButton.isPressed && CurrentCapacity > 0)
        {
            //Decrease ammo count, and reset the timer.
            CurrentCapacity--;
            _timer = _timerInterval;

            //Have we hit an object thats eligible?
            if (Physics.Raycast(_firstPersonCamera.transform.position, _firstPersonCamera.transform.forward, out RaycastHit hit, _weaponRange, _interactionMask))
            {
                if (hit.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    //Calculate direction of impact force and apply it to the object.
                    Vector3 direction = hit.point - DirectivePlayer.Instance.PlayerCharacter.transform.position;
                    rb.AddForceAtPosition(direction.normalized * _weaponForce, hit.point);
                }

                //If  this object can be dealt damage, do it.
                if (hit.transform.TryGetComponent<DestructibleEntity>(out DestructibleEntity dE))
                {
                    dE.DealDamage(50);//Damage count currently hardwired, to be adjusted during tuning.
                }
            }
        }

        //Reload if we are out of ammo.
        if (CurrentCapacity == 0)
        {
            Reload();
        }
    }

    ///<summary>Asynchronous coroutine to reload the weapon.
    public override IEnumerator ReloadRoutine()
    {
        float reloadTimer = 0;

        while (reloadTimer < _weaponData.ReloadTime)
        {
            reloadTimer += Time.deltaTime;
            yield return null;
        }

        CurrentCapacity = MaximumCapacity;
        IsReady = true;
    }
}
