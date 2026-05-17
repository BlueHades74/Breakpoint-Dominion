using UnityEngine;

public class PerformantBullet : MonoBehaviour
{
    public Transform bulletTransform;
    public Vector3 bulletPosition;
    public Vector3 bulletDirection;
    public float lifeTime;
    public GameObject bulletObject;
    public bool active;
    public Rigidbody rb;

    public void SetTransform()
    {
        bulletTransform = this.gameObject.transform;
        bulletObject = this.gameObject;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("It hit the wall!");
            this.lifeTime = 0f;
        }
    }
}
