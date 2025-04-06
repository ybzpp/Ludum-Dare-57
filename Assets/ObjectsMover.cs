using System.Collections.Generic;
using UnityEngine;

public class ObjectsMover : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;
    public float Speed = 5f;
    public float DistanceBetweenObjects = 2f;
    public List<GameObject> Objects = new List<GameObject>();

    private List<bool> objectsMoving;
    private List<float> distancesCovered;

    private void OnEnable()
    {
        InitializeObjects();
    }

    private void InitializeObjects()
    {
        objectsMoving = new List<bool>();
        distancesCovered = new List<float>();

        for (int i = 0; i < Objects.Count; i++)
        {
            Objects[i].transform.position = StartPoint.position;
            objectsMoving.Add(i == 0); // Only first object starts moving
            distancesCovered.Add(0f);
        }
    }

    private void Update()
    {
        for (int i = 0; i < Objects.Count; i++)
        {
            if (objectsMoving[i])
            {
                MoveObject(i);
                CheckDistanceToNextObject(i);
            }
        }
    }

    private void MoveObject(int index)
    {
        GameObject obj = Objects[index];

        // Move object towards end point
        float step = Speed * Time.deltaTime;
        obj.transform.position = Vector3.MoveTowards(obj.transform.position, EndPoint.position, step);

        // Update distance covered
        distancesCovered[index] = Vector3.Distance(StartPoint.position, obj.transform.position);

        // Check if reached end point
        if (Vector3.Distance(obj.transform.position, EndPoint.position) < 0.01f)
        {
            obj.transform.position = StartPoint.position;
            distancesCovered[index] = 0f;
        }
    }

    private void CheckDistanceToNextObject(int currentIndex)
    {
        // If this is not the last object
        if (currentIndex < Objects.Count - 1)
        {
            // Check if current object has moved far enough to activate next one
            if (distancesCovered[currentIndex] >= DistanceBetweenObjects && !objectsMoving[currentIndex + 1])
            {
                objectsMoving[currentIndex + 1] = true;
            }
        }
    }
}