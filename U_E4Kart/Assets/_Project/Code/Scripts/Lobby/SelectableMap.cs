using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableMap : MonoBehaviour
{
    [SerializeField] private MapData data;
    [SerializeField] private Toggle thisToggle;
    [SerializeField] private TextMeshProUGUI mapNameText;
    [SerializeField] private Image mapIcon;

    public Action<MapData, SelectableMap> onSelectMap;

    private void Awake()
    {
        thisToggle.onValueChanged.AddListener(OnChangeToggleValue);

        mapNameText.text = data.mapName;
        mapIcon.sprite = data.mapIcon;
    }

    public MapData SetSelected(bool selected)
    {
        thisToggle.isOn = selected;
        return data;
    }

    public bool GetToggleOn => thisToggle.isOn;

    public MapData GetMapData => data;

    private void OnChangeToggleValue(bool value)
    {
        if (value)
        {
            onSelectMap?.Invoke(data, this);
        }
    }
}
