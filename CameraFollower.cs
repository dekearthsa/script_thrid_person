using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Vector3 offset;

    void Start()
    {
        // เก็บระยะห่างเริ่มต้นระหว่างกล้องกับตัวละคร
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // ให้กล้องตามตำแหน่งตัวละคร โดยคงระยะ offset ไว้ตลอด
        transform.position = player.position + offset;

        // หมุนกล้องคงเดิมไว้ (ถ้าอยากให้กล้องหมุนตามตัวละครค่อยว่ากัน)
        // หรือใช้ LookAt หากต้องการมุมกล้องตรงกลางจริง ๆ
        // transform.LookAt(player);  // ← ใส่อันนี้ถ้าอยากให้โฟกัสตรงตัวละครเสมอ
    }
}
