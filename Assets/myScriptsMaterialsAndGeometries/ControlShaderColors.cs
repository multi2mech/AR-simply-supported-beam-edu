using UnityEngine;

public class UpdateShaderProperties : MonoBehaviour
{
    public Material targetMaterial; // Assign the material with the shader
    private BeamPositioning beamPositioning;
    public LoadingScheme loadingScheme;
    private float[] thresholds;
    private int numThresholds;
    [SerializeField] private Color[] mycolors = { 
        new Color32(255, 255, 255, 255),  // Red (#FF0000)
        new Color32(255, 255, 255, 255),  // Yellow (#FFFF00)
        new Color32(255, 255, 255, 255),  // Green (#00FF00)
        new Color32(136, 118, 89, 255),  // Chamoisee (#887659)
        new Color32(109, 76, 61, 255),  // Coffee (#6D4C3D)
        new Color32(220, 201, 182, 255), // Dun (#DCC9B6)
        new Color32(114, 125, 113, 255) // Reseda Green (#727D71)
};


    void Update()
    {
        beamPositioning = GetComponent<BeamPositioning>();
        MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
        bool drawBeam = meshGenerator.drawBeam;
        if (drawBeam){
        if (beamPositioning != null)
        {
            targetMaterial.SetVector("_ReferencePoint", beamPositioning.GetBaseCenter());
            if (loadingScheme != null)
            {
                double[] thresholds_tmp = loadingScheme.GetAbsolutePositions();
                //Debug.Log("Thresholds: " + string.Join(", ", thresholds_tmp));
                thresholds = new float[10];

                numThresholds = thresholds_tmp.Length;
                for (int i = 0; i < numThresholds; i++)
                {
                    thresholds[i] = (float)thresholds_tmp[i];
                }
            }
            else
            {
                Debug.LogWarning("LoadingScheme script is not attached to the GameObject.");
            }

            // Convert threshold array to a Vector4 array (max 10 values)
            float[] shaderThresholds = new float[10];
            for (int i = 0; i < numThresholds && i < 10; i++)
            {
                shaderThresholds[i] = thresholds[i];
            }
            targetMaterial.SetFloatArray("_Thresholds", shaderThresholds);

            // Convert color array to a Vector4 array (max 10 values)
            Vector4[] shaderColors = new Vector4[10];
            for (int i = 0; i < numThresholds && i < 10; i++)
            {
                // shaderColors[i] = mycolors[i];

                shaderColors[i] = new Vector4(mycolors[i].r, mycolors[i].g, mycolors[i].b, mycolors[i].a);
                // Debug.Log($"Assigning mycolors[{i}]: {mycolors[i]} as Vector4 {shaderColors[i]}");

            }
            // Debug.Log("Colors: " + string.Join(", ", shaderColors));
            targetMaterial.SetVectorArray("_Colors", shaderColors);

            // Set the number of thresholds
            targetMaterial.SetInt("_NumThresholds", numThresholds);


        }
        else
        {
            Debug.LogWarning("BeamPositioning script is not attached to the GameObject.");
        }
       
    }
    }
}