using System.Diagnostics;
 

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle,
}

[System.Serializable] // Make cl
public class Weapon
{
    public WeaponType weaponType;
    public int bulletInMagazine;
    public int magzineCapacity;
    public int totalReserveAmmo;
    public bool isReloadingAimation;


    public void RefillBullet()
    {
        // int totalReserveAmmo = magzineCapacity; // // adding bullet in magzine
        int bulletsToReload = magzineCapacity;
 

        if (bulletsToReload > totalReserveAmmo)
            bulletsToReload = totalReserveAmmo;

        totalReserveAmmo -= bulletsToReload;
        bulletInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;

    }

    public bool CanShoot()
    {
        if (isReloadingAimation)
            return false;

        return HaveEnoughBullet();
    }

    private bool HaveEnoughBullet()
    {
        // UnityEngine.Debug.Log(bulletInMagazine);
        if (bulletInMagazine > 0)
        {
            bulletInMagazine--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanReload()
    {
        if (bulletInMagazine == magzineCapacity) 
            return false;
 
        if (totalReserveAmmo > 0)
            return true;

        return false;
    }
}
