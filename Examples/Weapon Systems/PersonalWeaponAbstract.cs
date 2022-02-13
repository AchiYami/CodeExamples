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

///<summary>Base Class for all On-Foot Weapons</summary>
public abstract class PersonalWeaponAbstract : MonoBehaviour
{

    ///<summary>Display name for this weapon.</summary>
    public string WeaponName;

    ///<summary>Tracks if this weapon is ready to be used.</summary>
    public bool IsReady;

    ///<summary>Public property for grabbing the ammo display string</summary>
    public abstract string AmmoString
    {
        get;
    }

    ///<summary>The transform of the player's camera.</summary>
    protected Transform _firstPersonCamera;

    ///<summary>The effective range of the weapon.</summary>
    protected float _weaponRange;

    ///<summary>The layermask that determine what can be "hit" by this weapon.</summary>
    protected LayerMask _interactionMask;

    ///<summary>The magnitude of kinetic force an impact of this weapon's projectile produces.</summary>
    protected float _weaponForce;

    ///<summary>Timer used to detect whether this weapon is ready to fire.</summary>
    protected float _timer;

    ///<summary>The amount of time it takes for this weapon to become ready again after firing.</summary>
    protected float _timerInterval;


    ///<summary>Base method for all personal weapons to attack</summary>
    public abstract void Attack();

    ///<summary>
    ///Base method for all personal weapons to "cooldown". 
    ///For ranged weapons this is a reload, for melee weapons it is the time between swings.
    ///</summary>
    public virtual void Reload()
    {
        IsReady = false;
        StartCoroutine(ReloadRoutine());
    }

    ///<summary>Base method for asynchronous coroutine based reloading of a weapon</summary>
    public abstract IEnumerator ReloadRoutine();

    ///<summary>Initializes the weapon with Player Data</summary>
    ///<param name-"fpwc">The First Person Weapon Controller</param>
    public virtual void Initialize(FirstPersonWeaponContainer fpwc)
    {
        _weaponRange = fpwc.distance;
        _interactionMask = fpwc.interactionMask;
        _firstPersonCamera = fpwc.FirstPersonCamera.transform;
    }

}
