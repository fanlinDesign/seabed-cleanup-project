using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRGrabInteractable))]
public class DebrisSfx : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip grabClip;
    public AudioClip releaseClip; // 可选：松手音效，不想要就留空

    [Header("Volume")]
    [Range(0f, 1f)] public float grabVolume = 1f;
    [Range(0f, 1f)] public float releaseVolume = 1f;

    private AudioSource _audio;
    private XRGrabInteractable _grab;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _grab = GetComponent<XRGrabInteractable>();

        // 推荐：避免脚本被复制后漏设置
        _audio.playOnAwake = false;
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

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (grabClip != null)
            _audio.PlayOneShot(grabClip, grabVolume);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (releaseClip != null)
            _audio.PlayOneShot(releaseClip, releaseVolume);
    }
}
