using System;
using UnityEngine;

public class PlayerGroundedCheck : MonoBehaviour
{
    // action to alert the player is touching the ground
    public static event Action onTouchingGround;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // player is touching ground, sound alarm
            onTouchingGround?.Invoke();
        }
    }
}
