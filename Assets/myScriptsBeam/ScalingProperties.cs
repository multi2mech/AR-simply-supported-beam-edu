using UnityEngine;

public class ScalingProperties : MonoBehaviour
{
    private float pointerSizeScaled = 0.0f;
    public float GetPointerSizeScaled()
    {
        return pointerSizeScaled;
    }

    public void SetPointerSizeScaled(float size)
    {
        pointerSizeScaled = size;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
