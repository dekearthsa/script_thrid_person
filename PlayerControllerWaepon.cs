using Unity.Mathematics;
using UnityEngine;

public class PlayerControllerWaepon : MonoBehaviour
{
    private Player player;
    private PlayerMovement playerMovement;
     [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;
    
    [SerializeField] private Transform weaponHolder;
    // [SerializeField] private Transform aim;
    // [SerializeField] private 
 
    void Start()
    {
        player = GetComponent<Player>();
        playerMovement = GetComponentInChildren<PlayerMovement>();
        player.controls.Character.Fire.performed += ctx => Shoot();
    }
 
      
    private void Shoot(){
        if(playerMovement.nowCombatMode){
            GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
            newBullet.GetComponent<Rigidbody>().linearVelocity = BulletDirection() * bulletSpeed;
            Destroy(newBullet,5);
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

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(gunPoint.position, gunPoint.position +BulletDirection()* 25);
    // }
}
