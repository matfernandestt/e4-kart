using UnityEngine;
using UnityEngine.UI;

public class MapSelectionScreen : MonoBehaviour
{
    [SerializeField] private Button[] closeMapSelectionButton;
    
    private SelectableMap[] selectableMaps;

    private void Awake()
    {
        RefreshMapSelectionScreen();
    }

    public void RefreshMapSelectionScreen()
    {
        selectableMaps ??= GetComponentsInChildren<SelectableMap>(true);

        foreach (var button in closeMapSelectionButton)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(CloseMenu);
        }

        foreach (var selectableMap in selectableMaps)
        {
            selectableMap.onSelectMap = OnSelectMap;
            selectableMap.SetSelected(selectableMap.GetToggleOn);
            if(selectableMap.GetToggleOn)
                OnSelectMap(selectableMap.GetMapData, selectableMap);
        }
    }

    private void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    private void OnSelectMap(MapData data, SelectableMap map)
    {
        foreach (var selectableMap in selectableMaps)
        {
            if(selectableMap != map)
                selectableMap.SetSelected(false);
        }
        GlobalSettingsData.Instance.SetChosenMap(data);
    }
}