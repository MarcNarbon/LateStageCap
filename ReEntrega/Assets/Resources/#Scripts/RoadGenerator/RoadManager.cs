using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TilesDictionary
{
    public int zone;
    public GameObject tile;

    public TilesDictionary(int z, GameObject t)
    {
        zone = z;
        tile = t;
    }
}

public class RoadManager : MonoBehaviour {

    [SerializeField]
    public List <TilesDictionary> tilesList;
    // Path where the Resources folder is located
    public string resourcesPath;
    // Prefab folders path must end with /
    public string [] prefabFolders;
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
                    tilesList.Add(new TilesDictionary(i,(GameObject)Resources.Load(prefabPath, typeof(GameObject))));
                }
                
            }
        }        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
