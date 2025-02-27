using UnityEngine;

public class EnsureIsEnabled : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(true);
        this.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
