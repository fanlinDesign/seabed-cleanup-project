using UnityEngine;

public class StartBarrierBlocker : MonoBehaviour
{
    [Header("Which object is the player root? (XR Origin)")]
    public Transform xrOrigin;

    [Header("Barrier")]
    public bool barrierEnabled = true;

    [Tooltip("Pushback strength (meters). 0.1~0.3 is usually enough.")]
    public float pushBack = 0.2f;

    private void OnTriggerStay(Collider other)
    {
        if (!barrierEnabled) return;
        if (xrOrigin == null) return;

        // 只对玩家生效：用名字/Tag 过滤，避免碰到其他物体触发
        // ✅ 推荐你给 XR Origin 加一个 Tag: Player
        if (!other.CompareTag("Player")) return;

        // 把 XR Origin 往后推一点（沿着空气墙的反方向）
        Vector3 away = (xrOrigin.position - transform.position);
        away.y = 0f;

        if (away.sqrMagnitude < 0.0001f)
            away = -transform.forward;

        away.Normalize();

        xrOrigin.position += away * pushBack;
    }

    public void SetBarrier(bool on)
    {
        barrierEnabled = on;
        // 也可以直接禁用 collider
        var col = GetComponent<Collider>();
        if (col) col.enabled = on;
    }
}