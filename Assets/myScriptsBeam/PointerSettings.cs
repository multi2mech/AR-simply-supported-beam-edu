using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/ScaleSettings", order = 1)]
public class PointerSettings : ScriptableObject
{
    public float scaleFactor = 0.0005f; // Default scale factor
    public Vector3 Base = new Vector3(-1.2f, 0.0f, 2f ); // Default scale base
    public Vector3 End = new Vector3(1.2f, 0.0f, 2f ); // Default scale base


    public void SetPointerInitialLocations(Vector3 baseIN, Vector3 endIN)
    {
        Base = baseIN;
        End = endIN;
    }
}