using UnityEngine;

public class CoralCompleteUI : MonoBehaviour
{
    [Header("References")]
    public CoralReviver coral;
    public GameObject uiToShow;

    private bool _shown = false;

    private void Start()
    {
        if (uiToShow != null)
            uiToShow.SetActive(false);
    }

    private void Update()
    {
        if (_shown) return;
        if (coral == null || uiToShow == null) return;

        if (coral.IsRevived)
        {
            uiToShow.SetActive(true);
            _shown = true;
        }
    }
}
