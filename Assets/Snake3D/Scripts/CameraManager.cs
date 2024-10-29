using UnityEngine;

namespace Snake3D
{
    public class CameraManager : MonoBehaviour
    {
        public Transform targetGrid;
        public Camera mainCamera;
        public float aspectRatioPadding = 2f;

        private void Start()
        {
            AdjustCameraSize();
        }

        private void AdjustCameraSize()
        {
            var gridManager = targetGrid.GetComponent<Grid.Grid>();
            if (gridManager == null) return;

            float gridWidth = gridManager.gridWidth;
            float gridHeight = gridManager.gridHeight;

            var aspectRatio = Screen.width / (float)Screen.height;

            var verticalSize = gridHeight / 2f + aspectRatioPadding;
            var horizontalSize = (gridWidth / 2f + aspectRatioPadding) / aspectRatio;

            mainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
    }
}