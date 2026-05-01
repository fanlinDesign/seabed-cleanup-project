using UnityEngine;

public class CoralGroupManager : MonoBehaviour
{
    [Header("All corals that must be revived")]
    public CoralReviver[] corals;

    public bool AllRevived
    {
        get
        {
            if (corals == null || corals.Length == 0) return false;

            for (int i = 0; i < corals.Length; i++)
            {
                if (corals[i] == null) return false;
                if (!corals[i].IsRevived) return false;
            }
            return true;
        }
    }
}
