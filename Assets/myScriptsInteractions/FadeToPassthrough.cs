using UnityEngine;
using System.Collections;

public class FadeToPassthrough : MonoBehaviour
{
    public Material backgroundMaterial; // Assign the background material
    public float fadeDelay = 6f;  // Time before fade starts
    public float fadeDuration = 4f; // Fade-out duration

    private void Start()
    {
        // Ensure the material starts fully visible
        Color color = backgroundMaterial.color;
        color.a = 1f;
        backgroundMaterial.color = color;

        StartCoroutine(FadeOutBackground());
    }

    IEnumerator FadeOutBackground()
    {
        // Wait for the fade delay before starting
        yield return new WaitForSeconds(fadeDelay);

        Color color = backgroundMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0.1f, elapsedTime / fadeDuration);
            color.a = alpha;
            backgroundMaterial.color = color;
            yield return null;
        }

        // Ensure it reaches the final value
        color.a = 0.1f;
        backgroundMaterial.color = color;

        // After fade-out, disable the object for passthrough effect
        gameObject.SetActive(false);
    }
}