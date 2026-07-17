using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("基礎移動")]
    [SerializeField] float speedMax;
    [SerializeField] float accel = 15f;
    [SerializeField] float rotateSpeed = 20f;

    [Header("ジャンプ関連")]
    [SerializeField] float jumpSpeed = 12f;
    [SerializeField] float groundNormalYMin = 0.7f;
    [SerializeField] float groundDamping = 8f;
    [SerializeField] float airDamping = 0.2f;

    [Header("攻撃（ファイア）")]
    [SerializeField] GameObject firePrefab;
    [SerializeField] float fireSpeed = 5f;
    [SerializeField] Vector3 fireOffset;

    [Header("被弾処理")]
    [SerializeField] int HP = 2;
    [SerializeField] float invincibleTimeMax = 0.5f;
    [SerializeField] float knockbackSpeed = 5f;

    PlayerInput playerInput;
    Rigidbody rb;
    Animator animator;
    Vector3 rotateTarget;

    bool isGrounded = false;
    float invincibleTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        rb.sleepThreshold = -1;
    }

    private void FixedUpdate()
    {
        // 減衰を地上と空中で変える
        if (isGrounded)
        {
            rb.linearDamping = groundDamping;
        }
        else
        {
            rb.angularDamping = airDamping;
        }

        // 物理計算中に設置判定を行うため、一旦ここでfalseにしておく
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        var accelVec = playerInput.actions["Move"].ReadValue<Vector2>();

        var cameraDir = playerInput.camera.transform.forward;
        cameraDir.y = 0;
        cameraDir = cameraDir.normalized;

        var cameraRight = playerInput.camera.transform.right;

        var accelVec3D = cameraDir * accelVec.y * accel + cameraRight * accelVec.x * accel;

        rb.AddForce(accelVec3D, ForceMode.Acceleration);

        if (accelVec3D != Vector3.zero)
        {
            rotateTarget = accelVec3D.normalized;
        }
        // 前方向をコピーしておく
        Vector3 forward = transform.forward;

        // 上方向を固定
        transform.up = Vector3.up;

        // 前方向をターゲットに向かって補間
        transform.forward = Vector3.Slerp(forward, rotateTarget, rotateSpeed * Time.deltaTime);

        // AnimatorのMoveSpeedパラメータにRigidbodyの移動速度の大きさを与える
        Vector3 velocityXZ = rb.linearVelocity;
        velocityXZ.y = 0;
        animator.SetFloat("MoveSpeed", rb.linearVelocity.magnitude);

        // Jump
        if (isGrounded && playerInput.actions["Jump"].WasPressedThisFrame())
        {
            Vector3 jumpVec = new Vector3(0, jumpSpeed, 0);
            rb.AddForce(jumpVec, ForceMode.VelocityChange);
        }

       // Attack
       if (playerInput.actions["Attack"].WasPressedThisFrame())
        {
            var position = transform.position + transform.TransformVector(fireOffset);
            var fireObj = Object.Instantiate(firePrefab, position,transform.rotation);
            var fireRB  = fireObj.GetComponent<Rigidbody>();
            if (fireRB != null)
            {
                fireRB.linearVelocity = transform.forward * fireSpeed;
            }
        }

        // 無敵時間を減らす
        if (invincibleTime > 0f) invincibleTime -= Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y >= groundNormalYMin)
            {
                isGrounded = true;
            }
        }

        // 被弾処理
        var attackObj = collision.gameObject.GetComponent<AttackObject>();
        if (attackObj != null && invincibleTime <= 0)
        {
            HP -= attackObj.power;
            invincibleTime = invincibleTimeMax;
            if (HP <= 0)
            {
                // Destroy(gameObject);
                Debug.Log("Game Over");
            }

            // knockback
            var dir = transform.position - collision.transform.position;
            dir.y = 0;
            var knockbackVec = dir.normalized * knockbackSpeed;
            rb.AddForce(knockbackVec, ForceMode.VelocityChange);
        }
    }
}
