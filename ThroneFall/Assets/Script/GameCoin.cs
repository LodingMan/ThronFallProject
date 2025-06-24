using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GameCoin : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameCoinHandler coinHandler;
    
    [Header("Floating Settings")]
    public float hoverHeight = 1.5f; 
    public float followSharpness = 8f; 
    public float rotationSpeed = 270f; 
    
    [Header("Magnet Settings")]
    public float detectRange = 3f;
    public float minSpeed = 3f;
    public float maxSpeed = 12f;
    public AnimationCurve attractionCurve;
    public float stopDistance = 0.3f;
    
    [Header("Physics Settings")]
    public float drag = 1.5f;
    public float angularDrag = 0.5f;
    public float hoverDamp = 0.6f; // 부양 감쇠 계수

    private Rigidbody rb;
    private bool isAttracted;
    private bool isField = false;
    private Vector3 targetPosition;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.drag = drag;
        rb.angularDrag = angularDrag;
        
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!coinHandler) coinHandler = FindObjectOfType<GameCoinHandler>();
    }

    private void FixedUpdate()
    {
        if (!isField || !player) return;
        targetPosition = player.position + Vector3.up * hoverHeight;
        Vector3 toTarget = targetPosition - transform.position;
        float distance = toTarget.magnitude;

        if (!isAttracted && distance < detectRange)
        {
            isAttracted = true;
            currentSpeed = minSpeed;
        }

        if (isAttracted)
        {
            float progress = 1 - Mathf.Clamp01(distance / detectRange);
            float targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, attractionCurve.Evaluate(progress));
            
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, followSharpness * Time.fixedDeltaTime);

            Vector3 followForce = toTarget.normalized * currentSpeed;
            rb.AddForce(followForce - rb.velocity * hoverDamp, ForceMode.Acceleration);

            rb.AddTorque(Vector3.up * rotationSpeed * Mathf.Deg2Rad, ForceMode.Acceleration);
        }

        if (distance <= stopDistance)
        {
            ReturnCoin();
        }
    }

    public void SpawnCoin(Vector3 position)
    {
        isField = true;
        isAttracted = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = position;
        rb.useGravity = false;
        gameObject.SetActive(true);
        ApplyRandomBounce();

    }
    private void ApplyRandomBounce()
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1f),  // 위쪽으로 더 많이 튀도록
            Random.Range(-1f, 1f)
        ).normalized;

        float randomForce = Random.Range(3f, 7f);
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * Random.Range(100f, 300f);

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }

    public void ReturnCoin()
    {
        if (!isField) return;
        
        isField = false;
        isAttracted = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
        coinHandler?.PushCoin(this);
    }

    private void OnDrawGizmosSelected()
    {
        if (player && isField)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPosition);
            Gizmos.DrawWireSphere(targetPosition, 0.2f);
        }
    }
}