using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotgunWeapon : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject pelletPrefab;
    public Camera mainCamera;
    public PlayerRecoilMotor recoilMotor;

    [Header("Ammo")]
    public int clipSize = 2;
    public int ammoInClip = 2;
    public float reloadTime = 1.2f;
    public float fireCooldown = 0.35f;

    [Header("Shotgun")]
    public int pelletCount = 7;
    public float spreadAngle = 14f;
    public float pelletSpeed = 14f;
    public float recoilForce = 4f;

    private float nextFireTime = 0f;
    private bool isReloading = false;

    void Update()
    {

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

        aimDirection.Normalize();

        for (int i = 0; i < pelletCount; i++)
        {
            float angleOffset = Random.Range(-spreadAngle, spreadAngle);
            Vector2 shotDirection = RotateVector(aimDirection, angleOffset);

            GameObject pellet = Instantiate(pelletPrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = shotDirection * pelletSpeed;
            }
        }

        ammoInClip--;
        nextFireTime = Time.time + fireCooldown;

        if (recoilMotor != null)
        {
            recoilMotor.AddImpulse(-aimDirection * recoilForce);
        }

        if (ammoInClip <= 0)
        {
            StartCoroutine(Reload());
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

    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        ).normalized;
    }
}