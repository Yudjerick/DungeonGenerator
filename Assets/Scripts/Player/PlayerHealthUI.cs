using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image hpBar;
 
    private void OnEnable()
    {
        playerHealth.HealthChangedEvent += OnHealthChanged;
    }

    private void OnDisable()
    {
        playerHealth.HealthChangedEvent -= OnHealthChanged;
    }

    private void OnHealthChanged(float oldHealth, float newHealth)
    {
        hpBar.fillAmount = newHealth / playerHealth.MaxHealth;
    }
}
