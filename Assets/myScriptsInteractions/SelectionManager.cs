using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public bool isSelected = false; // Instance variable, accessible from Inspector
    public Vector3 RayInitialDistance { get; private set; } // Initial ray length when selected
    // Method to set isSelected to true
    public Quaternion initialControllerRotation { get; private set; } // Initial controller rotation when selected
    public bool debugQ = false;
    public Vector3 RayInitialOrigin { get; private set; } // Initial ray origin when selected
    public void SetSelectedTrue()
    {
        isSelected = true;

        RayInitialDistance = GetComponent<RayComputation>().RayDistance;
        RayInitialOrigin = GetComponent<RayComputation>().RayOrigin;
        initialControllerRotation = GetComponent<RayComputation>().RayRotation;

        
        if (debugQ){             
            Debug.Log("Controller quaternion to angles: " + initialControllerRotation.eulerAngles);
            Debug.Log("isSelected set to TRUE");
            Debug.Log("Updated ray length: " + RayInitialDistance);
        }
        
    }

    // Method to set isSelected to false
    public void SetSelectedFalse()
    {
        isSelected = false;
        if (debugQ){             
            Debug.Log("isSelected set to FALSE");
        }
    }


    
}


