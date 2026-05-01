using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropZoneTrigger : MonoBehaviour
{
    [Header("Target Coral")]
    public CoralReviver coral;

    [Header("Consume Behavior")]
    public bool destroyBallOnPlace = true;

    [Header("Audio (played from DropZone so it won't be cut off)")]
    public AudioSource placeAudio;
    public AudioClip placeClip;
    [Range(0f, 1f)] public float placeVolume = 1f;

    private void Reset()
    {
        coral = GetComponentInParent<CoralReviver>();
        placeAudio = GetComponent<AudioSource>();

        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (coral != null && coral.IsRevived) return;

        ProbioticTag probiotic = other.GetComponentInParent<ProbioticTag>();
        if (probiotic == null) return;

        if (probiotic.consumed) return;
        probiotic.consumed = true;

        if (placeAudio != null && placeClip != null)
        {
            placeAudio.PlayOneShot(placeClip, placeVolume);
        }

        if (coral != null)
        {
            coral.AddProbiotic();
        }

        if (destroyBallOnPlace)
        {
            Destroy(probiotic.gameObject);
        }
    }
}
