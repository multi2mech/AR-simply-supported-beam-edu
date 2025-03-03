using UnityEngine;


[System.Serializable]
public class Constraint: IPositionable
{
    public string name;       // Name of the constraint
    private Vector3 position;  // Position of the constraint
    public bool movableQ; // Whether the constraint is movable
    public ConstraintType type; // Type of the constraint
    public bool internalQ; // Whether the constraint is internal

    // Read-only property to retrieve degrees of freedom
     
     // Backing field for positionRatio
    [SerializeField] private float _positionRatio;

    // Property implementation for IPositionable
    public float positionRatio
    {
        get => _positionRatio; // Getter returns the private field
        set => _positionRatio = value; // Setter updates the private field
    }

    // ✅ Constructor to initialize the fields
    public Constraint(string name, float positionRatio, ConstraintType type, bool internalQ, bool movableQ)
    {
        this.name = name;
        this.positionRatio = positionRatio;
        this.type = type;
        this.internalQ = internalQ;
        this.movableQ = movableQ;
    }
       

        public void SetPositionRatio(float ratioIN)
        {
            positionRatio = ratioIN;
        }

        public float GetPositionRatio()
        {
            return positionRatio;
        }
        public void SetPosition(Vector3 positionIN)
        {
            position = positionIN;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        private GameObject constraintObject; // 3D object representing the constraint
        public void SetObject(GameObject obj)
        {
            constraintObject = obj;
        }

        public GameObject GetObject()
        {
            return constraintObject;
        }

        private Vector3 originalPosition;

        public void SetOriginalPosition(Vector3 positionIN)
        {
            originalPosition = positionIN;
        }
        public Vector3 GetOriginalPosition()
        {
            return originalPosition;
        }

        private GameObject commonJointObject; // 3D object representing the constraint
        public void SetCommonObject(GameObject obj)
        {
            commonJointObject = obj;
        }
        public GameObject GetCommonObject()
        {
            return commonJointObject;
        }
    public dofFreeQ dofFreeQ
    {
        get
        {
            switch (type)
            {
                case ConstraintType.Roller:
                    return dofFreeQ.SetFreeDof(false, false, false);
                case ConstraintType.Hinge:
                    return dofFreeQ.SetFreeDof(false, false, false);
                case ConstraintType.Fixed:
                    return dofFreeQ.SetFreeDof(false, false, false);
                default:
                    return dofFreeQ.SetFreeDof(false, false, false);
            }
        }
    }
}

public enum ConstraintType
        {
            Roller,
            Hinge,
            Fixed
        }

public struct dofFreeQ
{
    public bool x;
    public bool y;
    public bool r;

    // Constructor
    public dofFreeQ(bool x, bool y, bool r)
    {
        this.x = x;
        this.y = y;
        this.r = r;
    }

    // Method to set free degrees of freedom
    public static dofFreeQ SetFreeDof(bool x, bool y, bool r)
    {
        return new dofFreeQ(x, y, r);
    }
}




