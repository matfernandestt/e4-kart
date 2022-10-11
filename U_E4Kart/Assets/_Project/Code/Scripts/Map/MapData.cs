using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Map/Map Data")]
public class MapData : ScriptableObject
{
    public string mapName;
    public Sprite mapIcon;
    public GameMap gameMapPrefab;

    public string prefabName => gameMapPrefab.gameObject.name;
}
