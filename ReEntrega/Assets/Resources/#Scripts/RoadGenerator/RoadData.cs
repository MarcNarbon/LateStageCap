using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROAD_TYPE { CENTER, CENTER_LEFT, CENTER_RIGHT, SIDE_LEFT, SIDE_RIGHT, LIMIT_LEFT, LIMIT_RIGHT };

public class RoadData : MonoBehaviour
{
    public ROAD_TYPE roadType;
}
