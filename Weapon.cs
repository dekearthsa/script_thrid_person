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
    public int ammo;
    public int maxAmmo;
}
