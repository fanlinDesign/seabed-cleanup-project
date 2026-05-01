using UnityEngine;

public class BarnacleFollowPoint : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 localPosOffset;
    public Vector3 localEulerOffset;

    void LateUpdate()
    {
        if (followTarget == null) return;

        transform.position = followTarget.TransformPoint(localPosOffset);
        transform.rotation = followTarget.rotation * Quaternion.Euler(localEulerOffset);
    }
}