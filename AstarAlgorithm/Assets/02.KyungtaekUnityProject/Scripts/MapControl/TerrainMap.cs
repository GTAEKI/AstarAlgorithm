using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMap : TileMapController
{
    private const string TERRAIN_TILEMAP_OBJ_NAME = "Terrain Tilemap";

    private Vector2Int mapCellSize = default;
    private Vector2 mapCellGap = default;

    private List<TerrainController> allTerrains = default;

    //! Awake 타임에 초기화 할 내용을 재정의한다.
    public override void InitAwake(MapBoard mapControler_)
    {
        tileMapObjName = TERRAIN_TILEMAP_OBJ_NAME;
        base.InitAwake(mapControler_);

        allTerrains = new List<TerrainController>();

        //{타일의 x축 개수와 전체 타일의 수로 맵의 가로, 세로 사이즈를 연산한다.
        mapCellSize = Vector2Int.zero;
        float tempTileY = allTileObjs[0].transform.localPosition.y;
        for (int i = 0; i < allTileObjs.Count; i++)
        {
            if (tempTileY.IsEquals(allTileObjs[i].transform.localPosition.y) == false)
            {
                mapCellSize.x = i;
                break;
            } // if: 첫번째 타일의 y 좌표와 달라지는 지점 전까지가 맵의 가로 셀 크기이다.라고 전제한것이다
        }

        // 전체 타일의 수를 맵의 가로 셀 크기로 나눈 값이 맵의 세로 셀 크기이다.
        mapCellSize.y = Mathf.FloorToInt(allTileObjs.Count / mapCellSize.x);

        //} 타일의 x축 개수와 전체 타일의 수로 맵의 가로, 세로 사이즈를 연산한다.

        //{ x축 상의 두 타일, y축 상의 두 타일 사이의 로컬 포지션으로 타일 갭을 연산한다.
        mapCellGap = Vector2.zero;
        mapCellGap.x = allTileObjs[1].transform.localPosition.x - allTileObjs[0].transform.localPosition.x;
        mapCellGap.y = allTileObjs[mapCellSize.x].transform.localPosition.y - allTileObjs[0].transform.localPosition.y;
        //} x축 상의 두 타일, y축 상의 두 타일 사이의 로컬 포지션으로 타일 갭을 연산한다.
    } //InitAwake();

    private void Start()
    {
        //{타일맵의 일부를 일정 확률로 다른 타일로 교체하는 로직
        GameObject changeTilePrefab = ResManager.Instance.terrainPrefabs[RDefine.TERRAIN_PREF_OCEAN];

        //타일맵 중에 어느 정도를 바다로 교체할 것인지 결정한다.
        const float CHANGE_PERCENTAGE = 15.0f;
        float correctChangePercentage = allTileObjs.Count * (CHANGE_PERCENTAGE / 100.0f);

        //바다로 교체할 타일의 정보를 리스트 형태로 생성해서 섞는다.
        List<int> changedTileResult = GFunc.CreateList(allTileObjs.Count, 1);
        changedTileResult.Shuffle();

        GameObject tempChangeTile = default;
        for(int i = 0; i<allTileObjs.Count; i++)
        {
            if(correctChangePercentage <= changedTileResult[i]) { continue; }

            //프리펩을 인스턴스화 해서 교체할 타일의 트랜스폼을 복사한다.
            tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);

            tempChangeTile.name = changeTilePrefab.name;
            tempChangeTile.SetLocalScale(allTileObjs[i].transform.localScale);
            tempChangeTile.SetLocalPos(allTileObjs[i].transform.localPosition);

            allTileObjs.Swap(ref tempChangeTile,i);
            tempChangeTile.DestroyObj();

        } //loop : 위에서 연산한 정보로 현재 타일맵에 바다를 적용하는 루프
          //}타일맵의 일부를 일정 확률로 다른 타일로 교체하는 로직

        //{ 기존에 존재하는 타일의 순서를 조정하고, 컨트롤러를 캐싱하는 로직 (캐싱 : 인스턴스화된 어떤것을 데리고와서 메모리에 할당하는것을 캐싱이라고 한다.)
        TerrainController tempTerrain = default;
        TerrainType terrainType = TerrainType.NONE;

        int loopCnt = 0;
        foreach(GameObject tile_ in allTileObjs)
        {
            tempTerrain = tile_.GetComponentMust<TerrainController>();
            switch (tempTerrain.name)
            {
                case RDefine.TERRAIN_PREF_PLAIN:
                    terrainType = TerrainType.PLAIN_PASS;
                    break;
                case RDefine.TERRAIN_PREF_OCEAN:
                    terrainType = TerrainType.OCEAN_N_PASS;
                    break;
                default:
                    terrainType = TerrainType.NONE;
                    break;
            }//switch: 지형별로 다른 설정을 한다.

            // TODO: tempTerrain Setup 함수 필요함.
            tempTerrain.transform.SetAsFirstSibling();
            allTerrains.Add(tempTerrain);
            loopCnt += 1;
        }// loop: 타일의 이름과 랜더링 순서대로 정렬하는 루프
        //} 기존에 존재하는 타일의 순서를 조정하고, 컨트롤러를 캐싱하는 로직
    } // Start();


    //! 초기화된 타일의 정보로 연산한 맵의 가로, 세로 크기를 리턴하는 함수
    public Vector2Int GetCellSize() { return mapCellSize; }

    //! 초기화된 타일의 정보로 연산한 타일 사이의 갭을 리턴하는 함수.
    public Vector2 GetCellGap() { return mapCellGap; }

    //! 인덱스에 해당하는 타일을 리턴하는 함수
    public TerrainController GetTile(int tileIdx1D)
    {
        if (allTerrains.IsValid(tileIdx1D))
        {
            return allTerrains[tileIdx1D];
        }
        return default;
    } // GetTile();
} 
