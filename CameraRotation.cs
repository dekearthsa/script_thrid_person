using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private float currentY = 0f;
    public float rotationSpeed = 180f; // องศาต่อวินาที
    private Quaternion targetRotation;

     void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentY += 90f;
            targetRotation = Quaternion.Euler(60f, currentY, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            currentY -= 90f;
            targetRotation = Quaternion.Euler(60f, currentY, 0f);
        }

        // กด Q เพื่อหมุน 90° ทางซ้าย (ย้อนกลับ)
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     targetRotation *= Quaternion.Euler(0, -90f, 0);
        // }

        // Smoothly หมุนไปยัง targetRotation
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
