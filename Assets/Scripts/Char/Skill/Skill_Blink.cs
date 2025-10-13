using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("通用引用")]
    public PlayerPlatformer player;        // 由外部 Init 注入
    private Rigidbody2D rb => player.rb;    // 快捷访问

    /* ============ 1. 瞬移 ============ */
    [Header("瞬移")]
    public float maxRadius = 4f;

    public void Teleport()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        Vector3 dir = mouse - player.transform.position;
        if (dir.magnitude > maxRadius) dir = dir.normalized * maxRadius;
        player.transform.position += dir;
    }

    /* ============ 2. 飞天 ============ */
    [Header("飞天")]
    public float flySpeed = 12f;
    public float flyDuration = 2f;

    public void Fly() => StartCoroutine(FlyRoutine());

    private IEnumerator FlyRoutine()
    {
        float t = 0;
        while (t < flyDuration)
        {
            t += Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, flySpeed);
            yield return null;
        }
    }

    /* ============ 3. 缩放 ============ */
    [Header("缩放")]
    private Vector3 originalScale;
    private readonly Vector3 big  = Vector3.one * 2f;
    private readonly Vector3 small = Vector3.one * 0.3f;

    public void Resize()
    {
        originalScale = player.transform.localScale;
        Vector3 target = Random.value > 0.5f ? big : small;
        player.transform.localScale = target;
        StartCoroutine(ResizeRestoreAfter(5f));
    }

    private IEnumerator ResizeRestoreAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.transform.localScale = originalScale;
    }

    /* ============ 4. 重力反转 ============ */
    [Header("重力反转")]
    public float flipDuration = 5f;
    private Vector3 originalGravity;

    public void GravityFlip()
    {
        originalGravity = Physics2D.gravity;
        Physics2D.gravity = -originalGravity;
        player.transform.Rotate(180, 0, 0);          // 视觉翻转
        StartCoroutine(GravityRestoreAfter(flipDuration));
    }

    private IEnumerator GravityRestoreAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.gravity = originalGravity;
        player.transform.Rotate(-180, 0, 0);
    }

    /* ============ 外部注入 ============ */
    public void Init(PlayerPlatformer owner) => player = owner;
}