using UnityEngine;

[CreateAssetMenu(fileName = "MapDataCollection", menuName = "Map/Map Data Collection")]
public class MapDataCollection : ScriptableObject
{
    public MapData[] maps;
    
    public static int GetMapId(MapData mapToGet)
    {
        for (var i = 0; i < Instance.maps.Length; i++)
        {
            var agent = Instance.maps[i];
            if (agent == mapToGet)
                return i;
        }
        return 0;
    }
    
    public static MapData GetMapObject(int id)
    {
        var map = Instance.maps[0];
        if (Instance.maps[id] != null)
        {
            map = Instance.maps[id];
        }

        return map;
    }
    
    private static MapDataCollection _instance;
    public static MapDataCollection Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Resources.Load<MapDataCollection>("MapDataCollection");
            return _instance;
        }
    }
}
