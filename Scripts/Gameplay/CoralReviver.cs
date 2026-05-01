using System.Collections;
using UnityEngine;

public class CoralReviver : MonoBehaviour
{
    [Header("Renderer & Materials")]
    public Renderer coralRenderer;
    public Material bleachedMat;      // 白化状态材质
    public Material aliveFadeMat;     // 恢复用材质（带透明度/渐变）

    [Header("Progress")]
    [Min(1)] public int neededBalls = 1;

    [Header("Revive Effect")]
    public float reviveDuration = 0.8f;

    private int _current = 0;
    private bool _revived = false;
    public bool IsRevived => _revived;

    private Renderer _overlayRenderer;
    private Material _overlayRuntimeMat;
    private Coroutine _co;

    private void Reset()
    {
        coralRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        if (coralRenderer != null && bleachedMat != null)
            coralRenderer.sharedMaterial = bleachedMat;

        SetupOverlayRenderer();
        SetOverlayAlpha(0f);
    }

    private void SetupOverlayRenderer()
    {
        if (coralRenderer == null || aliveFadeMat == null) return;

        Transform old = transform.Find("AliveOverlay");
        if (old != null)
            Destroy(old.gameObject);

        GameObject go = new GameObject("AliveOverlay");
        go.transform.SetParent(coralRenderer.transform, false);

        _overlayRenderer = go.AddComponent<MeshRenderer>();
        MeshFilter mf = go.AddComponent<MeshFilter>();

        MeshFilter sourceMf = coralRenderer.GetComponent<MeshFilter>();
        if (sourceMf != null)
        {
            mf.sharedMesh = sourceMf.sharedMesh;
        }
        else
        {
            SkinnedMeshRenderer smr = coralRenderer.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                SkinnedMeshRenderer overlaySmr = go.AddComponent<SkinnedMeshRenderer>();
                overlaySmr.sharedMesh = smr.sharedMesh;
                overlaySmr.rootBone = smr.rootBone;
                overlaySmr.bones = smr.bones;
                overlaySmr.updateWhenOffscreen = true;

                Destroy(mf);
                Destroy(_overlayRenderer);

                _overlayRenderer = overlaySmr;
            }
        }

        _overlayRuntimeMat = new Material(aliveFadeMat);
        _overlayRenderer.sharedMaterial = _overlayRuntimeMat;

        _overlayRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _overlayRenderer.receiveShadows = false;
    }

    public void AddBall()
    {
        if (_revived) return;

        _current++;
        Debug.Log($"{gameObject.name} progress: {_current}/{neededBalls}");

        if (_current >= neededBalls)
        {
            _revived = true;
            Debug.Log($"{gameObject.name} REVIVED!");

            if (_co != null) StopCoroutine(_co);
            _co = StartCoroutine(CoRevive());
        }
    }

    // 兼容你原来的 DropZoneTrigger
    public void AddProbiotic()
    {
        AddBall();
    }

    private IEnumerator CoRevive()
    {
        if (_overlayRuntimeMat == null)
        {
            if (coralRenderer != null && aliveFadeMat != null)
                coralRenderer.sharedMaterial = aliveFadeMat;
            yield break;
        }

        float t = 0f;
        while (t < reviveDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / reviveDuration);
            SetOverlayAlpha(a);
            yield return null;
        }

        SetOverlayAlpha(1f);

        // 最后把底材也切到彩色材质，避免只靠 overlay
        if (coralRenderer != null && aliveFadeMat != null)
            coralRenderer.sharedMaterial = aliveFadeMat;
    }

    private void SetOverlayAlpha(float a)
    {
        if (_overlayRuntimeMat == null) return;

        if (_overlayRuntimeMat.HasProperty("_BaseColor"))
        {
            Color c = _overlayRuntimeMat.GetColor("_BaseColor");
            c.a = a;
            _overlayRuntimeMat.SetColor("_BaseColor", c);
        }

        if (_overlayRuntimeMat.HasProperty("_Color"))
        {
            Color c = _overlayRuntimeMat.GetColor("_Color");
            c.a = a;
            _overlayRuntimeMat.SetColor("_Color", c);
        }
    }
}
