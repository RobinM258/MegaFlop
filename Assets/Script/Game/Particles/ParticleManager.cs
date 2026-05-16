using UnityEngine;

public enum ParticleEffectType
{
    TreeDestroy,
    EnemyDeath
}

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [Header("Particle Prefabs")]
    [SerializeField] private ParticleSystem treeDestroyEffect;
    [SerializeField] private ParticleSystem enemyDeathEffect;

    [Header("Settings")]
    [SerializeField] private float defaultDestroyDelay = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayEffect(ParticleEffectType effectType, Vector3 position)
    {
        PlayEffect(effectType, position, Vector2.zero);
    }

    public void PlayEffect(ParticleEffectType effectType, Vector3 position, Vector2 direction)
    {
        ParticleSystem prefab = GetEffectPrefab(effectType);

        if (prefab == null)
        {
            Debug.LogWarning($"Aucun prefab assigné pour l'effet {effectType}.", this);
            return;
        }

        Quaternion rotation = GetEffectRotation(effectType, direction);

        ParticleSystem fx = Instantiate(prefab, position, rotation);

        ApplyOptionalSettings(effectType, fx, direction);

        fx.Play();
        Destroy(fx.gameObject, defaultDestroyDelay);
    }

    private ParticleSystem GetEffectPrefab(ParticleEffectType effectType)
    {
        switch (effectType)
        {
            case ParticleEffectType.TreeDestroy:
                return treeDestroyEffect;

            case ParticleEffectType.EnemyDeath:
                return enemyDeathEffect;

            default:
                return null;
        }
    }

    private Quaternion GetEffectRotation(ParticleEffectType effectType, Vector2 direction)
    {
        switch (effectType)
        {
            case ParticleEffectType.EnemyDeath:
                if (direction != Vector2.zero)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
                    return Quaternion.Euler(0f, 0f, angle);
                }
                return Quaternion.identity;

            case ParticleEffectType.TreeDestroy:
            default:
                return Quaternion.identity;
        }
    }

    private void ApplyOptionalSettings(ParticleEffectType effectType, ParticleSystem fx, Vector2 direction)
    {
        switch (effectType)
        {
            case ParticleEffectType.EnemyDeath:
                var shape = fx.shape;
                shape.radius = Random.Range(0.5f, 1.5f);
                break;
        }
    }
}