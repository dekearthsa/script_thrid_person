using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{   
    private Rig rig;
    private WeaponVisaulControler weaponVisaulControler;
    private Player player;
    private PlayerControl controls;
    private CharacterController characterController; 
    
    private Animator animator;
    [Header("Move direction")]
    // [SerializeField] public float walkSpeed = 6f;
    // [SerializeField] public float runSpeed = 10f;
    // private float speed;
    public Vector3 movementDirection;
    private float verticalVelocity;
    private bool isRunning;
 
    [SerializeField] public float fallSpeed;
    [SerializeField] public float turnSpeed;

    
 
    public Vector2 moveInput {get; private set;}
    // private Vector2 aimInput;
    public bool nowCombatMode = true;
    
    // public float rotationAngle = 90f; // หมุนทีละ 90 องศา 
    // public float rotateSpeed = 360f; // องศาต่อวินาที
    // private float currentY = 0f;


    // private void Awake() {

    // }
    private void Start()
    {
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        weaponVisaulControler = GetComponentInChildren<WeaponVisaulControler>();

        FunctionPlayControl();
    }

    private void Update()
    {
        SetWalkingNoneCombat();
        ApplyMovementCB();
        ApplyRotation();
        AnimatorController();
        // ChangeCameraRotation();
    }

    private void ApplyRotation() {
        Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0;
        lookingDirection.Normalize();
        // transform.forward = lookingDirection; //// หันทันที 

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection); // ทิศการหมุ่น 
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime); // หน่วงการหมุ่น
        
        
        // if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask)){
        //     lookingDirection = hitInfo.point - transform.position;
        //     lookingDirection.y = 0;
        //     lookingDirection.Normalize();
        //     transform.forward = lookingDirection;

        //     aim.position = new Vector3(hitInfo.point.x, aim.position.y, hitInfo.point.z);

        // }
    }

    private void FunctionPlayControl(){
        controls = player.controls;

        controls.Character.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Character.Movement.canceled += ctx => moveInput = Vector2.zero;

        
        controls.Character.Run.performed += ctx =>{ 
                isRunning = true; 
            }; 
        controls.Character.Run.canceled += ctx => { 
                isRunning = false; 
            }; 
    }

    private void ApplyMovementCB(){
        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S
        Transform cam = Camera.main.transform;

        // ดึง forward และ right ของกล้อง แล้วทำให้แบนราบ (ไม่เอาแกน y)
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(cam.right, new Vector3(1, 0, 1)).normalized;

        // คำนวณทิศทางตามกล้อง
        movementDirection = camForward * v + camRight * h;

        // movementDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        ApplyGravity(); 
        if(movementDirection.magnitude > 0){
            if(!nowCombatMode){
                characterController.Move(movementDirection * Time.deltaTime * 3f); // walking none combat mode
            }else{
                if(isRunning == false){
                    characterController.Move(movementDirection * Time.deltaTime * 5f); // Speed combat walk
                }else{
                    characterController.Move(movementDirection * Time.deltaTime * 8f); // Speed combat Running
                }
            }
        }
    }

    // private void ChangeCameraRotation()
    // {
 
    //        if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         currentY += 90f;
    //         if (currentY >= 360f) currentY -= 360f;

    //         transform.rotation = Quaternion.Euler(60f, currentY, 0f); // 60 เป็นค่า X เดิมของคุณ
    //     }
    // }
 

    private void AnimatorController()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        /// animator.SetFloat(ชื่อ Animator ที่เราตั้งใน Animator parameter , ค่าที่เปลี่ยนแปลง, เพิ่มความ Smooth ของ animation, ปรับให้เวลาเป็นไปตาม Frame จริง ๆ);
        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool playerRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", playerRunAnimation);
    }


    
    private void SetWalkingNoneCombat(){
        
        if(Input.GetKeyDown(KeyCode.V)){
            // Debug.Log();
           
            if(nowCombatMode){
                animator.SetBool("IsCombat", false);
                nowCombatMode= false;
                 weaponVisaulControler.SwitchoffWeaponModels();
                for (int i = 1; i < animator.layerCount; i++)
                {
                    animator.SetLayerWeight(i, 0);
                }
                rig.weight = 0;
            }else{
                animator.SetBool("IsCombat", true);
                nowCombatMode= true;
                rig.weight = 1;
                // weaponVisaulControler.SwitchGunOn(weaponVisaulControler.currentGunPOS);
                weaponVisaulControler.SwitchOnCurrentModel();
                for (int i = 1; i < animator.layerCount; i++)
                {
                    animator.SetLayerWeight(i, 0);
                }
                // for (int i = 1; i < animator.layerCount; i++)
                // {
                //     animator.SetLayerWeight(i, 0);
                // }
                
                 animator.SetLayerWeight(1, animator.GetInteger("IsLayerIdx"));
            }
            
        }
    }

    
    private void ApplyGravity(){
        if(characterController.isGrounded == false){
            movementDirection.y  = verticalVelocity  -9.81f * fallSpeed * Time.deltaTime;
        }else{
            verticalVelocity = -.5f;
        }
    }

}
