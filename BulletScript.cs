using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private  Rigidbody rb => GetComponent<Rigidbody>();
    private void OnCollisionEnter(Collision other)
    {
        // rb.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(gameObject);
    }
}
