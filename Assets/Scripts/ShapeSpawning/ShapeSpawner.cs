using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public GameObject[] shapes;
    public int randomShape;
    public int randomRotation;

    void Awake()
    {
        randomShape = Random.Range(0, shapes.Length);
        randomRotation = Random.Range(0, 4) * 90;
    }

    public void SpawnNext()
    {
        // create child object with prefab position adjacement (for O shape rotation)
        Instantiate(shapes[randomShape], transform.position + shapes[randomShape].transform.position, Quaternion.Euler(0, 0, randomRotation), transform);
        // chose random next shape
        randomShape = Random.Range(0, shapes.Length);
        // chose random next shape rotation
        randomRotation = Random.Range(0, 4) * 90;
        // visaully display next shape
        NextShape.NewShape();
    }
}
