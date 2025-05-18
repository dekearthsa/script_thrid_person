using UnityEngine;

public class PlayerControllerWaepon : MonoBehaviour
{
    private Player player;
 
    void Start()
    {
        player = GetComponent<Player>();
        player.controls.Character.Fire.performed += ctx => Shoot();
    }
 
      
    private void Shoot(){
 
        GetComponentInChildren<Animator>().SetTrigger("Fire");
        
    }
}
