using TMPro;
using UnityEngine;

public class ScoreboardPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeText;
    [SerializeField] private TextMeshProUGUI nicknameText;

    public void Setup(int place, string nickname)
    {
        UpdatePlace(place);
        nicknameText.text = nickname;
    }

    public void UpdatePlace(int place)
    {
        switch (place)
        {
            case 1:
                placeText.text = $"{place}st";
                break;
            case 2:
                placeText.text = $"{place}nd";
                break;
            case 3:
                placeText.text = $"{place}rd";
                break;
            default:
                placeText.text = $"{place}th";
                break;
        }
    }
}
