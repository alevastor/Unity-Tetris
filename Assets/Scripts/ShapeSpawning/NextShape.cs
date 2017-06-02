using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextShape : MonoBehaviour
{
    static ShapeSpawner spawner;
    static GameObject[] shapes;

    void Start()
    {
        spawner = GameObject.FindObjectOfType<ShapeSpawner>();
        shapes = new GameObject[spawner.shapes.Length];
        // Instantiate all shapes
        for (int i = 0; i < spawner.shapes.Length; i++)
        {
            shapes[i] = Instantiate(spawner.shapes[i], transform.position, Quaternion.identity, transform);
        }
    }

    public static void NewShape()
    {
        // turn off all shapes
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].SetActive(false);
        }
        // turn on and rotate next shape, taken from ShapeSpawner class
        shapes[spawner.randomShape].SetActive(true);
        shapes[spawner.randomShape].transform.rotation = Quaternion.Euler(0, 0, spawner.randomRotation);
    }
}
