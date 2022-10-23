using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlaceInRaceHud : MonoBehaviour
{
    [SerializeField] private Color firstPlaceColor;
    [SerializeField] private Color secondPlaceColor;
    [SerializeField] private Color thirdPlaceColor;
    [SerializeField] private Color otherPlaceColor;
    
    [SerializeField] private TextMeshProUGUI placeNumber;
    [SerializeField] private TextMeshProUGUI placeText;

    private GameObject[] players = Array.Empty<GameObject>();
    private Transform finishLine;
    private Transform myKart;

    private void Awake()
    {
        placeNumber.text = "";
        placeText.text = "";
        
        RaceController.onStartRace += OnStartRace;
    }

    private void OnDestroy()
    {
        RaceController.onStartRace -= OnStartRace;
    }

    private void OnStartRace()
    {
        if (!PhotonNetwork.connected) return;
        players = GameObject.FindGameObjectsWithTag("Player");
        finishLine = FindObjectOfType<FinishLine>().transform;
        myKart = FindObjectOfType<BaseKart>().transform;
    }
    
    private void RefreshPlace()
    {
        if (!PhotonNetwork.connected) return;
        if (players.Length <= 1 || players.Length != PhotonNetwork.playerList.Length) return;

        var playerDistance = new Dictionary<Transform, float>();
        foreach (var player in players)
        {
            playerDistance.Add(player.transform, Vector3.Distance(player.transform.position, finishLine.position));
        }
        var sortedDictionary = playerDistance.OrderBy(pair => pair.Value);
        var place = 1;
        foreach (var pair in sortedDictionary)
        {
            if (pair.Key == myKart)
            {
                UpdateText(place);
            }
            place++;
        }
    }

    private void UpdateText(int place)
    {
        placeNumber.text = place.ToString();
        switch (place)
        {
            case 1:
                placeText.text = "st";
                placeText.color = firstPlaceColor;
                placeNumber.color = firstPlaceColor;
                break;
            case 2:
                placeText.text = "nd";
                placeText.color = secondPlaceColor;
                placeNumber.color = secondPlaceColor;
                break;
            case 3:
                placeText.text = "rd";
                placeText.color = thirdPlaceColor;
                placeNumber.color = thirdPlaceColor;
                break;
            default:
                placeText.text = "th";
                placeText.color = otherPlaceColor;
                placeNumber.color = otherPlaceColor;
                break;
        }
    }

    private void Update()
    {
        RefreshPlace();
    }
}
