using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;


public class PlayerAim : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCamera; // กล้องเสมือน
    private float cameraYRotationFloat;
    [SerializeField] public Transform aimObject; // ตัวที่อยากให้ตามตอนกดเมาส์
    [SerializeField] public Transform playerObject; // ตัวที่ให้ตามตอนปล่อยเมาส์
    private Player player; 
    private PlayerControl controls; 
    [Header("Aim Viusal - Laser")]
    [SerializeField] private LineRenderer aimLaser;
    // [SerializeField] private float tipLenght = 5f;
    // private bool isUpdateTipLength =false;
    // private float lastestTipLength;

    [Header("Aim control")]
    [SerializeField] Transform aim;
    [SerializeField] private bool isAimingPrecisly;
    //  private LineRenderer lineRenderer;

    [Header("Camera control")]
    
    [Range(1f, 5f)]
    [SerializeField] private float minCameraDistance = 5f;
    [Range(2.5f, 5f)]
    [SerializeField] private float maxCameraDistance =5f ;
    [SerializeField] private float cameraSensetivity = 5f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private LayerMask aimLayerMask;
    private RaycastHit lastKnowMouseHit;
 
    private Vector2 mouseInput;
        
     void Start()
    {
        player = GetComponent<Player>();
        PlayerAimAssgin();
    }

    void Update()
    {
        UpdateAimVisauls();
        ChangeAimMode();
        LaserHandler();
        if(isAimingPrecisly){
            aim.position = new Vector3(GetMouseHitInfo().point.x, transform.position.y + 1, GetMouseHitInfo().point.z);    
        }else{
            aim.position = GetMouseHitInfo().point;
        }
    }

    private void LaserHandler(){
        if(Input.GetKeyDown(KeyCode.T)){
            Debug.Log(aimLaser.enabled);
            if(aimLaser.enabled){
                aimLaser.enabled = false;
            }else{
                aimLaser.enabled = true;
            }
        }
    }

    private void UpdateAimVisauls(){
        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();
        // // demo variable
        float tipLenght = 5f;
        float gunDistance = 4f;

        Vector3  endPoint = gunPoint.position + laserDirection * gunDistance;

        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            tipLenght = 0;
            // Debug.Log("gun point " + gunPoint.position);
            // Debug.Log("hit.point "+hit.point);
            // Debug.Log("gunDistance " +gunDistance);
            // isUpdateTipLength = false;
            // if(!isUpdateTipLength){
            //     isUpdateTipLength = true;
            //     lastestTipLength = tipLenght;
            // }
        }
        // lastestTipLength = tipLenght;
        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1,endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * tipLenght);
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

    

    public Transform Aim() => aim;

    public bool CanAimPrecisly() => isAimingPrecisly;
    // {
    //     if (isAimingPrecisly)
    //     {
    //         return true;
    //     }
    //     return false;
    // }

    

    #region  


    private Vector3 DesieredCameraPostion(){
        Quaternion rotationCam = virtualCamera.transform.rotation;
        Vector3 eulerRotation = rotationCam.eulerAngles;
        cameraYRotationFloat= Mathf.Abs(eulerRotation.y);
        int cameraRotationNor = Mathf.FloorToInt(cameraYRotationFloat);
        // Debug.Log("Y rotation angle: " + cameraRotationNor);

        if (cameraRotationNor >= 175 && cameraRotationNor <= 185 )
        {
            // cameraYRotationFloat = 0f;
            float actualMaxCameraDistance = player.movement.moveInput.y < +.5f ? minCameraDistance : maxCameraDistance;
            Vector3  desieredCameraPostion = GetMouseHitInfo().point;
            Vector3 aimDirection = (desieredCameraPostion - transform.position).normalized;
            float distainceToDesierdPosition = Vector3.Distance(transform.position, desieredCameraPostion);
            float clampedDistance = Mathf.Clamp(distainceToDesierdPosition, minCameraDistance, actualMaxCameraDistance);
            desieredCameraPostion = transform.position + aimDirection * clampedDistance;
            desieredCameraPostion.y = transform.position.y + 1;
            return desieredCameraPostion;   
        }
        else
        {
            float actualMaxCameraDistance = player.movement.moveInput.y < -.5f? minCameraDistance: maxCameraDistance;
            Vector3  desieredCameraPostion = GetMouseHitInfo().point;
            Vector3 aimDirection = (desieredCameraPostion - transform.position).normalized;
            float distainceToDesierdPosition = Vector3.Distance(transform.position, desieredCameraPostion);
            float clampedDistance = Mathf.Clamp(distainceToDesierdPosition, minCameraDistance, actualMaxCameraDistance);
            desieredCameraPostion = transform.position + aimDirection * clampedDistance;
            desieredCameraPostion.y = transform.position.y + 1;
            return desieredCameraPostion;   
        }
        
        
    }

    private void UpdateCameraPostion(){
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPostion(), cameraSensetivity * Time.deltaTime);
    }

    
    #endregion

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnowMouseHit;

    }
    private void PlayerAimAssgin(){
        controls = player.controls; 
        controls.Character.Aim.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => mouseInput = Vector2.zero;

    }

}
