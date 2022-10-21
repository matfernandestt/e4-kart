using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public void FinishRace(PhotonPlayer player)
    {
        if (RaceController.GetCustomProperty(RaceController.CustomProperty_RaceWinner) != null) return;
        
        RaceController.SetCustomProperty(RaceController.CustomProperty_RaceWinner, player.NickName);
        RaceController.onFinishRace?.Invoke();
    }

    public void FixPosition()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 10f))
        {
            var road = hit.collider.GetComponent<Road>();
            if (road != null)
            {
                transform.position = hit.point;
                transform.up = hit.normal;
            }
        }
    }
}
