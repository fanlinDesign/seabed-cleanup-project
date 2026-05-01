using UnityEngine;

public class SeagrassWallGate : MonoBehaviour
{
    [Header("Gate")]
    public Collider blocker;

    [Header("Goal")]
    public int neededTrash = 6;

    private int _current = 0;

    public void AddTrash(int amount = 1)
    {
        _current += amount;

        if (_current >= neededTrash)
        {
            OpenGate();
        }
    }

    private void OpenGate()
    {
        if (blocker != null)
            blocker.enabled = false;
    }
}
