using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ZONE { INITIAL };

[System.Serializable]
public class TileInfo
{
    public ZONE zone;
    public ROAD_TYPE rType;
    public GameObject tile;
    public Vector2 dimensions = new Vector2();

    public TileInfo(int z, GameObject t)
    {
        zone = (ZONE)z;
        tile = t;
        rType = t.GetComponent<RoadData>().roadType;
        dimensions = t.GetComponent<BoxCollider2D>().size * t.transform.localScale;
    }
}

public class RoadManager : MonoBehaviour {

    [SerializeField]
    private List <TileInfo> tilesList;

    [Space(10)]
    // Path where the Resources folder is located
    public string resourcesPath;
    // Prefab folders path must end with /
    public string [] prefabFolders;

    [Space(10)]
    [SerializeField]
    private int currentZone = 0;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < prefabFolders.Length; i++)
        {
            DirectoryInfo info = new DirectoryInfo(resourcesPath + prefabFolders[i]);
            FileInfo [] fileInfo = info.GetFiles();                       

            foreach (FileInfo file in fileInfo)
            {
                if (file.ToString().Contains(".prefab") && !file.ToString().Contains(".meta"))
                {
                    string prefabPath = "" + prefabFolders[i] + file.Name.Remove(file.Name.IndexOf('.'));
                    Debug.Log(prefabPath);
                    tilesList.Add(new TileInfo(i,(GameObject)Resources.Load(prefabPath, typeof(GameObject))));
                }
                
            }
        }

        foreach (TileInfo x in tilesList)
        {
            if(x.zone == (ZONE)currentZone)
            {
                GameObject holder = Instantiate(x.tile, transform);
                Vector2 position = SpawnPosition(x);
                holder.transform.position = new Vector3(position.x, position.y, 0f);
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Vector2 SpawnPosition(TileInfo tileInfo)
    {
        Vector2 position = new Vector2();

        switch (tileInfo.rType)
        {
            case ROAD_TYPE.CENTER:
                position.x = 0;
                break;
            case ROAD_TYPE.CENTER_LEFT:
                position.x -= tileInfo.dimensions.x;
                break;
            case ROAD_TYPE.CENTER_RIGHT:
                position.x += tileInfo.dimensions.x;
                break;
            case ROAD_TYPE.SIDE_LEFT:
                position.x -= tileInfo.dimensions.x * 2;
                break;
            case ROAD_TYPE.SIDE_RIGHT:
                position.x +=  tileInfo.dimensions.x * 2;
                break;
            case ROAD_TYPE.LIMIT_LEFT:
                position.x -= tileInfo.dimensions.x * 2;
                break;
            case ROAD_TYPE.LIMIT_RIGHT:
                position.x += tileInfo.dimensions.x * 2;
                break;
            default:
                break;
        }

        return position;
    }
}
