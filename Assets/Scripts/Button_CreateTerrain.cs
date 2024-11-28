using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Button_CreateTerrain : MonoBehaviour
{
    public GameObject Terrain;
    int timesclicked;
    int xOffset = 0;
    int zOffset = -100;
    int xIncrement;
    int number = 1;



    public void CreateTerrain()
    {
        xIncrement = Terrain.GetComponent<MeshGenerator>().gridX;


        Vector3 newPosition = Terrain.transform.position;
        int isEvenOrOdd = number % 2;


        switch (isEvenOrOdd)
        {
            case 0:
                Instantiate(Terrain, new Vector3(newPosition.x + xOffset, newPosition.y, newPosition.z), Quaternion.identity);
                Debug.Log("yeah");
                break;
            case 1:
                xOffset += xIncrement;
                Instantiate(Terrain, new Vector3(newPosition.x + xOffset, newPosition.y, newPosition.z += zOffset), Quaternion.identity);
                break;
            
        }
        number++;
    }
}
