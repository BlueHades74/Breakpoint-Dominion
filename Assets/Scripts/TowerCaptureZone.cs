using UnityEngine;
using System.Collections;
using Unity.Netcode;
using System.Xml.Serialization;

public class TowerCaptureZone : NetworkBehaviour
{
    private NetworkVariable<int> playersInZone = new(0);
    private NetworkVariable<int> capPercentage = new(0);
    private Coroutine captureRoutine;

    [SerializeField] private Renderer rend;
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer) enabled = false;
    }

    private void Start()
    {
        // set the local tower color grey
        rend.material.color = Color.grey;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        // not the player, don't continue
        if (!other.CompareTag("Player")) return;

        playersInZone.Value++;

        if (playersInZone.Value == 1) captureRoutine = StartCoroutine(CaptureTower());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;

        // not the player, don't continue
        if (!other.CompareTag("Player")) return;

        playersInZone.Value--;

        if (playersInZone.Value == 0 && captureRoutine != null)
        {
            StopCoroutine(captureRoutine);
            captureRoutine = null;
        }
    }

    [ClientRpc]
    private void ChangeColorClientRpc()
    {
        // server sends out message to set local tower color
        rend.material.color = Color.blueViolet;
    }

    private IEnumerator CaptureTower()
    {
        while (capPercentage.Value < 100)
        {
            capPercentage.Value += 5;

            if (capPercentage.Value >= 100) ChangeColorClientRpc();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
