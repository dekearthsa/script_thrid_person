using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisaulControler : MonoBehaviour
{
    private Rig rig;
    private Animator amimr;
    private PlayerMovement playerMovement;
    private PlayerControllerWaepon playerControllerWaepon;
    // private WeaponVisaulControler weaponVisaulControler;

    [Header("Gun")]
    [SerializeField] private Transform[] gunTransform;
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform rifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniper;

    [Header("Left hand target tranform")]
    public Transform currentGunPOS;
   

    [Header("Rig")]
    [SerializeField] private float rigIncreaseStep;
    private bool isRigIncrease;
    // private float timer = 0f;

    [Header("Left hand IK")]
    [SerializeField] public TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_target;
    [SerializeField] private float leftHandIK_InreaseStep = 1.5f;
    private bool shouldIncreaseLeftHandIKWeight;
    private bool busyGrabingWeapon;
    // private Weapon currentWeapon;



    private void Start()
    {
        SwitchGunOn(pistol);
        amimr = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerControllerWaepon = GetComponentInParent<PlayerControllerWaepon>();
        // currentWeapon = GetComponentInParent<Weapon>();
        // weaponVisaulControler = GetComponentInParent<WeaponVisaulControler>();
    }

    private void Update()
    {
        CheckWeaponSwitch();
        // if(Input.GetKeyDown(KeyCode.R) && (!busyGrabingWeapon)){
        //     PlayReloadAnimation();

        // }
        if (playerControllerWaepon.CurrentWeapon().isReloadingAimation == true && IsReloadAnimationFinished())
        {
            playerControllerWaepon.CurrentWeapon().isReloadingAimation = false;
        }
   
        UpdateRigWeight();
        UpdateLeftHandIKWeight();
        
    }

    private bool IsReloadAnimationFinished()
{
    AnimatorStateInfo stateInfo = amimr.GetCurrentAnimatorStateInfo(1); // ใช้ layer ที่ 0 หรือ layer ที่คุณใช้กับ reload
        
        bool stats =  stateInfo.IsName("Reloading Weapon") && stateInfo.normalizedTime >= 1.0f;
        // Debug.Log(stats);
        return stateInfo.IsName("Reloading Weapon") && stateInfo.normalizedTime >= 1.0f;
}


    public void PlayReloadAnimation()
    {
        playerControllerWaepon.CurrentWeapon().isReloadingAimation = true;
        Debug.Log(playerControllerWaepon.CurrentWeapon().isReloadingAimation);
        if (busyGrabingWeapon) return;
        amimr.SetBool("IsReloading", true);
        amimr.SetTrigger("Reload");
        PauseRig();
    }

    private void UpdateRigWeight()
    {
        if (isRigIncrease)
        {
            // Debug.Log(isRigIncrease);
            rig.weight += rigIncreaseStep * Time.deltaTime;
            if (rig.weight >= 1)
            {
                // Debug.Log("here");
                isRigIncrease = false;
                amimr.SetBool("IsReloading", false);
                playerControllerWaepon.CurrentWeapon().isReloadingAimation = false;
                Debug.Log(playerControllerWaepon.CurrentWeapon().isReloadingAimation);
            }
        }
    }

    private void UpdateLeftHandIKWeight(){
        if(shouldIncreaseLeftHandIKWeight){
            leftHandIK.weight += leftHandIK_InreaseStep * Time.deltaTime;
            if(leftHandIK.weight >= 1){
                shouldIncreaseLeftHandIKWeight = false;
            }
        }
    }
    private void PauseRig(){
        rig.weight = 0.1f;    
    }

    public void ReturnWeightToOne() => isRigIncrease = true;
    public void ReturnWieghtHandWeightIK() => shouldIncreaseLeftHandIKWeight = true;
 
    private void PlayerWeaponGrabAnimation(GrabType grabType) {
        // wepaonPOS = leftHandPOS.transform;
        // wepaonPOS.position = leftHandPOS.position;
        // wepaonPOS.rotation = leftHandPOS.rotation;
        leftHandIK.weight = 0;
        PauseRig();
        amimr.SetTrigger("WeaponGrab"); 
        amimr.SetFloat("WeaponGrabType", ((float)grabType));
        SetBusyGrabingWeapon(true);
    }

    public void SetBusyGrabingWeapon(bool isBusy){
        busyGrabingWeapon = isBusy;
        amimr.SetBool("IsGrabingWeapon", busyGrabingWeapon);
    }
 

    private void CheckWeaponSwitch(){
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {   
            playerMovement.nowCombatMode = true;
            StartAnimationCombatMode();
            SwitchGunOn(pistol);
            ChangeAnimationGunFire(1);
            PlayerWeaponGrabAnimation(GrabType.BackGrab);

        };
        if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            playerMovement.nowCombatMode = true;
            StartAnimationCombatMode();
            SwitchGunOn(revolver); 
            ChangeAnimationGunFire(1); 
            PlayerWeaponGrabAnimation(GrabType.BackGrab);
            };
        if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            playerMovement.nowCombatMode = true;
            StartAnimationCombatMode();
            SwitchGunOn(rifle); 
            ChangeAnimationGunFire(1); 
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
            };
        if (Input.GetKeyDown(KeyCode.Alpha4)) { 
            playerMovement.nowCombatMode = true;
            StartAnimationCombatMode();
            SwitchGunOn(shotgun); 
            ChangeAnimationGunFire(2); 
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
            };
        if (Input.GetKeyDown(KeyCode.Alpha5)) { 
            playerMovement.nowCombatMode = true;
            StartAnimationCombatMode();
            SwitchGunOn(sniper); 
            ChangeAnimationGunFire(3); 
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
            };
    }

    private void StartAnimationCombatMode(){
        amimr.SetBool("IsCombat", true);
        rig.weight = 1;
        for (int i = 1; i < amimr.layerCount; i++)
        {
            amimr.SetLayerWeight(i, 0);
        }
        amimr.SetLayerWeight(1, amimr.GetInteger("IsLayerIdx"));
    }


    public void SwitchGunOn(Transform gunTransfrom)
    {
        SwitchGunOff();
        gunTransfrom.gameObject.SetActive(true);
        currentGunPOS = gunTransfrom;
        AttachLeftHand();
    }

    public void SwitchGunOff()
    {
        for (int i = 0; i < gunTransform.Length; i++)
        {
            gunTransform[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTranform = currentGunPOS.GetComponentInChildren<LeftHandTargetTranform>().transform;
        leftHandIK_target.localPosition = targetTranform.localPosition;
        leftHandIK_target.localRotation = targetTranform.localRotation;

    }

    private void ChangeAnimationGunFire(int animatorLayerIdx)
    {
        amimr.SetBool("IsReloading", false);
        
        for (int i = 1; i < amimr.layerCount; i++)
        {
            amimr.SetLayerWeight(i, 0);
        }
        amimr.SetLayerWeight(animatorLayerIdx, 1);
        amimr.SetInteger("IsLayerIdx", animatorLayerIdx);
    }


}

public enum GrabType {SideGrab=0, BackGrab=1};