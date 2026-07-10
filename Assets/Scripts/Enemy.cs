using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotateSpeed = 20f;

    Rigidbody rb;

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
                // プレイヤー以外の障害物にあたった場合は見えない
                isSeenPlayer = false;
            }
        }

        if (isSeenPlayer)
        {
            var subVec = playerCollider.bounds.center - rb.position;
            subVec.y = 0;
            rb.linearVelocity = subVec.normalized * moveSpeed;

            var rotateTarget = subVec.normalized;
            Vector3 forward = transform.forward;
            transform.forward = Vector3.Slerp(forward, rotateTarget, rotateSpeed * Time.deltaTime);
        }
    }
}
