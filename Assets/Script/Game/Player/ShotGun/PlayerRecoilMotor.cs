using UnityEngine;

public class PlayerRecoilMotor : MonoBehaviour
{
    [Header("Recoil")]
    public float recoverySpeed = 10f;

    private Vector2 recoilVelocity;

    public void AddImpulse(Vector2 impulse)
    {
        recoilVelocity += impulse;
    }

    void Update()
    {
        transform.position += (Vector3)(recoilVelocity * Time.deltaTime);

        recoilVelocity = Vector2.Lerp(
            recoilVelocity,
            Vector2.zero,
            recoverySpeed * Time.deltaTime
        );
    }
}