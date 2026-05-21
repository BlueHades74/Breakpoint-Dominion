using System;
using Unity.Netcode;
using UnityEngine;

public class RotatorLaser : NetworkBehaviour
{
    [SerializeField] private float defaultDistance = 100f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private Transform m_transform;
    private RaycastHit hit;

    private float laserDamage = 25;

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Physics.Raycast(m_transform.position, transform.up, out hit))
        {
            // draw to the hit point
            DrawRay(firePoint.position, hit.point);

            if (!IsServer) return;

            if (hit.collider.CompareTag("Player"))
            {
                // grab specific player's health
                PlayerMovement health = hit.collider.GetComponent<PlayerMovement>();
                health.TakeDamage(laserDamage);
            }
        }
        else
        {
            // draw to the max distance
            DrawRay(firePoint.position, firePoint.transform.up * defaultDistance);
        }
    }

    private void DrawRay(Vector3 startPos, Vector3 endPos)
    {
        laserLine.SetPosition(0, startPos);
        laserLine.SetPosition(1, endPos);
    }
}
