using UnityEngine;


[System.Serializable]
        public class Load : IPositionable
    {
        public string name;       // Name of the constraint
         // Backing field for positionRatio
        [SerializeField] private float _positionRatio;

        // Property implementation for IPositionable
        public float positionRatio
        {
            get => _positionRatio; // Getter returns the private field
            set => _positionRatio = value; // Setter updates the private field
        }

    [SerializeField] private float _minRatioPosition;
    public float minRatioPosition
    {
        get => _minRatioPosition; // Getter returns the private field
        set => _minRatioPosition = value; // Setter updates the private field
    }


    [SerializeField] private float _maxRatioPosition;
    public float maxRatioPosition
    {
        get => _maxRatioPosition; // Getter returns the private field
        set => _maxRatioPosition = value; // Setter updates the private field
    }
        public LoadType type; // Type of the constraint
        public bool movableQ = true; // Whether the constraint is internal
        public float magnitude = 1;   // Magnitude of the load
        public bool scalableQ = true;

        public bool internalQ = false; // Whether the constraint is internal
        public float minMultiplier = 0.0f;

        public float maxMultiplier = 2f;

        private Vector3 position;  
        private GameObject magnitudeObject; // 3D object representing the constraint
        private float referenceHeight; // Mesh of the magnitude object
        private GameObject pointerObject; // 3D object representing the constraint

        public float GetMagnitude()
        {
            return magnitude;
        }

        public Vector2 GetMultiplierRange()
        {
            return new Vector2(minMultiplier, maxMultiplier);
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

        public float GetActualHeight()
        {
            return magnitudeObject.GetComponent<MeshFilter>().mesh.bounds.size.z;
        }

        public float GetReferenceHeight(){
            return referenceHeight;
        }

        
        public void SetMagnitudeObject(GameObject obj)
        {
            magnitudeObject = obj;
            Mesh mesh = magnitudeObject.GetComponentInChildren<MeshFilter>().mesh;
            if (mesh != null)
            {
                referenceHeight = magnitudeObject.GetComponentInChildren<MeshFilter>().mesh.bounds.size.z;
                Debug.Log("Set a reference height of " + referenceHeight + " for " + magnitudeObject.name);
            }
            
        }

        

        public GameObject GetMagnitudeObject()
        {
            return magnitudeObject;
        }

        private Vector3 originalPosition;
    //private double positionRatio1;

    public Load(string name, double positionRatio, LoadType type, int magnitude, bool internalQ, float minMultiplier, float maxMultiplier, float minRatioPosition = 0.0f, float maxRatioPosition = 1.0f)
    {
        this.name = name;
        this.positionRatio = (float)positionRatio;
        this.type = type;
        this.magnitude = magnitude;
        this.internalQ = internalQ;
        this.minMultiplier = minMultiplier;
        this.maxMultiplier = maxMultiplier;
        this.minRatioPosition = minRatioPosition;
        this.maxRatioPosition = maxRatioPosition;
    }

    public void SetMinMaxPositionRatio(float min, float max)
        {
            minRatioPosition = min;
            maxRatioPosition = max;
        }

        public Vector2 GetMinMaxPositionRatio()
        {
            return new Vector2(minRatioPosition, maxRatioPosition);
        }

    public void SetOriginalPosition(Vector3 positionIN)
        {
            originalPosition = positionIN;
        }
        public Vector3 GetOriginalPosition()
        {
            return originalPosition;
        }

        public void SetPointerObject(GameObject obj)
        {
            pointerObject = obj;
        }
        public GameObject GetPointerObject()
        {
            return pointerObject;
        }
        
    }

public enum LoadType
        {
            Force,
            Torque,
            DistrubtedForce
        }

