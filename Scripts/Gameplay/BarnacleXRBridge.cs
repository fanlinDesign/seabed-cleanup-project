using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class BarnacleXRBridge : MonoBehaviour
{
    public BarnaclePullOff pullOff;
    public BarnacleFollowPoint follow;

    XRGrabInteractable _grab;

    void Reset()
    {
        pullOff = GetComponent<BarnaclePullOff>();
        follow = GetComponent<BarnacleFollowPoint>();
    }

    void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();
        if (pullOff == null) pullOff = GetComponent<BarnaclePullOff>();
        if (follow == null) follow = GetComponent<BarnacleFollowPoint>();

        // 绑定抓取事件
        _grab.selectEntered.AddListener(OnGrab);
        _grab.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        if (_grab == null) return;
        _grab.selectEntered.RemoveListener(OnGrab);
        _grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // 抓住时：停止贴点位，否则会抖
        if (follow != null) follow.enabled = false;

        // 让 pullOff 记录“开始抓取时的位置”
        if (pullOff != null)
            pullOff.BeginGrab(args.interactorObject.transform);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (pullOff != null) pullOff.EndGrab();

        // 松手时：如果还没拔掉，就继续贴回点位
        if (follow != null) follow.enabled = true;
    }
}