using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{

    [SerializeField] private float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (IsOwner)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;


    }

    public void Move()
    {

    }
}
