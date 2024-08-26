using System.Collections;
using System.Collections.Generic;
using DungeonGeneration;
using NaughtyAttributes;
using UnityEngine;

public class BlockGridCreator : MonoBehaviour
{
    [SerializeField] private GameObject cellBlock;
    [SerializeField] private float gridCellSize = 1f;
    [SerializeField] private int height = 1;
    [SerializeField] private int width = 1;
    [SerializeField] private int depth = 1;
    [Button]
    void CreateBlock(){
        var parent = new GameObject("Room_" + height + "x" + width + "x" + depth);
        var roomDimensions = parent.AddComponent<RoomData> ();
        roomDimensions.Height = height;
        roomDimensions.Width = width;
        roomDimensions.Depth = depth;
        Vector3 cursor = Vector3.zero;
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                for(int k = 0; k < depth; k++){
                    
                    Instantiate(cellBlock, cursor, Quaternion.identity, parent.transform);
                    cursor += Vector3.forward * gridCellSize;
                }
                cursor += Vector3.up * gridCellSize;
                cursor.z = 0;
            }
            cursor += Vector3.right * gridCellSize;
            cursor.y = 0;
        }
    }
}
