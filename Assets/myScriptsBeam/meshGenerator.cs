using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BeamPositioning))]
public class MeshGenerator : MonoBehaviour
{
    public int extrusionSteps = 6;
    // public string sectionType = "Squared";
    public int beamRatio = 8;
    public SectionType sectionType;
    public int beamSegments = 2; 
    public Material undeformedMaterial;
    
    public Material momentumMaterial;
    public Material momentumFillMaterial;
    private Mesh undeformedBeamMesh;
    private Mesh momentumMesh;
    private Mesh mesh;
    private BeamPositioning beamPositioning;
    public bool drawBeam = false;
    public LoadGeometriesAuxiliaries loadGeometriesAuxiliaries;
    private Mesh originalMesh;
    private Mesh originalMomentumMesh;
    private GameObject undeformedMeshObject;
    private GameObject momentumMeshObject;
    private GameObject momentumFillMeshObject;

    private bool drawUndeformedMeshQ = false;
    private bool deformMainBeamQ = false;
    private bool drawMomentumMeshQ = false;
    public LoadingScheme loadingScheme;
    private Mesh momentumFillMesh;
    public MathematicalSegment mathematicalSegment;

    private bool generatedMeshesQ = false;
    
    void Start()
    {
        beamPositioning = GetComponent<BeamPositioning>();

        if (beamPositioning == null)
        {
            Debug.LogError("BeamPositioning component not found on the GameObject!");
            return;
        }
        

        if (drawBeam)
        {
            GenerateMesh();
            undeformedMeshObject = new GameObject("UndeformedMeshObject");
            momentumMeshObject = new GameObject("MomentumMeshObject");
            momentumFillMeshObject = new GameObject("MomentumFillMeshObject");
            
            
            
        }

    }

    void Update()
    {   if (drawBeam)
        {   if (momentumFillMeshObject == null || momentumMeshObject == null || undeformedMeshObject == null || mesh == null)
            {
                generatedMeshesQ = false;
            }
            if (!generatedMeshesQ){
                GenerateMesh();
            }
            
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
            loadGeometriesAuxiliaries.SetLoads();
            loadGeometriesAuxiliaries.UpdateLoads();
            loadGeometriesAuxiliaries.SetConstraints();
            loadGeometriesAuxiliaries.UpdateConstraints();

            if (OVRInput.GetDown(OVRInput.RawButton.A)) {
                drawUndeformedMeshQ = !drawUndeformedMeshQ; 
                deformMainBeamQ = !deformMainBeamQ;
            }
            
            double[,] matrix = loadingScheme.GetMatrix();
            double[] vector = loadingScheme.GetVector(); 
            var result = MatrixSolver.Solve(matrix, vector);

            //Debug.Log("Solved: " + string.Join(", ", result.Select(r => $"{r,10:E2}")));
            double[] absolutePositions = loadingScheme.GetAbsolutePositions();
            
            mathematicalSegment.SetResults(result.ToList());
            
            UpdateDeformedMesh(mathematicalSegment.GetSegments());
            
            UpdateMomentumMesh(mathematicalSegment.GetSegments());
            

            // Vector3 pointA = beamPositioning.GetBaseCenter();
            // Vector3 pointB = beamPositioning.GetBaseCenter() + beamPositioning.GetNormal() * beamPositioning.GetBeamLength();
            // float thickness = 0.1f * beamPositioning.GetBeamSectionSize();
            // // Create a new cylinder or use a prefab
            // GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // // Calculate the midpoint between A and B
            // line.transform.position = (pointA + pointB) / 2f;

            // // Set the scale (Y-axis should be half the length of the line)
            // float length = Vector3.Distance(pointA, pointB);
            // line.transform.localScale = new Vector3(thickness, length / 2f, thickness);

            // // Rotate to align with direction
            // line.transform.up = (pointB - pointA).normalized;
            

            
            // if (OVRInput.GetDown(OVRInput.RawButton.A)) {
            //     drawUndeformedMeshQ = !drawUndeformedMeshQ; // Example of toggling a boolean
            // }

            if (drawUndeformedMeshQ){
                if (undeformedMeshObject == null)
                {
                    undeformedMeshObject = new GameObject("UndeformedMeshObject");
                    
                }
                ShowUndeformedBeam(undeformedBeamMesh, undeformedMaterial);
            }
            else
            {
                if (undeformedMeshObject != null)
                {
                    Destroy(undeformedMeshObject);
                    undeformedMeshObject = null;
                }
            }



            if (OVRInput.GetDown(OVRInput.RawButton.B)) {
                drawMomentumMeshQ = !drawMomentumMeshQ; // Example of toggling a boolean
            }

            if (drawMomentumMeshQ){
                if (momentumMeshObject == null)
                {
                    momentumMeshObject = new GameObject("MomentumMeshObject");

                }
                if (momentumFillMeshObject == null)
                {
                    momentumFillMeshObject = new GameObject("MomentumFillMeshObject");
                    
                }
                ShowMomentum(momentumMesh, momentumMaterial);
                CreateAndShowMomentumFillShape(beamPositioning, momentumMesh.vertices.ToList());
                ShowMomentumFill(momentumFillMesh, momentumFillMaterial);
            }
            else
            {
                if (momentumMeshObject != null)
                {
                    Destroy(momentumMeshObject);
                    momentumMeshObject = null;
                }
                if(momentumFillMeshObject != null)
                {
                    Destroy(momentumFillMeshObject);
                    momentumFillMeshObject = null;
                }
            }
    }
    }

void UpdateDeformedMesh(List<MathematicalSegment.Segment> segments){

    // float[] deflections = new float[segments.Count];
    // for (int i = 0; i < segments.Count; i++)
    // {
    //     deflections[i] = segments[i].Deflection;
    // }
    Vector3[] vertices = originalMesh.vertices;
    EQ_IV eq = new EQ_IV();
    if (deformMainBeamQ){

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 basePoint = beamPositioning.GetBaseCenter();
            Vector3 vertex = vertices[i];
            Vector3 distance = vertex - basePoint;
            
            float distanceAlongBeam = Mathf.Abs(Vector3.Dot(distance, beamPositioning.GetNormal()));
            //int segmentIndex = Mathf.FloorToInt(distanceAlongBeam / beamPositioning.GetBeamLength() * beamSegments);
            int segmentIndex = GetSegmentIndex(segments, distanceAlongBeam);
            if (segmentIndex == -1)
            {
                Debug.LogError("Segment index not found for distance: " + distanceAlongBeam + " over a length of " + beamPositioning.GetBeamLength());
            }
            float deflection = eq.Get_v(distanceAlongBeam, segments[segmentIndex]);
            
            vertices[i] = vertex - deflection*beamPositioning.GetDeflectionDirection();
        }
    }
    // reassign it to the new mesh
    mesh.vertices = vertices;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    GetComponent<MeshFilter>().mesh = mesh;
}


