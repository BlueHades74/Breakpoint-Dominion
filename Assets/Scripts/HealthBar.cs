using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }
}
