using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class DebrisFreeze : MonoBehaviour
{
    private Rigidbody _rb;
    private XRGrabInteractable _grab;

    [Header("Start State")]
    public bool startFrozen = true;     // �����Ƿ񶳽�
    public bool disableGravity = true;  // ˮ�½��������

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _grab = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        _grab.selectEntered.AddListener(OnGrab);
        _grab.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        _grab.selectEntered.RemoveListener(OnGrab);
        _grab.selectExited.RemoveListener(OnRelease);
    }

    private void Start()
    {
        if (disableGravity) _rb.useGravity = false;

        if (startFrozen)
        {
            _rb.isKinematic = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        _rb.isKinematic = false;  // ץ���������������Ͷ����
        if (disableGravity) _rb.useGravity = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // ���ֺ󱣳ַ�Kinematic���������ܱ��ӡ��������������������
        _rb.isKinematic = false;
        if (disableGravity) _rb.useGravity = false;
    }
}
