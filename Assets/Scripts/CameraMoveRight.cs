using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveRight : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject TerrainX;
    int xIncrement;
    Vector3 newPosition;


    public void MoveCameraRight()
    {
        newPosition = mainCamera.transform.position;
        newPosition.x += xIncrement = TerrainX.GetComponent<MeshGenerator>().gridX;
        mainCamera.transform.position = newPosition;
    }

    public void MoveCameraLeft()
    {
        newPosition = mainCamera.transform.position;
        newPosition.x -= xIncrement = TerrainX.GetComponent<MeshGenerator>().gridX;
        mainCamera.transform.position = newPosition;
    }
}