void UpdateMomentumMesh(List<MathematicalSegment.Segment> segments){

    // float[] deflections = new float[segments.Count];
    // for (int i = 0; i < segments.Count; i++)
    // {
    //     deflections[i] = segments[i].Deflection;
    // }
    Vector3[] vertices = originalMomentumMesh.vertices;
    EQ_IV eq = new EQ_IV();
    for (int i = 0; i < vertices.Length; i++)
    {
        Vector3 basePoint = beamPositioning.GetBaseCenter();
        Vector3 vertex = vertices[i];
        Vector3 distance = vertex - basePoint;
        
        float distanceAlongBeam = Mathf.Abs(Vector3.Dot(distance, beamPositioning.GetNormal()));
        //int segmentIndex = Mathf.FloorToInt(distanceAlongBeam / beamPositioning.GetBeamLength() * beamSegments);
        int segmentIndex = GetSegmentIndex(segments, distanceAlongBeam);
        if (segmentIndex == -1)
        {
            Debug.LogError("Segment index not found for distance: " + distanceAlongBeam + " over a length of " + beamPositioning.GetBeamLength());
        }
        float deflection = eq.Get_M_normalized(distanceAlongBeam, segments[segmentIndex]);
        
        vertices[i] = vertex + deflection*beamPositioning.GetDeflectionDirection();
    }
    // reassign it to the new mesh
    momentumMesh.vertices = vertices;
    momentumMesh.RecalculateNormals();
    momentumMesh.RecalculateBounds();
    GetComponent<MeshFilter>().mesh = mesh;
}

