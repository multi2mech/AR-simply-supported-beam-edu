using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/ScaleSettings", order = 1)]
public class PointerSettings : ScriptableObject
{
    public float scaleFactor = 0.0005f; // Default scale factor
    public Vector3 Base = new Vector3(-1.0f, 0.5f, 1f ); // Default scale base
    public Vector3 End = new Vector3(1.0f, 0.5f, 1f ); // Default scale base
}