using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // destroy bullet
            // destroy/ deal damage to enemy

            // probably going to need to call an action
            // for performant shoot to listen for
        }
    }
}
