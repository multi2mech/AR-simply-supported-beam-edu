using System.Collections.Generic;
using UnityEngine;

public class EnsureObjectsAreEnabled : MonoBehaviour
{

    [SerializeField] public List<GameObject> targetObjects; // Assign the disabled GameObject in Inspector

    void Start()
    {
    foreach (var targetObject in targetObjects)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // Enable the GameObject
            Debug.Log(targetObject.name + " has been enabled!");
        }
        
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }
    }
}