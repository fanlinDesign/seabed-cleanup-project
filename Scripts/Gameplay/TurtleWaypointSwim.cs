using System.Collections.Generic;
using UnityEngine;

public class TurtleWaypointSwim : MonoBehaviour
{
    [Header("Refs")]
    public Transform visualPivot;          // TurtleVisualPivot
    public Transform headForwardPoint;
    public Transform pathRoot;             // TurtlePath（里面放 P1(0), P1(1)...）

    [Header("Move")]
    public float moveSpeed = 0.6f;
    public float turnSpeed = 3f;
    public float arriveDistance = 0.2f;
    public bool loop = true;

    [Header("Vertical (optional)")]
    public bool verticalRoam = true;
    public float baseY = 0f;
    public float minHeightOffset = -0.8f;
    public float maxHeightOffset = 1.2f;
    public float verticalSpeed = 0.6f;

    [Header("State")]
    public bool activeSwim = false;
    public float startDelay = 0f;

    // runtime
    private readonly List<Transform> _waypoints = new List<Transform>();
    private int _index = 0;
    private float _timer = 0f;
    private float _targetY;
    private Quaternion _visualInitialLocalRot;

    private void Awake()
    {
        if (visualPivot != null)
            _visualInitialLocalRot = visualPivot.localRotation;

        RebuildWaypoints();
        PickNextY();
    }

    private void OnValidate()
    {
        moveSpeed = Mathf.Max(0f, moveSpeed);
        turnSpeed = Mathf.Max(0f, turnSpeed);
        arriveDistance = Mathf.Max(0.001f, arriveDistance);
        verticalSpeed = Mathf.Max(0f, verticalSpeed);
    }

    private void Update()
    {
        if (!activeSwim) return;

        if (startDelay > 0f)
        {
            startDelay -= Time.deltaTime;
            return;
        }

        if (_waypoints.Count == 0) return;

        Transform wp = _waypoints[_index];
        Vector3 target = wp.position;

        if (verticalRoam)
        {
            _timer += Time.deltaTime * verticalSpeed;
            float t = Mathf.PingPong(_timer, 1f);
            target.y = Mathf.Lerp(_targetY, baseY + Random.Range(minHeightOffset, maxHeightOffset), t);
        }
        else
        {
            target.y = transform.position.y;
        }

        Vector3 pos = transform.position;
        Vector3 to = target - pos;

        Vector3 toXZ = new Vector3(to.x, 0f, to.z);
        float distXZ = toXZ.magnitude;

        if (distXZ <= arriveDistance)
        {
            _index++;
            if (_index >= _waypoints.Count)
            {
                if (loop) _index = 0;
                else { activeSwim = false; }
            }
            PickNextY();
            return;
        }

        Vector3 dir = toXZ.normalized;
        transform.position = Vector3.MoveTowards(pos, new Vector3(target.x, pos.y, target.z), moveSpeed * Time.deltaTime);

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion desired = Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, desired, turnSpeed * Time.deltaTime);
        }

        if (visualPivot != null)
        {
            visualPivot.localRotation = _visualInitialLocalRot;
        }
    }

    private void PickNextY()
    {
        _targetY = baseY + Random.Range(minHeightOffset, maxHeightOffset);
        _timer = 0f;
    }

    public void SetActiveSwim(bool on)
    {
        activeSwim = on;

        if (on)
        {
            if (_waypoints.Count == 0)
                RebuildWaypoints();

            _index = Mathf.Clamp(_index, 0, Mathf.Max(0, _waypoints.Count - 1));
        }
    }

    public void RebuildWaypoints()
    {
        _waypoints.Clear();
        if (pathRoot == null) return;

        for (int i = 0; i < pathRoot.childCount; i++)
        {
            Transform c = pathRoot.GetChild(i);
            if (c != null) _waypoints.Add(c);
        }
    }
}
