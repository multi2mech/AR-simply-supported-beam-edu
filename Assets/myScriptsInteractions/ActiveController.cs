using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerTriggerManager : MonoBehaviour
{
    //[SerializeField] private GameObject leftController;
    //[SerializeField] private GameObject rightController;

    private GameObject activeController; // Currently active controller
    

    private bool isRightControllerActive = true; 
    // Input Action references (set in the Inspector)
    //[SerializeField] private InputActionProperty leftTriggerAction;
    //[SerializeField] private InputActionProperty rightTriggerAction;

    private void Update()
    {
        // Check for right trigger press
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            ControllerSetActiveRight();
        }

        // Check for left trigger press
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            ControllerSetActiveLeft();
        }
    }

    private void ControllerSetActiveRight()
    {
        isRightControllerActive = true;
        //Debug.Log("Right controller is active.");
    }

    private void ControllerSetActiveLeft()
    {
        isRightControllerActive = false;
        //Debug.Log("Left controller is active.");
    }
    public bool IsRightControllerActive()
    {
        return isRightControllerActive;
    }
}
