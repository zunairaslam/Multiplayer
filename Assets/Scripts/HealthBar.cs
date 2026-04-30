using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float maxHealth = 100f;

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public float CurrentHealth { get; set; }

    private void OnValidate()
    {
        Debug.Log("[HealthBar] OnValidate called");
        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
            Debug.Log("[HealthBar] Slider found in OnValidate: " + (healthSlider != null));
        }
    }

    private void Awake()
    {
        Debug.Log("[HealthBar] Awake called");
        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
            Debug.Log("[HealthBar] Slider found in Awake: " + (healthSlider != null));
        }
    }

    private void Start()
    {
        Debug.Log("[HealthBar] Start called");
        Debug.Log("[HealthBar] Slider is null: " + (healthSlider == null));

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            Debug.Log("[HealthBar] Slider configured - Min: 0, Max: " + maxHealth);
        }

        // Initialize health only if this player has state authority
        Debug.Log("[HealthBar] HasStateAuthority: " + HasStateAuthority);
        if (HasStateAuthority)
        {
            Debug.Log("[HealthBar] Setting CurrentHealth to: " + maxHealth);
            CurrentHealth = maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = CurrentHealth;
            Debug.Log("[HealthBar] Slider value set to: " + CurrentHealth);
        }
        else
        {
            Debug.LogWarning("[HealthBar] Slider is NULL in Start!");
        }
    }

    private void OnHealthChanged()
    {
        Debug.Log("[HealthBar] OnHealthChanged called - CurrentHealth: " + CurrentHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        Debug.Log("[HealthBar] UpdateHealthBar called - CurrentHealth: " + CurrentHealth);
        if (healthSlider != null)
        {
            healthSlider.value = CurrentHealth;
            Debug.Log("[HealthBar] Slider updated to: " + CurrentHealth);
        }
        else
        {
            Debug.LogWarning("[HealthBar] UpdateHealthBar - Slider is NULL!");
        }
    }

    /// <summary>
    /// Deal damage to this health bar. Only works if you have state authority.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (HasStateAuthority)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        }
    }

    /// <summary>
    /// Heal this health bar. Only works if you have state authority.
    /// </summary>
    public void Heal(float amount)
    {
        if (HasStateAuthority)
        {
            CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        }
    }

    /// <summary>
    /// Set health to a specific value. Only works if you have state authority.
    /// </summary>
    public void SetHealth(float health)
    {
        if (HasStateAuthority)
        {
            CurrentHealth = Mathf.Clamp(health, 0, maxHealth);
        }
    }

    /// <summary>
    /// RPC to deal damage from any client (will be executed on state authority)
    /// </summary>
    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void DealDamageRpc(float damage)
    //{
    //    TakeDamage(damage);
    //}

    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return (CurrentHealth / maxHealth) * 100f;
    }
}
