using System;
using Unity.Netcode;
using UnityEngine;

public class RotatorLaser : MonoBehaviour
{
    [SerializeField] private float defaultDistance = 100f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private Transform m_transform;
    private RaycastHit hit;

    public static event Action<GameObject> onPlayerHit;

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

            if (hit.collider.CompareTag("Player"))
            {
                onPlayerHit?.Invoke(hit.collider.gameObject);
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
