.. _Loads:

Loads
============



From ``myScriptBeam/LoadingScheme.cs``:

.. code-block:: c#
  
  loads.Add(new Load(name: "Load1", positionRatio: (float)ratio_load, type: LoadType.Force, magnitude: 1, internalQ: false, minMultiplier: 0.5f, maxMultiplier: 2));


.. warning:: 
  The following documentation is partially AI generated.


.. contents::
   :local:

Overview
--------

The ``Load`` class in Unity represents a force or constraint applied to a structure. It implements the ``IPositionable`` interface and includes properties related to its magnitude, type, and spatial constraints. The load can be moved, scaled, and linked with 3D GameObjects to represent it visually in the scene.

It supports serialization, making it editable from the Unity Inspector.

Class Definition
----------------

.. code-block:: csharp

    [System.Serializable]
    public class Load : IPositionable

Fields and Properties
---------------------

- ``string name``  
  The name of the load.

- ``LoadType type``  
  Type of the load (Force, Torque, DistributedForce).

- ``float magnitude``  
  The numeric strength of the load (default: 1).

- ``bool movableQ``  
  Whether the load is movable.

- ``bool scalableQ``  
  Whether the load is scalable.

- ``bool internalQ``  
  Whether the load is internal to the system.

- ``float positionRatio``  
  Position of the load along the beam (0–1).

- ``float minRatioPosition, maxRatioPosition``  
  Range within which the load can be positioned.

- ``float minMultiplier, maxMultiplier``  
  Multiplier limits for scalable loads.

- ``GameObject magnitudeObject``  
  A GameObject representing the load's visual magnitude.

- ``GameObject pointerObject``  
  A GameObject used as a pointer or indicator.

- ``float referenceHeight``  
  Reference height of the magnitude object's mesh.

Methods
-------

- ``float GetMagnitude()``  
  Returns the magnitude value.

- ``Vector2 GetMultiplierRange()``  
  Returns the (min, max) multiplier range.

- ``void SetPositionRatio(float ratio)``  
  Sets the load’s position ratio.

- ``float GetPositionRatio()``  
  Gets the current position ratio.

- ``void SetPosition(Vector3 position)``  
  Sets the world position of the load.

- ``Vector3 GetPosition()``  
  Returns the load’s position in world space.

- ``float GetActualHeight()``  
  Returns the actual height from the mesh bounds.

- ``float GetReferenceHeight()``  
  Returns the stored reference height.

- ``void SetMagnitudeObject(GameObject obj)``  
  Sets the magnitude visual object and calculates reference height.

- ``GameObject GetMagnitudeObject()``  
  Returns the magnitude object.

- ``void SetMinMaxPositionRatio(float min, float max)``  
  Sets limits for valid position ratios.

- ``Vector2 GetMinMaxPositionRatio()``  
  Returns the min and max position ratio.

- ``void SetOriginalPosition(Vector3 position)``  
  Stores the original position of the load.

- ``Vector3 GetOriginalPosition()``  
  Returns the original position.

- ``void SetPointerObject(GameObject obj)``  
  Assigns the pointer visual object.

- ``GameObject GetPointerObject()``  
  Gets the pointer object.

Enum: LoadType
--------------

Defines the type of load:

.. code-block:: csharp

    public enum LoadType
    {
        Force,
        Torque,
        DistrubtedForce
    }

``Force`` – A standard linear force.  
``Torque`` – A rotational load.  
``DistrubtedForce`` – A force applied across a range.



.. toctree::
  :maxdepth: 5  