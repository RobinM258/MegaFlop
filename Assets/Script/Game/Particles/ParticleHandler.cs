using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    public ParticleSystem destroyTree;
    public ParticleSystem enemyDying;

    public void CreateParticle(string tag, Vector3 hitPosition, Vector2 hitDirection)
        {
            switch (tag)
            {
                case "Tree":
                    Debug.Log("Tree");
                    ParticleSystem treeFx = Instantiate(destroyTree, hitPosition, Quaternion.identity);
                    treeFx.Play();
                    Destroy(treeFx.gameObject, 3f);
                    break;

                case "Enemy":
                    Debug.Log("Enemy");
                    float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg + 180f;
                    ParticleSystem enemyFx = Instantiate(enemyDying, hitPosition, Quaternion.Euler(0f, 0f, angle));

                    var shape = enemyFx.shape;
                    shape.radius = Random.Range(5, 15);

                    enemyFx.Play();
                    Destroy(enemyFx.gameObject, 3f);
                    break;
            }
        }
}