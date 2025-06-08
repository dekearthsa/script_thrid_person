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

    public bool CanShoot()
    {
        return HaveEnoughBullet();
    }

    private bool HaveEnoughBullet()
    {
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
        if (totalReserveAmmo > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReloadBullets()
    {

    }
}
