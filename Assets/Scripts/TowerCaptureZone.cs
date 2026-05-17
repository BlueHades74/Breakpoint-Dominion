using UnityEngine;

public class TowerCaptureZone : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("CAPTURING!");
        }
    }

}