public void ShowMomentum(Mesh mesh, Material material)
    {
        // Create a new GameObject
        // Add a MeshFilter component and assign the mesh
        MeshFilter meshFilter = momentumMeshObject.GetComponent<MeshFilter>();
        if (meshFilter == null){
            meshFilter = momentumMeshObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = mesh;

        // Add a MeshRenderer component and assign the material
        MeshRenderer meshRenderer = momentumMeshObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null){
            meshRenderer = momentumMeshObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = material;
        momentumMeshObject.transform.position = new Vector3(0, 0, 0);
    }


    public void ShowMomentumFill(Mesh mesh, Material material)
    {
        // Create a new GameObject
        // Add a MeshFilter component and assign the mesh
        MeshFilter meshFilter = momentumFillMeshObject.GetComponent<MeshFilter>();
        if (meshFilter == null){
            meshFilter = momentumMeshObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = mesh;

        // Add a MeshRenderer component and assign the material
        MeshRenderer meshRenderer = momentumMeshObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null){
            meshRenderer = momentumMeshObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = material;
        momentumMeshObject.transform.position = new Vector3(0, 0, 0);
    }



public void CreateAndShowMomentumFillShape(BeamPositioning beamPositioning, List<Vector3> verticesIN){
    Vector3 pointA = beamPositioning.GetBaseCenter();
    Vector3 pointB = beamPositioning.GetBaseCenter() + beamPositioning.GetNormal() * beamPositioning.GetBeamLength();
    Mesh mesh;
    if (momentumFillMesh == null)
    {
        mesh = new Mesh();
    }
    else{
        mesh = momentumFillMesh;
    }
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    // Generate vertices: functionVertices + corresponding segment vertices
    int count = verticesIN.Count;
    for (int i = 0; i < count; i++)
    {
        vertices.Add(verticesIN[i]); // Upper curve
        float t = (float)i / (count - 1);
        Vector3 basePoint = Vector3.Lerp(pointA, pointB, t);
        vertices.Add(basePoint); // Base segment
    }

    // Generate triangles
    for (int i = 0; i < count - 1; i++)
    {
        int i0 = i * 2;
        int i1 = i0 + 1;
        int i2 = i0 + 2;
        int i3 = i0 + 3;

        // First triangle
        triangles.Add(i0);
        triangles.Add(i2);
        triangles.Add(i1);

        // Second triangle
        triangles.Add(i1);
        triangles.Add(i2);
        triangles.Add(i3);
    }

    // Assign mesh data
    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.RecalculateNormals();

    if (momentumFillMesh == null)
    {
        momentumFillMesh = Instantiate(mesh);    
    }
    
    MeshFilter mf = momentumFillMeshObject.GetComponent<MeshFilter>();
    if (mf == null){
        mf = momentumFillMeshObject.AddComponent<MeshFilter>();
    }
    mf.mesh = momentumFillMesh;
    MeshRenderer mr = momentumFillMeshObject.GetComponent<MeshRenderer>();
    if (mr == null){
        mr = momentumFillMeshObject.AddComponent<MeshRenderer>();
    }

Material mat = momentumFillMaterial;

// Enable transparency settings for Unity Standard Shader
mat.SetFloat("_Mode", 3); // 3 = Transparent mode
mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
mat.SetInt("_ZWrite", 0);
mat.DisableKeyword("_ALPHATEST_ON");
mat.EnableKeyword("_ALPHABLEND_ON");
mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
mat.renderQueue = 3000;

// Apply transparency while keeping original color
mat.color = new UnityEngine.Color(mat.color.r, mat.color.g, mat.color.b, 0.3f); // Adjust alpha

// Assign modified material to the mesh renderer (only if needed)
mr.material = mat;

    momentumFillMeshObject.transform.position = new Vector3(0, 0, 0);


}

public void ShowUndeformedBeam(Mesh mesh, Material material)
    {
        // Create a new GameObject
        // Add a MeshFilter component and assign the mesh
        MeshFilter meshFilter = undeformedMeshObject.GetComponent<MeshFilter>();
        if (meshFilter == null){
            meshFilter = undeformedMeshObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = mesh;

        // Add a MeshRenderer component and assign the material
        MeshRenderer meshRenderer = undeformedMeshObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null){
            meshRenderer = undeformedMeshObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = material;
        undeformedMeshObject.transform.position = new Vector3(0, 0, 0);
    }

public int GetSegmentIndex(List<MathematicalSegment.Segment> segments, double distanceAlongBeam)
{
    for (int i = 0; i < segments.Count; i++)
    {   //Debug.Log("Sement:" + i + "start: " + segments[i].Start + "distance total:"+ distanceAlongBeam);
        if (segments[i].Start <= distanceAlongBeam && distanceAlongBeam <= segments[i].End)
        {
            return i; // Return the index of the matching segment
        }
        if (i==segments.Count-1 && distanceAlongBeam > segments[i].End)
        {
            return i;
        }
    }
    return -1; // Return -1 if no segment matches
}

void GenerateMesh(){
    GenerateBeamMesh();
    GenerateUndeformedBeamMesh();
    GenerateMomentumMesh();
    generatedMeshesQ = true;
    
}


void GenerateMomentumMesh()
{
    float extrusionLength = beamPositioning.GetBeamLength() / extrusionSteps;
    // Debug.Log("extrusionLength: " + extrusionLength);
    Vector3[] baseSection = BaseSection.GetSection(sectionType, beamPositioning.GetBeamLength(),2* beamRatio, beamPositioning);

    mesh =  GenerateTheMesh(baseSection, extrusionLength);
    //originalMesh = mesh;
    momentumMesh = Instantiate(mesh);
    originalMomentumMesh = Instantiate(mesh);
}

void GenerateUndeformedBeamMesh()
{
    float extrusionLength = beamPositioning.GetBeamLength() / extrusionSteps;
    // Debug.Log("extrusionLength: " + extrusionLength);
    Vector3[] baseSection = BaseSection.GetSection(sectionType, beamPositioning.GetBeamLength(),2* beamRatio, beamPositioning);

    mesh =  GenerateTheMesh(baseSection, extrusionLength);
    //originalMesh = mesh;
    undeformedBeamMesh = Instantiate(mesh);
}

void GenerateBeamMesh()
{
    float extrusionLength = beamPositioning.GetBeamLength() / extrusionSteps;
    // Debug.Log("extrusionLength: " + extrusionLength);
    Vector3[] baseSection = BaseSection.GetSection(sectionType, beamPositioning.GetBeamLength(), beamRatio, beamPositioning);

    beamPositioning.SetBeamSectionSize(beamPositioning.GetBeamLength()/beamRatio);

    

    //originalMesh = mesh;

    mesh = GenerateTheMesh(baseSection, extrusionLength);
    originalMesh = Instantiate(mesh);
}

private Mesh GenerateTheMesh(Vector3[] baseSection, float extrusionLength){

    Vector3[] vertices = new Vector3[baseSection.Length * (extrusionSteps + 1)];
    for (int i = 0; i <= extrusionSteps; i++)
    {
        for (int j = 0; j < baseSection.Length; j++)
        {
            vertices[i * baseSection.Length + j] = baseSection[j] + i * extrusionLength * beamPositioning.GetNormal();
        }
    }

    int quads = extrusionSteps * baseSection.Length;
    int[] triangles = new int[quads * 6];
    int t = 0;

    for (int i = 0; i < extrusionSteps; i++)
    {
        for (int j = 0; j < baseSection.Length; j++)
        {
            int current = i * baseSection.Length + j;
            int next = i * baseSection.Length + (j + 1) % baseSection.Length;
            int above = (i + 1) * baseSection.Length + j;
            int aboveNext = (i + 1) * baseSection.Length + (j + 1) % baseSection.Length;

            triangles[t++] = current;
            triangles[t++] = next;
            triangles[t++] = above;

            triangles[t++] = next;
            triangles[t++] = aboveNext;
            triangles[t++] = above;
        }
    }

    // Triangulate the base section
    List<int> baseTriangles = EarClipping.Triangulate(baseSection, -beamPositioning.GetNormal());

    // Triangulate the cap section
    Vector3[] capSection = new Vector3[baseSection.Length];
    for (int i = 0; i < baseSection.Length; i++)
    {
        capSection[i] = vertices[extrusionSteps * baseSection.Length + i];
    }
    List<int> capTriangles = EarClipping.Triangulate(capSection, -beamPositioning.GetNormal());

    // Reverse the winding order for the cap
    for (int i = 0; i < capTriangles.Count; i += 3)
    {
        // Swap two vertices to reverse the triangle's winding order
        int temp = capTriangles[i];
        capTriangles[i] = capTriangles[i + 1];
        capTriangles[i + 1] = temp;
    }

    int[] allTriangles = new int[triangles.Length + baseTriangles.Count + capTriangles.Count];
    triangles.CopyTo(allTriangles, 0);

    for (int i = 0; i < baseTriangles.Count; i++)
    {
        allTriangles[triangles.Length + i] = baseTriangles[i];
    }

    for (int i = 0; i < capTriangles.Count; i++)
    {
        allTriangles[triangles.Length + baseTriangles.Count + i] = capTriangles[i] + extrusionSteps * baseSection.Length;
    }

    mesh = new Mesh
    {
        vertices = vertices,
        triangles = allTriangles
    };

    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    GetComponent<MeshFilter>().mesh = mesh;

    return mesh;

}


private Vector3[] ConvertToLocalSpace(Vector3[] vertices)
{
    Vector3[] localVertices = new Vector3[vertices.Length];
    for (int i = 0; i < vertices.Length; i++)
    {
        localVertices[i] = transform.InverseTransformPoint(vertices[i]);
    }
    return localVertices;
}


public void SetBoolDrawToTrue()
{
    drawBeam = true;

}


public void SetBoolDrawToFalse()
{
    drawBeam = false;

}

}