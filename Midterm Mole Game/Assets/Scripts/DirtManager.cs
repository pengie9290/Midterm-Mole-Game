using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtManager : MonoBehaviour
{
    public GameObject DirtPrefab;
    public float GridSizeX = 15;
    public float GridSizeY = 10;
    public float DirtSizeX = 0.05f;
    public float DirtSizeY = 0.05f;

    void Start()
    {
        GenerateDirt();
    }

    void Update()
    {
        
    }

    void GenerateDirt()
    {
        float StartX = transform.position.x - GridSizeX / 2;
        float StartY = transform.position.y - GridSizeY / 2;
        float EndX = transform.position.x + GridSizeX / 2;
        float EndY = transform.position.y + GridSizeY / 2;

        for (float y = StartY; y < EndY; y += DirtSizeY)
        {
            for (float x = StartX; x < EndX; x += DirtSizeX)
            {
                GameObject.Instantiate(DirtPrefab, new Vector3(x, y, transform.position.z), Quaternion.identity);
            }
        }
    }
}
