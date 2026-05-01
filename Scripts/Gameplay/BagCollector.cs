using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class BagCollector : MonoBehaviour
{
    [Header("Gate")]
    public SeagrassWallGate gate;

    [Header("Collect")]
    public bool destroyOnCollect = true;

    [Header("Audio")]
    public AudioSource collectAudio;
    public AudioClip collectClip;
    [Range(0f, 1f)] public float collectVolume = 1f;

    [Header("Bag Visual Contents")]
    public GameObject bottleVisualPrefab;
    public Transform contentsRoot;
    public int maxVisualBottles = 12;

    [Tooltip("每收集一个，往上叠一点点（本地坐标）")]
    public Vector3 itemLocalOffset = new Vector3(0f, 0.02f, 0f);

    [Tooltip("随机散布范围（本地坐标）")]
    public Vector3 itemLocalJitter = new Vector3(0.03f, 0.02f, 0.03f);

    [Tooltip("袋内展示瓶子的统一缩放（建议 0.15~0.4）")]
    [Range(0.05f, 1f)] public float visualScale = 0.3f;

    private XRSocketInteractor _socket;
    private int _visualCount = 0;

    private void Awake()
    {
        _socket = GetComponent<XRSocketInteractor>();

        if (collectAudio == null)
            collectAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _socket.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        _socket.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        var t = args.interactableObject.transform;

        var tag = t.GetComponentInParent<DebrisTag>();
        if (tag == null) return;

        if (tag.collected) return;
        tag.collected = true;

        if (collectAudio != null && collectClip != null)
            collectAudio.PlayOneShot(collectClip, collectVolume);

        gate?.AddTrash(1);

        SpawnVisualBottle();

        if (destroyOnCollect)
        {
            _socket.interactionManager?.SelectExit(_socket, args.interactableObject);
            Destroy(tag.gameObject);
        }
    }

    private void SpawnVisualBottle()
    {
        if (bottleVisualPrefab == null || contentsRoot == null) return;
        if (_visualCount >= maxVisualBottles) return;

        Vector3 jitter = new Vector3(
            Random.Range(-itemLocalJitter.x, itemLocalJitter.x),
            Random.Range(-itemLocalJitter.y, itemLocalJitter.y),
            Random.Range(-itemLocalJitter.z, itemLocalJitter.z)
        );

        Vector3 localPos = (_visualCount * itemLocalOffset) + jitter;

        var go = Instantiate(bottleVisualPrefab, contentsRoot);
        go.transform.localPosition = localPos;
        go.transform.localRotation = Random.rotation;
        go.transform.localScale = Vector3.one * visualScale;

        _visualCount++;
    }
}
