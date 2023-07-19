using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode //개념적으로만 존재하여 monobehavior를 상속받지 않는다 따라서 Gameobject같은것들이 없다.
                       // 여기서 Gameobject가 없다는뜻은 AstarNode가 따로 오브젝트가 되서 1:1로 대응하는 transform같은것이 없다는 것이다.
                       // 여기서 AstarNode는 F값을 저장하는 용도로 스크립트에서 사용될 예정
{
    public TerrainController Terrain { get; private set; }
    public GameObject DestinationObj { get; private set; } //두개의 성중 하나의 인스턴스를 받아오기 위해 선언

    // Astar algorithm
    public float AstarF { get; private set; } = float.MaxValue; //G+H
    public float AstarG { get; private set; } = float.MaxValue; //거리
    public float AstarH { get; private set; } = float.MaxValue; //휴리스틱
    public AstarNode AstarPrevNode { get; private set; } = default; //부모노드

    public AstarNode(TerrainController terrain_, GameObject destinationObj_)
    {
        Terrain = terrain_;
        DestinationObj = destinationObj_;
    } // AstarNode()

    //! Astar 알고리즘에 사용할 비용을 설정한다.
    public void UpdateCost_Astar(float gCost, float heuristic, AstarNode prevNode)
    {
        float aStarF = gCost + heuristic;

        if(aStarF < AstarF)
        {
            AstarG = gCost;
            AstarH = heuristic;
            AstarF = aStarF;

            AstarPrevNode = prevNode;
        } //if : 새로 계산한 비용이 더 작은 경우에만 업데이트 한다.
        else { /* Do nothing */}
    } //UpdateCost_Astar()

    //! 설정한 비용을 출력한다.
    public void ShowCost_Astar()
    {
        GFunc.Log($"TileIdx1D: {Terrain.TileIdx1D},"+ $"F: {AstarF}, G: {AstarG}, H: {AstarH}");
    } //ShowCost_Astar()
}
