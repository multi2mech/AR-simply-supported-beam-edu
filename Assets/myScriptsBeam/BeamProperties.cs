using UnityEngine;

public class BeamProperties : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Load Properties")]

    [Tooltip("Young's Modulus.")]
    public float E = 1;
    [Tooltip("Cross-sectional area.")]
    public float A = 1;
    [Tooltip("Moment of inertia.")]
    public float I = 1;
    [Header("Reference")]
    public GameObject LoadingScheme;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
