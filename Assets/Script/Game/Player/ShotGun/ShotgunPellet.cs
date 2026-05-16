using UnityEngine;

public class ShotgunPellet : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 1.0f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BasicEnemy enemy = collision.GetComponent<BasicEnemy>();
            if (enemy != null)
            {
                enemy.GetDamage(damage);
            }

            Destroy(gameObject);
        }
        if (collision.CompareTag("Tree"))
        {
            Vector3 hitPosition = collision.transform.position;
        
            ParticleManager.Instance.PlayEffect(ParticleEffectType.TreeDestroy, hitPosition);
        
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}