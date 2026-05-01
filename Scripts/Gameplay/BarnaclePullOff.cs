using UnityEngine;
using UnityEngine.Events;

public class BarnaclePullOff : MonoBehaviour
{
    [Header("Setup")]
    public float pullDistance = 0.25f;
    public Transform grabbedBy; // 抓取时由外部赋值（下一步接 XR Grab）

    [Header("Events")]
    public UnityEvent onPulledOff;

    Vector3 _grabStartPos;
    bool _grabbing = false;
    bool _done = false;

    public void BeginGrab(Transform grabber)
    {
        if (_done) return;
        grabbedBy = grabber;
        _grabStartPos = grabbedBy.position;
        _grabbing = true;
    }

    public void EndGrab()
    {
        _grabbing = false;
        grabbedBy = null;
    }

    void Update()
    {
        if (_done) return;
        if (!_grabbing || grabbedBy == null) return;

        float d = Vector3.Distance(grabbedBy.position, _grabStartPos);
        if (d >= pullDistance)
        {
            _done = true;
            onPulledOff?.Invoke();
            Destroy(gameObject);
        }
    }
}