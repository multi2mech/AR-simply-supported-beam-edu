.. _Geometries:

Geometries
============

Some usefull 3D geometries are included in the project. They are located in the folder ``Assets/my3Dgeometries``. The included geometries are the intial pointers, the force (arrow point and body) and several constraints:

 - Hinge
 - Roller
 - Fixed

3D models
----------------

.. raw:: html

    <style>
        .three-column {
            display: flex;
            gap: 20px;
        }

        .column {
            flex: 1;
        }

        iframe {
            width: 100%;
            height: 400px;
            border: 1px solid #ccc;
        }
    </style>

.. raw:: html

    <div class="three-column">
        <div class="column">
            <iframe src="../_static/hinge.html"></iframe>
        </div>
        <div class="column">
            <iframe src="../_static/roller.html"></iframe>
        </div>
        <div class="column">
            <iframe src="../_static/fixed.html"></iframe>
        </div>
    </div>

Note that all the goemetries miss the common "cylinder" because it is used as a second object to marker the position. 

.. raw:: html

    <div class="three-column">
        <div class="column">
            <iframe src="../_static/common_c.html"></iframe>
        </div>
    </div>

Similarly, the force is splitted into body and arrow head.

.. raw:: html

    <div class="three-column">
        <div class="column">
            <iframe src="../_static/arrow.html"></iframe>
        </div>
        <div class="column">
            <iframe src="../_static/arrow_body.html"></iframe>
        </div>
    </div>


Sizes and positions
--------------------

Each imported 3D geoemtriy is treateed as a Unity prefab with attached:
 - ``UpdateMaterialProperties.cs`` script to update the material properties based on the RayCasting.
 - Collider surface and script for the Meta Ray Interaction
 - ``SelectionManager.cs`` script to manage the selection of the object
 - ``RayComputation.cs`` script to manage the ray casting and the interaction with the object.
 - ``InteractableUnityEventWrapper.cs`` script to manage the interaction with the object.
 - Movements script like ``LoadMovement`` or ``ConstraintMovement`` to manage the movement of the object.

3D geometry:

.. image:: /_static/geom1.png
   :alt: Colors
   :width: 600px

Parent object:

.. image:: /_static/geom2.png
   :alt: Colors
   :width: 600px

Scale adjustment
""""""""""""""""""""""""""""""""""""""""""""""""

Each geoemtry is normalized and then rescaled proportianlly to the beam section.

.. code-block:: c#

  public void NormalizeObject(GameObject obj, char? flag = null)
    {
        // Find the MeshFilter in the object or its children
        MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
        if (meshFilter != null)
        {
            Bounds bounds = meshFilter.mesh.bounds;// Get the mesh bounds in local space

            if (largestDimension > 0)
            {
                // Compute the scale factor to normalize the largest dimension to 1
                float scaleFactor = 1.0f / largestDimension;
                // Rescale the object
                meshFilter.transform.localScale *= scaleFactor;
                Quaternion targetRotation = Quaternion.Euler(-90, 0, 0); // Rotate 90° around X-axis
                meshFilter.transform.rotation = targetRotation;
                Renderer renderer = obj.GetComponentInChildren<Renderer>();
                
                SetPointerDimensionScaled(pointerDimension * scaleFactor, obj);
            
                if (renderer != null && loadsMaterial != null)
                {
                    // Assign the material
                    renderer.material = loadsMaterial;
                    Debug.Log($"Assigned custom material to {obj.name}");
                }
                else
                {
                    Debug.LogWarning($"Renderer or custom material not found for {obj.name}");
                }

            }
            else
            {
                Debug.LogError($"Bounds size is zero for {obj.name}. Cannot normalize.");
            }
        }
        else
        {
            Debug.LogError($"No MeshFilter found in {obj.name} or its children.");
        }
    }






.. toctree::
  :maxdepth: 5  