using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private WeaponVisaulControler weaponVisaulControler;
    private PlayerControllerWaepon playerControllerWaepon;



    private void Start()
    {
         
        weaponVisaulControler = GetComponentInParent<WeaponVisaulControler>();
        playerControllerWaepon = GetComponentInParent<PlayerControllerWaepon>();
    }

    public void ReloadIsOver()
    {
        // Debug.Log("ReloadIsOver");
        weaponVisaulControler.ReturnWeightToOne();
        playerControllerWaepon.CurrentWeapon().RefillBullet();
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
