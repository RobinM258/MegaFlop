using UnityEngine;

public class CameraFollowElastic : MonoBehaviour
{
    public GameObject WorldObj;
    private UIManager UIManagerScript;

    public Transform target;
    public float smoothTime = 0.2f;
    public Vector3 baseOffset = new Vector3(0f, 0f, -10f);
    public Vector2 lookAhead = new Vector2(1.5f, 1f);

    private Vector3 velocity = Vector3.zero;
    private Vector3 lastTargetPos;

    void Start()
    {
        UIManagerScript = WorldObj.GetComponent<UIManager>();

        if (target != null)
            lastTargetPos = target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;
        if (!GameData.isPaused)
        {
            float dt = Time.deltaTime > 0 ? Time.deltaTime : Time.unscaledDeltaTime;
            Vector3 targetVelocity = (target.position - lastTargetPos) / dt;

            Vector3 dynamicOffset = new Vector3(
                Mathf.Clamp(targetVelocity.x * 0.1f, -lookAhead.x, lookAhead.x),
                Mathf.Clamp(targetVelocity.y * 0.1f, -lookAhead.y, lookAhead.y),
                0f
            );

            Vector3 desiredPosition = target.position + baseOffset + dynamicOffset;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref velocity,
                smoothTime
            );

            lastTargetPos = target.position;
        }
        else
        {
            transform.position = target.position;
        }
    }
}