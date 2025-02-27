using UnityEngine;

public class RescalePointer : MonoBehaviour
{
    public PointerSettings pointerSettings; // Reference to the script containing the scale factor
    public Position position; 

    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (pointerSettings != null)
        {
            transform.localScale = new Vector3(pointerSettings.scaleFactor, pointerSettings.scaleFactor, pointerSettings.scaleFactor);
        }
        else
        {
            Debug.LogError("pointerSettings script not assigned to RescalePointer!");
        }

        if (position == Position.Start)
        {
            initialPosition = pointerSettings.Base;
        }
        if (position == Position.End)
        {
            initialPosition = pointerSettings.End;
        }
        transform.localPosition = initialPosition;
    }
}

public enum Position
        {
            Start,
            End
        }