using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerformantShoot : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform startingPoint;
    [SerializeField] private float lifeTimer = 10f;

    private List<PerformantBullet> poolBullets = new List<PerformantBullet>();
    private List<PerformantBullet> activeBullets = new List<PerformantBullet>();

    [SerializeField] private int poolSize = 100;

    private void Start()
    {
        // initial setup for pool of bullets
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulletObj = Instantiate(bulletPrefab);

            bulletObj.SetActive(false);

            PerformantBullet bullet = bulletObj.GetComponent<PerformantBullet>();

            bullet.bulletDirection = Vector3.zero;
            bullet.bulletPosition = Vector3.zero;
            bullet.lifeTime = lifeTimer;
            bullet.active = false;

            bullet.SetTransform();

            poolBullets.Add(bullet);
        }
    }

    private void Update()
    {
        // everyone should run this part
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            PerformantBullet bullet = activeBullets[i];

            bullet.bulletPosition += bullet.bulletDirection * Time.deltaTime * bulletSpeed;
            bullet.bulletTransform.position = bullet.bulletPosition;

            bullet.lifeTime -= Time.deltaTime;

            if (bullet.lifeTime <= 0f)
            {
                ReturnBulletToPool(bullet);
                continue;
            }
        }

        if (!IsOwner) return;

        // this is if I need to add something for only local use
    }

    private PerformantBullet GetBulletFromPool()
    {
        foreach (PerformantBullet bullet in poolBullets)
        {
            if (!bullet.active)
            {
                bullet.active = true;

                bullet.bulletObject.SetActive(true);

                return bullet;
            }
        }

        return null;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        // could just do transform.forward later, but this is
        // just in case I want to add more dynamic direction later
        Vector3 direction = transform.forward;
        // spawn for server to update
        SpawnBulletServerRpc(startingPoint.position, direction);
    }

    private void SpawnBulletLocal(Vector3 startPoint, Vector3 direction)
    {
        PerformantBullet bullet = GetBulletFromPool();

        // pool empty error
        if (bullet == null)
        {
            Debug.LogWarning("Pool empty!");
            return;
        }

        // reset the bullets
        bullet.lifeTime = lifeTimer;

        bullet.bulletTransform.position = startPoint;
        bullet.bulletPosition = startPoint;
        // normalized is good for changing direction, up and down, in the future
        bullet.bulletDirection = direction.normalized;

        bullet.bulletObject.transform.position = startPoint;
        // this is good for if I find art to use
        // it will adjust where whatever it is is facing
        bullet.bulletObject.transform.rotation = Quaternion.LookRotation(direction);

        bullet.rb.linearVelocity = bullet.bulletDirection * bulletSpeed;

        activeBullets.Add(bullet);
    }

    private void ReturnBulletToPool(PerformantBullet bullet)
    {
        bullet.active = false;

        bullet.bulletObject.SetActive(false);

        activeBullets.Remove(bullet);
    }

    [ServerRpc]
    private void SpawnBulletServerRpc(Vector3 startPoint, Vector3 direction)
    {
        // spawn bullet for host/server
        //SpawnBulletLocal(startPoint, direction);
        // tell clients to spawn bullet too
        SpawnBulletClientRpc(startPoint, direction);
    }

    [ClientRpc]
    private void SpawnBulletClientRpc(Vector3 startPoint, Vector3 direction)
    {
        // spawn bullet on clients
        SpawnBulletLocal(startPoint, direction);
    }
}
