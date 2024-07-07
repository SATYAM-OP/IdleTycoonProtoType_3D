using System.Collections.Generic;
using UnityEngine;

public class CustomerPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int initialPoolSize = 10;
    public Transform customerSpawnPoint;
    private List<GameObject> pooledObjects;
    public static CustomerPool instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the pool
        pooledObjects = new List<GameObject>();

        // Create initial objects
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewObject();
        }
    }

    // Method to create a new object
    private GameObject CreateNewObject()
    {
        GameObject newObj = Instantiate(objectPrefab,customerSpawnPoint.position,Quaternion.identity, transform);
        newObj.SetActive(false);
        pooledObjects.Add(newObj);
        return newObj;
    }

    // Method to get an object from the pool
    public GameObject GetObject()
    {
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // If no inactive objects, create a new one
        return CreateNewObject();
    }

    // Method to return an object to the pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
