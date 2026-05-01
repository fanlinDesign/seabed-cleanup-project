using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class ProbioticSwimMotion : MonoBehaviour
{
    [Header("Bobbing")]
    public float amplitude = 0.04f;
    public float frequency = 0.7f;

    [Header("Drift")]
    public float driftRadius = 0.03f;
    public float driftSpeed = 0.35f;

    [Header("After Release")]
    public float freeMoveTime = 0.25f;

    private Rigidbody _rb;
    private XRGrabInteractable _grab;

    private Vector3 _basePos;
    private float _seed;
    private bool _held;

    private Coroutine _returnRoutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _grab = GetComponent<XRGrabInteractable>();

        _seed = Random.Range(0f, 100f);

        _rb.useGravity = false;

        _grab.selectEntered.AddListener(OnGrab);
        _grab.selectExited.AddListener(OnRelease);
    }

    private void Start()
    {
        // 确保初始漂浮中心正确
        _basePos = transform.position;
        EnterFloatMode();
    }

    private void OnDestroy()
    {
        if (_grab == null) return;
        _grab.selectEntered.RemoveListener(OnGrab);
        _grab.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        _held = true;

        if (_returnRoutine != null) StopCoroutine(_returnRoutine);

        // 抓住：交给XRI控制
        _rb.isKinematic = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        _held = false;

        // 放手瞬间更新漂浮中心
        _basePos = _rb.position;

        if (_returnRoutine != null) StopCoroutine(_returnRoutine);
        _returnRoutine = StartCoroutine(ReturnToFloatAfterDelay());
    }

    private IEnumerator ReturnToFloatAfterDelay()
    {
        yield return new WaitForSeconds(freeMoveTime);

        _basePos = _rb.position;

        EnterFloatMode();
    }

    private void EnterFloatMode()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _rb.position = _basePos;

        _rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (_held) return;
        if (!_rb.isKinematic) return;

        float t = Time.time + _seed;

        float y = Mathf.Sin(t * Mathf.PI * 2f * frequency) * amplitude;
        float dx = Mathf.Cos(t * driftSpeed) * driftRadius;
        float dz = Mathf.Sin(t * driftSpeed) * driftRadius;

        Vector3 target = _basePos + new Vector3(dx, y, dz);
        _rb.MovePosition(target);
    }
}
