using UnityEngine;

public class PointerState : MonoBehaviour
{

    public bool showPointer = true; 
    public GameObject object1; // First object to toggle
    public GameObject object2; // Second object to toggle
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the objects are assigned
        if (object1 != null && object2 != null)
        {
            // Set the Renderer.enabled state based on showPointer
            object1.GetComponent<Renderer>().enabled = showPointer;
            object1.GetComponentInChildren<MeshCollider>().enabled = showPointer;
            object2.GetComponent<Renderer>().enabled = showPointer;
            object2.GetComponentInChildren<MeshCollider>().enabled = showPointer;
        }
        else
        {
            Debug.LogError("One or both objects are not assigned.");
        }
    }

    public void DeactivatePointer()
    {
        showPointer = false;
    }

    public void ActivatePointer()
    {
        showPointer = true;
    }

}
