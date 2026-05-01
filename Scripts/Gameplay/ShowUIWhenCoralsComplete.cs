using UnityEngine;

public class ShowUIWhenCoralsComplete : MonoBehaviour
{
    [Header("References")]
    public CoralGroupManager group;
    public GameObject uiToShow;   // UI_CoralComplete

    private bool _shown = false;

    private void Start()
    {
        if (uiToShow != null)
            uiToShow.SetActive(false);
    }

    private void Update()
    {
        if (_shown) return;
        if (group == null || uiToShow == null) return;

        if (group.AllRevived)
        {
            Debug.Log("ALL CORALS COMPLETE -> SHOW UI");
            uiToShow.SetActive(true);
            _shown = true;
        }
    }
}
