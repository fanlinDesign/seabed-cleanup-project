using UnityEngine;

public class TurtleBarnacleLevel : MonoBehaviour
{
    [Header("Refs")]
    public TurtleWaypointSwim turtleSwim;

    [Header("Barnacles")]
    public int needed = 3;
    [SerializeField] int current = 0;

    public void OnBarnaclePulled()
    {
        current++;

        if (current >= needed)
        {
            if (turtleSwim != null)
                turtleSwim.activeSwim = true;   // 🔥 直接改成这样
        }
    }
}