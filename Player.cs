using UnityEngine;

public class Player : MonoBehaviour
{

    /// การใส่  {get; private set;}  จะทำให้ function ที่มาจาก class PlayerAim กลายเป็น read-only
    public PlayerControl controls {get; private set;}  
    public PlayerAim aim {get; private set;}  

    private void Awake()
    {
        controls = new PlayerControl();
        aim = GetComponent<PlayerAim>();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }


 
}
