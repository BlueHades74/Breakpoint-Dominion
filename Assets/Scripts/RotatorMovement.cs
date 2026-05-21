using Unity.Netcode;
using UnityEngine;

public class RotatorMovement : MonoBehaviour
{
    [SerializeField] private float rotateAmount = 10f;

    
    private void Update()
    {
        TurnRotator();
    }
    private void TurnRotator()
    {
        transform.Rotate(transform.up * rotateAmount * Time.deltaTime);
    }
}
