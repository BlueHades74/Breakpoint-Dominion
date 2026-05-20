using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private GameObject canvas;

    private void Awake()
    {
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Cursor.lockState = CursorLockMode.Locked;
            canvas.SetActive(false);
        });

        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Cursor.lockState = CursorLockMode.Locked;
            canvas.SetActive(false);
        });
    }
}
