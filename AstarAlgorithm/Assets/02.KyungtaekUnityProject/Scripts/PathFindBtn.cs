using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindBtn : MonoBehaviour
{
    //! A star find 버튼을 누른 경우
    public void OnClickAstarFindBtn()
    {
        PathFinder.Instance.FindPath_Astar();
    } //OnClickAstarFindBtn()
}
