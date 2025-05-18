using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private WeaponVisaulControler weaponVisaulControler;

    private void Start()
    {
        weaponVisaulControler = GetComponentInParent<WeaponVisaulControler>();
    }

    public void ReloadIsOver(){
        // Debug.Log("ReloadIsOver");
        weaponVisaulControler.ReturnWeightToOne();
    }

    public void ReturnRig(){
        weaponVisaulControler.ReturnWeightToOne();
        weaponVisaulControler.ReturnWieghtHandWeightIK();
    }

    public void WeaponGrabIsOver(){
        weaponVisaulControler.SetBusyGrabingWeapon(false);
    }

    // public void WeaponIsReloading(){
    //     weaponVisaulControler.ReturnBusyReloadingWeapon();
    // }
}
