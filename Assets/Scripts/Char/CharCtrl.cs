using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPlatformer : MonoBehaviour
{
    [Header("移动")]
    public float maxSpeed   = 8f;      // 最大地面速度
    public float accel      = 100f;    // 地面加速度
    public float airMult    = 0.7f;    // 空中加速度倍数

    [Header("摩擦")]
    public float friction   = 15f;     // 地面摩擦（让松开键后快速停）

    [Header("跳跃")]
    public float jumpVel    = 12f;
    public float fallMult   = 4f;
    public float lowJumpMult= 3f;

    [Header("地面检测")]
    public Transform groundCheck;
    public float   checkRadius = 0.2f;
    public LayerMask groundLayer;

    public Rigidbody2D rb;
    float   horizontal;      // -1,0,1  原始输入
    bool    jumpReq;         // 缓存按键（在 Update 里收输入）
    bool    isGrounded;

    public SkillManager skillManager;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            skillManager = GetComponent<SkillManager>();
        }

        public void RestoreScale() => transform.localScale = Vector3.one;

        public void RestoreGravity()
        {
            Physics2D.gravity = new Vector2(0, -9.81f);
            transform.Rotate(-180, 0, 0); // 翻回来
        }

    /* 1. 只在 Update 收输入 —— 不碰物理 */
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");           // -1,0,1

        if (Input.GetButtonDown("Jump"))
            jumpReq = true;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    /* 2. 所有物理操作放进 FixedUpdate */
    void FixedUpdate()
    {
        MoveHorizontal();
        ApplyJumpGravity();
        DoJump();
    }

    /* ------------ 横向：加速 + 摩擦 ------------ */
    void MoveHorizontal()
    {
        float targetSpeed = horizontal * maxSpeed;
        float speedDiff   = targetSpeed - rb.velocity.x;

        // 选择加速度：地面 full / 空中 * airMult
        float accelRate = (isGrounded ? accel : accel * airMult) * Time.fixedDeltaTime;

        // 计算力 F = m*a
        float force = accelRate * speedDiff * rb.mass;
        rb.AddForce(Vector2.right * force, ForceMode2D.Force);

        // 地面快速停住（防止左右来回滑）
        if (isGrounded && Mathf.Abs(horizontal) < 0.01f)
        {
            float frictionForce = Mathf.Min(Mathf.Abs(rb.velocity.x), friction) * -Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * frictionForce, ForceMode2D.Impulse);
        }
    }

    /* ------------ 跳跃 ------------ */
    void DoJump()
    {
        if (jumpReq && isGrounded)
        {
            // 直接给速度，避免累加力造成高度不稳
            rb.velocity = new Vector2(rb.velocity.x, jumpVel);
        }
        jumpReq = false;   // 一帧内只跳一次
    }

    /* ------------ 下落手感 ------------ */
    void ApplyJumpGravity()
    {
        if (rb.velocity.y < 0)
            rb.gravityScale = fallMult;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.gravityScale = lowJumpMult;
        else
            rb.gravityScale = 1f;
    }

    /* ------------ 可视化 ------------ */
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}