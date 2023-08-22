using System.Collections.Generic;
using UnityEngine;

public class AddTrees : MonoBehaviour
{
    public GameObject theTree;

    // Use this for initialization
    private void Start()
    {
        TerrainData terrain;
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData;

        foreach (TreeInstance tree in terrain.treeInstances)
        {
            if (tree.prototypeIndex == 1)
            {
                Vector3 worldTreePos = Vector3.Scale(tree.position, terrain.size) + Terrain.activeTerrain.transform.position;
                GameObject treeInstance = Instantiate(theTree, worldTreePos, Quaternion.identity);
            }
        }

        List<TreeInstance> newTrees = new List<TreeInstance>(0);
        terrain.treeInstances = newTrees.ToArray();
    }
}