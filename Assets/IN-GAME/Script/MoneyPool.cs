using System.Collections.Generic;
using UnityEngine;

public class MoneyPool : MonoBehaviour
{
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private int initialPoolSize = 15;
    private List<GameObject> pooledObjects;
    public static MoneyPool instance;

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
        GameObject newObj = Instantiate(moneyPrefab,transform);
        newObj.SetActive(false);
        pooledObjects.Add(newObj);
        return newObj;
    }

    // Method to get an object from the pool
    public GameObject GetMoney()
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
        obj.transform.SetParent(transform);
        obj.SetActive(false);
    }

}
