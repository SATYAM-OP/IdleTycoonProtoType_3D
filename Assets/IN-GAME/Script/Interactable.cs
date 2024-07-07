using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInteractionManager>(out PlayerInteractionManager player))
        {
            PlayerIntracted(player);
        }
        if (other.TryGetComponent<Customer>(out Customer customer))
        {
            CustomerIntracted(customer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerInteractionManager>(out PlayerInteractionManager player))
        {
            PlayerStoppedInteraction(player);
        }
        if (other.TryGetComponent<Customer>(out Customer customer))
        {
            CustomerStoppedInteraction(customer);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<PlayerInteractionManager>(out PlayerInteractionManager player))
        {
            PlayerIntracting(player);
        }
        if (other.TryGetComponent<Customer>(out Customer customer))
        {
            CustomerIntracting(customer);
        }
    }

    protected virtual void PlayerIntracted(PlayerInteractionManager player){}
    protected virtual void PlayerIntracting(PlayerInteractionManager player){}
    protected virtual void PlayerStoppedInteraction(PlayerInteractionManager player){}
    protected virtual void CustomerIntracted(Customer customer) { }
    protected virtual void CustomerIntracting(Customer customer) { }
    protected virtual void CustomerStoppedInteraction(Customer customer) { }


}
