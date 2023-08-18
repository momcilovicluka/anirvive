using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTreeColliders : MonoBehaviour
{
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        foreach (TreeInstance tree in terrainData.treeInstances)
        {
            Vector3 worldPos = Vector3.Scale(tree.position, terrainData.size) + terrain.transform.position;

            GameObject treeColliderObj = new GameObject("TreeCollider");
            treeColliderObj.transform.position = worldPos;
            CapsuleCollider collider = treeColliderObj.AddComponent<CapsuleCollider>();
            collider.height = tree.heightScale;
        }
    }
}
