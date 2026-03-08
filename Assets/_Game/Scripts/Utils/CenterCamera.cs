using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    private Camera camera;
    [Header("Grid Settings")]
    [SerializeField] private int row = 4;
    [SerializeField] private int column = 3;
    [SerializeField] private float spacing = 1.6f;
    // Start is called before the first frame update
    void Start()
    {
        Center();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Center()
    {
        float offsetX = (column - 1) * spacing / 2f;
        float offsetY = -(row - 1) * spacing / 2f;
        int offsetZ = -10;
        camera.transform.position = new Vector3(offsetX, offsetY, offsetZ);
    }
}
