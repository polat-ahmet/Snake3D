using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetGrid; // The grid that the camera will focus on
    public Camera mainCamera;    // Reference to the main camera
    public float aspectRatioPadding = 2f; // Padding to ensure the grid is fully visible
    // [SerializeField] private float borderSize;
    
    void Start()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        // Assuming the grid object has been centered
        Grid gridManager = targetGrid.GetComponent<Grid>();
        if (gridManager == null) return;

        // Get grid width and height
        float gridWidth = gridManager.gridWidth;
        float gridHeight = gridManager.gridHeight;

        // Get the screen's aspect ratio (width/height)
        float aspectRatio = (float)Screen.width / (float)Screen.height;

        // To fit the grid within the camera's view, calculate orthographic size based on the grid dimensions
        // float cameraSizeBasedOnWidth = gridWidth / 2f / aspectRatio; // Width-based size
        // float cameraSizeBasedOnHeight = gridHeight / 2.0f;            // Height-based size
        //
        // // Set the camera's orthographic size to the largest required value (to fit both width and height)
        // mainCamera.orthographicSize = Mathf.Max(cameraSizeBasedOnWidth, cameraSizeBasedOnHeight);
        //
        
        //
        float verticalSize = (float)gridHeight / 2f + (float)aspectRatioPadding;
        float horizontalSize = ((float) gridWidth / 2f + (float) aspectRatioPadding) / aspectRatio;
        
        mainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }
}
