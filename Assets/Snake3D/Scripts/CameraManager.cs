using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetGrid;
    public Camera mainCamera;
    public float aspectRatioPadding = 2f;
 
    void Start()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        Grid gridManager = targetGrid.GetComponent<Grid>();
        if (gridManager == null) return;
        
        float gridWidth = gridManager.gridWidth;
        float gridHeight = gridManager.gridHeight;
        
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        
        float verticalSize = (float)gridHeight / 2f + (float)aspectRatioPadding;
        float horizontalSize = ((float) gridWidth / 2f + (float) aspectRatioPadding) / aspectRatio;
        
        mainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }
}
