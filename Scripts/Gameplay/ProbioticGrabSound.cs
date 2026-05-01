using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(AudioSource))]
public class ProbioticGrabSound : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip grabClip;
    public AudioClip releaseClip;

    private XRGrabInteractable _grab;
    private AudioSource _audio;

    private void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();
        _audio = GetComponent<AudioSource>();

        _grab.selectEntered.AddListener(OnGrab);
        _grab.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        if (_grab == null) return;
        _grab.selectEntered.RemoveListener(OnGrab);
        _grab.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Play(grabClip);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Play(releaseClip);
    }

    private void Play(AudioClip clip)
    {
        if (clip == null) return;
        _audio.PlayOneShot(clip);
    }
}
