.. _beamGenerator:

Beam Generator
===========================

Once the left and right pointers position is set (and the ``Y`` key is pressed on the left controller) the beam is drawned by:

- ``myScriptBeam/beamPosition.cs``:  is responsible for the position the initial and final point of the beam as well as computing the correct orientation of the beam normal and deflection direction.
- ``myScriptBeam/meshGenerator.cs``: is responsible for the generation of the mesh of the beam. 
- ``myScriptBeam/baseSection.cs``: is responsible for the cross section definition.

3D orientation
----------------

Once the pointers are set the beam need to be oriented. The referred GameObject stores the following information that will be used also for the movement of constraints and loads:
 - beam normal
 - beam direction
 - deflection direction
 - initial and final points in world coordinates


Beam normal
""""""""""""""""""""""""""""""


Deflection direction
""""""""""""""""""""""""""""""


Mesh generation
----------------

The 3D beam is generated programmatically by defining:

 - The cross section
 - Triangulation along the beam direction
 - Initial and final faces

Cross-section
""""""""""""""""""""""""""""""

Given the beam length :math:`L` and the cross-section sizes are combuted by considering a :math:`1/10` ratio. 
``baseSection.cs`` allows for different cross-section shapes. For instance a squared cross section is return with repsect the computed sizes:

.. code-block:: c#

    private static Vector3[] GetRectangleSection(float width, float height, BeamPositioning beamPositioning)
    {
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        Vector3[] rectangle = new Vector3[]
        {
            new Vector3(-halfWidth, -halfHeight, 0), // Bottom left
            new Vector3(halfWidth, -halfHeight, 0),  // Bottom right
            new Vector3(halfWidth, halfHeight, 0),   // Top right
            new Vector3(-halfWidth, halfHeight, 0)   // Top left
        };

        return TransformSection(rectangle, beamPositioning);
    }


Moreover, the section need to be oriented perpendicular to the beam section:

.. code-block:: c#

    private static Vector3[] TransformSection(Vector3[] section, BeamPositioning beamPositioning)
    {
        Vector3 center = beamPositioning.GetBaseCenter();
        Vector3 normal = beamPositioning.GetNormal();

        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, normal);

        for (int i = 0; i < section.Length; i++)
        {
            section[i] = rotation * section[i];
            section[i] += center;
        }

        return section;
    }

Beam mesh
""""""""""""""""""""""""""""""


.. code-block:: c#
    
   // from meshGenerator.cs
   void GenerateBeamMesh()
    {
        float extrusionLength = beamPositioning.GetBeamLength() / extrusionSteps;
        Vector3[] baseSection = BaseSection.GetSection(sectionType, beamPositioning.GetBeamLength(), beamRatio, beamPositioning);
        beamPositioning.SetBeamSectionSize(beamPositioning.GetBeamLength()/beamRatio);
        mesh = GenerateTheMesh(baseSection, extrusionLength);
        originalMesh = Instantiate(mesh);
    }

To generate the beam the cross section (obtained from ``myScriptBeam/BaseSection.cs``, with a fixed number of points ``pts`` ) is repeated ``extrusionSteps`` times along the beam direction such to haves :math:`extrusionSteps*pts` total points. Sequential cross sections are trinagulated by connecting the points of the cross section with the points of the next cross section as follows:

.. code-block:: c#
    
    // from meshGenerator.cs
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


    



Triangulation
""""""""""""""""""""""""""""""

To ensure a perfect triangulation for the top and bottom cross section faces of the beam the ear clipping algorithm :cite:`debergComputationalGeometryAlgorithms2008` is used. The triangulation is done in the ``myScriptBeam/EarClipping.cs`` script. The triangulation is done in the following steps:
 
1. The vertices of the cross section are sorted in a clockwise order.
2. The ear clipping algorithm is applied to the sorted vertices to generate the triangles.
3. The triangles are stored in a list and used to generate the mesh.
4. The mesh is generated using the triangles and the vertices of the cross section.



Deflection and deformed beam
--------------------------------


Beam colors and segment identication
-------------------------------------


.. toctree::
   :maxdepth: 5  

   


  