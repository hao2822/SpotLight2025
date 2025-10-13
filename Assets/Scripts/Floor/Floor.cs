using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D), typeof(PlatformEffector2D))]
public class Floor : MonoBehaviour
{
    [Header("基本")]
    public bool isOneWay = true;          // 是否单向
    public float surfaceFriction = 0.2f; // 0=冰面 1=粗糙

    [Header("可选行为")]
    public bool isCracked = false;       // 易碎
    public float crackDelay = 0.8f;      // 踩上后多久碎
    public bool isMoving = false;        // 来回移动
    public Vector2 moveDistance = new(5, 0);
    public float moveSpeed = 2f;

    [Header("事件")]
    public UnityEvent onLand;    // 玩家踩上
    public UnityEvent onLeave;   // 玩家离开
    public UnityEvent onBreak;   // 开始碎

    new BoxCollider2D collider;
    PlatformEffector2D effector;
    Vector3 startPos;
    int playerLayer;

    // 破碎计时
    float crackTimer = -1f;
    bool playerOnTop = false;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        effector = GetComponent<PlatformEffector2D>();
        startPos = transform.position;
        playerLayer = LayerMask.NameToLayer("Player");
        ApplyFriction();
        effector.useOneWay = isOneWay;
    }

    /* -------------- 生命周期 -------------- */
    void Update()
    {
        if (isMoving)
            DoPingPongMove();

        if (crackTimer > 0)
        {
            crackTimer -= Time.deltaTime;
            if (crackTimer <= 0)
                BreakNow();
        }
    }

    /* -------------- 碰撞检测 -------------- */
    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.layer != playerLayer) return;

        // 只算“从上方踩上”
        if (IsLandingFromTop(hit))
        {
            playerOnTop = true;
            onLand.Invoke();

            if (isCracked)
            {
                crackTimer = crackDelay;
            }
        }
    }

    void OnCollisionExit2D(Collision2D hit)
    {
        if (hit.gameObject.layer != playerLayer) return;
        playerOnTop = false;
        onLeave.Invoke();
    }

    /* -------------- 私有方法 -------------- */
    bool IsLandingFromTop(Collision2D col)
    {
        // 简单判断：接触点法线朝上（>0.7）即可
        foreach (var p in col.contacts)
            if (Vector2.Dot(p.normal, Vector2.up) > 0.7f) return true;
        return false;
    }

    void ApplyFriction()
    {
        // 通过物理材质改表面摩擦
        var mat = new PhysicsMaterial2D
        {
            friction = surfaceFriction,
            bounciness = 0
        };
        collider.sharedMaterial = mat;
    }

    void DoPingPongMove()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, 1);
        transform.position = startPos + (Vector3)moveDistance * t;
    }

    void BreakNow()
    {
        onBreak.Invoke();
        // 1. 关闭碰撞，让玩家掉下去
        collider.enabled = false;
        // 2. 可选：掉落动画
        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1.5f;
        // 3. 延迟销毁
        Destroy(gameObject, 2f);
    }

    /* -------------- 公共接口 -------------- */
    public void SetOneWay(bool enable) => effector.useOneWay = enable;
    public void SetFriction(float f)
    {
        surfaceFriction = f;
        ApplyFriction();
    }
}