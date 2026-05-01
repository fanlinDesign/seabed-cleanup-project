using UnityEngine;

public class AutoStartGame : MonoBehaviour
{
    public GameObject startUI;
    public GameObject startBarrier;
    public float delay = 3f;

    void Start()
    {
        if (startBarrier != null)
            startBarrier.SetActive(true);

        Invoke(nameof(StartGame), delay);
    }

    void StartGame()
    {
        if (startUI != null)
            startUI.SetActive(false);

        if (startBarrier != null)
            startBarrier.SetActive(false);
    }
}