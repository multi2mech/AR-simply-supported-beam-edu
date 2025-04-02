.. _Constraints:

Constraints
============



From ``myScriptBeam/LoadingScheme.cs``:

.. code-block:: c#
  
  constraints.Add(new Constraint(name: "Hinge", positionRatio: 0, type: ConstraintType.Hinge, internalQ: true, movableQ : false));
  constraints.Add(new Constraint(name: "Roller", positionRatio: (float)ratio_roller, type: ConstraintType.Roller, internalQ: true, movableQ: true));


.. warning::
  The following documentation is partially AI generated.

.. contents::
   :local:

Overview
--------

The ``Constraint`` class represents a boundary condition or physical constraint applied to a beam or structural element in Unity. It implements the ``IPositionable`` interface and includes data about the constraint type, position, mobility, and associated GameObjects. It also provides degree-of-freedom (DOF) info based on the constraint type.

Class Definition
----------------

.. code-block:: csharp

    [System.Serializable]
    public class Constraint : IPositionable

Fields and Properties
---------------------

- ``string name``  
  The name of the constraint.

- ``ConstraintType type``  
  Defines the constraint type: Roller, Hinge, or Fixed.

- ``bool movableQ``  
  Indicates whether the constraint can be repositioned.

- ``bool internalQ``  
  Marks if the constraint is internal.

- ``float positionRatio``  
  Relative position of the constraint on the beam (0 to 1).

- ``float minRatioPosition, maxRatioPosition``  
  The range within which the constraint can be placed.

- ``GameObject constraintObject``  
  A GameObject representing the visual constraint.

- ``GameObject commonJointObject``  
  Optional GameObject shared between constraints (e.g., joint representation).

- ``Vector3 position``  
  Position in world space (private, managed via methods).

- ``Vector3 originalPosition``  
  Stores the original position for reset or reference.

- ``dofFreeQ dofFreeQ`` (read-only)  
  Struct representing the degrees of freedom allowed for this constraint.

Methods
-------

- ``void SetPositionRatio(float ratio)``  
  Sets the constraint’s relative position.

- ``float GetPositionRatio()``  
  Returns the relative position of the constraint.

- ``void SetPosition(Vector3 position)``  
  Sets the constraint’s world position.

- ``Vector3 GetPosition()``  
  Returns the current world position.

- ``void SetMinMaxPositionRatio(float min, float max)``  
  Sets minimum and maximum limits for the position ratio.

- ``Vector2 GetMinMaxPositionRatio()``  
  Returns the position ratio limits as a Vector2.

- ``void SetObject(GameObject obj)``  
  Sets the 3D object that represents the constraint visually.

- ``GameObject GetObject()``  
  Gets the visual representation GameObject.

- ``void SetOriginalPosition(Vector3 position)``  
  Stores the original position of the constraint.

- ``Vector3 GetOriginalPosition()``  
  Returns the saved original position.

- ``void SetCommonObject(GameObject obj)``  
  Assigns a shared object used for joint representation.

- ``GameObject GetCommonObject()``  
  Gets the shared joint object.

Enum: ConstraintType
--------------------

.. code-block:: csharp

    public enum ConstraintType
    {
        Roller,
        Hinge,
        Fixed
    }

- ``Roller`` – Allows movement in one direction and rotation.
- ``Hinge`` – Restricts rotation, allows translation along one axis.
- ``Fixed`` – No degrees of freedom allowed.

Struct: dofFreeQ
----------------

Represents whether the constraint is free in the X, Y, and rotational directions.

.. code-block:: csharp

    public struct dofFreeQ
    {
        public bool x;
        public bool y;
        public bool r;

        public dofFreeQ(bool x, bool y, bool r);
        public static dofFreeQ SetFreeDof(bool x, bool y, bool r);
    }

``dofFreeQ`` is used internally to return the degrees of freedom depending on the constraint type.


.. toctree::
  :maxdepth: 5  