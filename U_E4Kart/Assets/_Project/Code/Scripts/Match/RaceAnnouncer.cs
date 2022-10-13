using UnityEngine;

public class RaceAnnouncer : MonoBehaviour
{
    public void CountdownEnd()
    {
        RaceController.onStartRace?.Invoke();
    }
}
