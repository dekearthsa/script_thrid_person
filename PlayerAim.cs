using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Player player; 
    private PlayerControl controls; 
    [Header("Aim info")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;
    private Vector2 aimInput;
        
     void Start()
    {
        player = GetComponent<Player>();
        PlayerAimAssgin();
        
    }

    void Update()
    {
        aim.position = new Vector3(GetMousePosition().x, aim.position.y, GetMousePosition().z);
    }

    public Vector3 GetMousePosition() {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask)){
            return hitInfo.point;
        }

        return Vector3.zero;

    }

    private void PlayerAimAssgin(){
        controls = player.controls; 
        controls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;

    }
 
    
}
