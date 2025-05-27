using UnityEngine;

public class Player : MonoBehaviour
{

    /// การใส่  {get; private set;}  จะทำให้ function ที่มาจาก class PlayerAim กลายเป็น read-only
    public PlayerControl controls {get; private set;}  
    public PlayerAim aim {get; private set;}  
    public PlayerMovement movement {get; private set;}
    public PlayerControllerWaepon weapon {get; private set;}

    private void Awake()
    {
        controls = new PlayerControl();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerControllerWaepon>();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }


 
}
