using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject startUI;          // 你的开始Panel（或整个Canvas）

    [Header("Barrier")]
    public Collider startBarrier;       // StartBarrier 的 BoxCollider

    private bool _started = false;

    private void Start()
    {
        // 开局：显示UI、开启空气墙
        if (startUI != null) startUI.SetActive(true);
        if (startBarrier != null) startBarrier.enabled = true;
        _started = false;
    }

    // 按钮 OnClick 绑定这个
    public void StartGame()
    {
        if (_started) return;
        _started = true;

        if (startUI != null) startUI.SetActive(false);
        if (startBarrier != null) startBarrier.enabled = false;
    }
}