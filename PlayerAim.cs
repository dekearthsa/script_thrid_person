using System;
using UnityEngine;
using Cinemachine;

public class PlayerAim : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCamera; // กล้องเสมือน
    [SerializeField] public Transform aimObject; // ตัวที่อยากให้ตามตอนกดเมาส์
    [SerializeField] public Transform playerObject; // ตัวที่ให้ตามตอนปล่อยเมาส์
    private Player player; 
    private PlayerControl controls; 
    [Header("Aim control")]
    [SerializeField] Transform aim;
    [SerializeField] private bool isAimingPrecisly;

    [Header("Camera control")]
    
    [Range(1f, 5f)]
    [SerializeField] private float minCameraDistance = 5f;
    [Range(2.5f, 5f)]
    [SerializeField] private float maxCameraDistance =5f ;
    [SerializeField] private float cameraSensetivity = 5f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private LayerMask aimLayerMask;
    private RaycastHit lastKnowMouseHit;
 
    private Vector2 aimInput;
        
     void Start()
    {
        player = GetComponent<Player>();
        PlayerAimAssgin();
        
    }

    void Update()
    {
        aim.position = GetMouseHitInfo().point;
        ChangeAimMode();
        if(isAimingPrecisly){
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        }        
    }

    private void ChangeAimMode(){
        if(Input.GetMouseButton(1)){
            isAimingPrecisly = false;
            virtualCamera.Follow = aimObject;
           UpdateCameraPostion();
            
        }else{
            isAimingPrecisly = true;
            virtualCamera.Follow = playerObject;
            UpdateCameraPostion();
            // aim.position = new Vector3(GetMouseHitInfo().x, aim.position.y, GetMouseHitInfo().z);
        }
    }

    private void UpdateCameraPostion(){
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPostion(), cameraSensetivity * Time.deltaTime);
    }

    public bool CanAimPrecisly(){
        if(isAimingPrecisly){
            return true;
        }
        return false;
    }

    private Vector3 DesieredCameraPostion(){

        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f? minCameraDistance: maxCameraDistance;
        
        Vector3  desieredCameraPostion = GetMouseHitInfo().point;
        Vector3 aimDirection = (desieredCameraPostion - transform.position).normalized;
        //  Debug.Log("desieredCameraPostion => " + desieredCameraPostion);
        //  Debug.Log("aimDirection => " + aimDirection);
        //  Debug.Log("transform.position => " + transform.position);
        float distainceToDesierdPosition = Vector3.Distance(transform.position, desieredCameraPostion);

        float clampedDistance = Mathf.Clamp(distainceToDesierdPosition, minCameraDistance, actualMaxCameraDistance);
        desieredCameraPostion = transform.position + aimDirection * clampedDistance;
         // transform.position ตำแหน่งปัจจุบันของ Object (ในที่นี้คือ Player), aimDirection ค่าของตำแหน่งเมาส์ลบด้วยตำแหน่งของ object ตัวละคร 
        //  if (distainceToDesierdPosition > maxCameraDistance ){
        //     desieredCameraPostion = transform.position + aimDirection * maxCameraDistance;
        //  }else if(distainceToDesierdPosition < minCameraDistance){
        //     desieredCameraPostion = transform.position + aimDirection * minCameraDistance;
        //  }
         
        desieredCameraPostion.y = transform.position.y + 1;
        return desieredCameraPostion;
    }

    public RaycastHit GetMouseHitInfo() {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask)){
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnowMouseHit;

    }
    private void PlayerAimAssgin(){
        controls = player.controls; 
        controls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;

    }

}
