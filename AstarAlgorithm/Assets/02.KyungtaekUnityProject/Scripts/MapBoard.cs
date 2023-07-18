using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoard : MonoBehaviour
{

    private const string TERRAIN_MAP_OBJ_NAME = "TerrainGrid";

    public Vector2Int MapCellSize { get; private set; } = default;

    public Vector2 MapCellGap { get; private set; } = default;

    private TerrainMap terrainMap = default;

    private void Awake()
    {
        //{ 매니저 스크립트를 초기화한다.
        ResManager.Instance.Create();
        //} 매니저 스크립트를 초기화한다.

        //{ 맵의 지형을 초기화하여 배치한다.
        terrainMap = gameObject.FindChildComponent<TerrainMap>(TERRAIN_MAP_OBJ_NAME);
        terrainMap.InitAwake(this);
        MapCellSize = terrainMap.GetCellSize();
        MapCellGap = terrainMap.GetCellGap();
        //} 맵의 지형을 초기화하여 배치한다.


    }
}
