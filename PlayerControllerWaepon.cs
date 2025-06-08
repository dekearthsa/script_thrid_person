using Unity.Mathematics;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering.LookDev;

public class PlayerControllerWaepon : MonoBehaviour
{
    private Player player;
    private PlayerMovement playerMovement;
    private float REF_BULLET_SPEED = 30f;
    [Header("Bullet detail")]
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;
    
    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private List<Weapon> weaponSlots;

    void Start()
    {
        player = GetComponent<Player>();
        playerMovement = GetComponentInChildren<PlayerMovement>();
        AssignInputEvent();

        currentWeapon.ammo = currentWeapon.maxAmmo;
    }

    private void AssignInputEvent()
    {
        PlayerControl controls = player.controls;
        controls.Character.Fire.performed += ctx => Shoot();
        controls.Character.Equipweapon1.performed += context => EquipWeapon(0);
        controls.Character.Equipweapon2.performed += context => EquipWeapon(1);
    }


    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];
    }
 
      
    private void Shoot()
    {
        if (playerMovement.nowCombatMode)
        {
            if (currentWeapon.ammo <= 0)
            {
                return;
            }
            currentWeapon.ammo--;
            GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

            Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

            rbNewBullet.mass = REF_BULLET_SPEED / bulletSpeed;
            rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;
        
            Destroy(newBullet, 3);
            GetComponentInChildren<Animator>().SetTrigger("Fire");
        }
    }

    public Vector3 BulletDirection(){
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - gunPoint.position).normalized;
        try{
            if(player.aim.CanAimPrecisly()){
             direction.y = 0;
            }
        
            weaponHolder.LookAt(aim);
            gunPoint.LookAt(weaponHolder);
            return direction;
        }catch{
            return direction;
        }
        
    }

    public Transform GunPoint() => gunPoint;

 
}