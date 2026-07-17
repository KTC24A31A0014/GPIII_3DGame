using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotateSpeed = 20f;

    [SerializeField] int HP = 2;
    [SerializeField] float invincibleTimeMax = 0.5f;
    [SerializeField] float knockbackSpeed = 5f;

    Rigidbody rb;

    float invincibleTime = 0f;

    public Collider playerCollider {  get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = playerCollider.bounds.center - rb.position;

        bool isSeenPlayer = true;
        if (Physics.Raycast(rb.position, direction.normalized, out var hitinfo))
        {
            if (hitinfo.collider != playerCollider)
            {
                // ÉvÉåÉCÉÑÅ[à»äOÇÃè·äQï®Ç…Ç†ÇΩÇ¡ÇΩèÍçáÇÕå©Ç¶Ç»Ç¢
                isSeenPlayer = false;
            }
        }

        if (isSeenPlayer && invincibleTime <= 0)
        {
            var subVec = playerCollider.bounds.center - rb.position;
            subVec.y = 0;
            rb.linearVelocity = subVec.normalized * moveSpeed;

            // PlayerÇÃï˚å¸Çå©ÇÈ
            var rotateTarget = subVec.normalized;
            Vector3 forward = transform.forward;
            transform.forward = Vector3.Slerp(forward, rotateTarget, rotateSpeed * Time.deltaTime);
        }

        // ñ≥ìGéûä‘Çå∏ÇÁÇ∑
        if (invincibleTime > 0f) invincibleTime -= Time.deltaTime;
    }

    // îÌíeèàóù
    private void OnCollisionStay(Collision collision)
    {
        var attackObj = collision.gameObject.GetComponent<AttackObject>();
        if (attackObj != null && invincibleTime <= 0 && !attackObj.CompareTag("Enemy"))
        {
            HP -= attackObj.power;
            invincibleTime = invincibleTimeMax;
            if (HP <= 0)
            {
                Destroy(gameObject);
            }

            // knockback
            var dir = transform.position - collision.transform.position;
            dir.y = 0;
            var knockbackVec = dir.normalized * knockbackSpeed;
            rb.linearVelocity = knockbackVec;
        }
    }
}
