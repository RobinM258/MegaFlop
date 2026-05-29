using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Laser : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    private ItemData itemData;
    public ItemData itemDataRef;
    public Camera mainCamera;
    public PlayerRecoilMotor recoilMotor;

    [Header("Ammo")]
    public int clipSize = 1;
    public int ammoInClip = 2;
    public float reloadTime = 1.2f;

    [Header("Laser")]
    public float laserLength;
    public LayerMask hitMask;

    private float nextFireTime = 0f;
    private bool isReloading = false;

    void Start()
    {
        ItemData instanceData = ScriptableObject.CreateInstance<ItemData>();
        instanceData.CopyFrom(itemDataRef);
        itemData = instanceData;
    }
    public void TryShoot()
    {
        if (isReloading)
            return;

        if (Time.time < nextFireTime)
            return;

        if (ammoInClip <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        FireShot();
    }

    void FireShot()
    {
        Vector2 aimDirection = GetAimDirection();
        if (aimDirection.sqrMagnitude <= 0.0001f)
            return;

        //Debug.Log("Shoot");
        aimDirection.Normalize();
        ammoInClip--;

        ShootLaser(aimDirection);

        if (ammoInClip <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    void ShootLaser(Vector2 direction)
    {
        float damage = itemData.Damage;
        float width = Mathf.Max(0.1f, itemData.Size);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Debug.Log("Damage = "+ itemData.Damage +" width = "+ width+" angle = "+angle);

        Vector2 boxSize = new Vector2(laserLength, width);
        Vector2 boxCenter = (Vector2)firePoint.position + direction * (laserLength * 0.5f);

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, hitMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                BasicEnemy enemy = hit.GetComponent<BasicEnemy>();
                if (enemy != null)
                {
                    enemy.GetDamage(damage, false);
                }
            }
            else if (hit.CompareTag("Tree"))
            {
                Vector3 hitPosition = hit.transform.position;
                ParticleManager.Instance.PlayEffect(ParticleEffectType.TreeDestroy, hitPosition);
                Destroy(hit.gameObject);
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammoInClip = clipSize;
        isReloading = false;
    }

    Vector2 GetAimDirection()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorld.z = 0f;

        Vector2 dir = (Vector2)(mouseWorld - firePoint.position);
        return dir.normalized;
    }

    void OnDrawGizmosSelected()
    {
        if (firePoint == null || itemData == null)
            return;

        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector2 dir;

        if (Application.isPlaying && Mouse.current != null && mainCamera != null)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mouseScreenPos);
            mouseWorld.z = 0f;
            dir = ((Vector2)(mouseWorld - firePoint.position)).normalized;
        }
        else
        {
            dir = firePoint.right; // direction par défaut dans l'éditeur
        }

        if (dir.sqrMagnitude < 0.0001f)
            dir = Vector2.right;

        float width = Mathf.Max(0.1f, itemData.Size);
        float length = laserLength;

        Vector2 boxSize = new Vector2(length, width);
        Vector2 boxCenter = (Vector2)firePoint.position + dir * (length * 0.5f);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Matrix4x4 oldMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, Quaternion.Euler(0f, 0f, angle), Vector3.one);

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawCube(Vector3.zero, boxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        Gizmos.matrix = oldMatrix;
    }
}