using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInteractionManager : MonoBehaviour
{
    public static PlayerInteractionManager Instance; // Singleton instance

    void Awake()
    {
        Instance = this; // Initialize the singleton instance
    }

    public void CompleteInteraction()
    {
        // Logic for what happens when the interaction is completed
        Debug.Log("Interaction completed!");
    }


    public void IntractionComplete(IntractionType intractionType) 
    {


    }

}