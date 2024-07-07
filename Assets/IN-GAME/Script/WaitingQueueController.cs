using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaitingQueueController : MonoBehaviour
{
    public List<Customer> customers;
    public List<Transform> waitingQueuePositions;
    private Vector3 enterancePosition;
    [SerializeField] Vector2 newCustomerSpawnTime;
    [SerializeField] private float spawnInterval = 2f; // Time interval between spawns

    private void Awake()
    {
        customers = new List<Customer>();
        waitingQueuePositions.Sort((a, b) => b.transform.position.z.CompareTo(a.transform.position.z));
        enterancePosition = waitingQueuePositions[waitingQueuePositions.Count - 1].position;
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    private IEnumerator SpawnCustomer()
    {
        for (int i = 0; i < waitingQueuePositions.Count; i++)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (CanAddCustomerToQueue())
            {
                Customer newCustomer = CustomerPool.instance.GetObject().GetComponent<Customer>();
                AddCustomerToQueue(newCustomer);
            }
        }
    }


    public bool CanAddCustomerToQueue()
    {
        return customers.Count < waitingQueuePositions.Count;
    }

    public void AddCustomerToQueue(Customer customer)
    {
        customers.Add(customer);
        customer.gameObject.SetActive(true);
        customer.MoveTo(enterancePosition);
        ArrangeCustomer();
    }

    public void ArrangeCustomer()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            Customer customer = customers[i];
            if (i >= waitingQueuePositions.Count)
            {
                break;
            }
            customer.MoveTo(waitingQueuePositions[i].position);
        }
    }


    public void RemoveCustomerFromQueue(Customer customer)
    {
        if (customers.Contains(customer))
        {
            customers.Remove(customer);
        }
        StartCoroutine(AddNewCustomer());
        ArrangeCustomer();
    }


    private IEnumerator AddNewCustomer()
    {
        yield return new WaitForSeconds(Random.Range(newCustomerSpawnTime.x, newCustomerSpawnTime.y));
        if (customers.Count < waitingQueuePositions.Count)
        {
            Customer newCustomer = CustomerPool.instance.GetObject().GetComponent<Customer>();
            AddCustomerToQueue(newCustomer);
        }
    }

    public Customer GetFirstCustomer()
    {
        if (customers.Count == 0) return null;
        Customer customer = customers[0];
        customer.OnTargetReached = null;
        return customer;
    }

    public bool IsCustomer(Customer customer)
    {
        return customers.Contains(customer);
    }
}