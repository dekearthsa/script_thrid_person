using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpactFX;
    private Rigidbody rb => GetComponent<Rigidbody>();
    private void OnCollisionEnter(Collision other)
    {
        // rb.constraints = RigidbodyConstraints.FreezeAll;
        BulletImpact(other);
        Destroy(gameObject);
    }

    private void BulletImpact(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject newImpact = Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(newImpact,1f);
        }
    }
}
