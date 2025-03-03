using System;
using System.Collections.Generic;
using System.Data;
using MathNet.Numerics.Interpolation;
using Unity.IntegerTime;
using UnityEngine;
using UnityEngine.AI;

public class LoadingScheme : MonoBehaviour
{
    // Loading scenario
    
    private double E;
    private double A;
    private double I;
    private double beamLength; // this is automatically set from the initial pointers
    
    [Header("Load Properties")]

    [Tooltip("Position of the roller support (0 to 1 along the beam).")]
    public double ratio_roller = 0.5; // roller positon

    [Tooltip("Initial force magnitude.")]
    public double force = 1; // force magnitude 

    private double force_scaled; // actual force mangitude (scaled with the geometry)
    private double zF_s0; // relative end of the first segment
    private double zF_s1; // relative end of the second segment

    private double[] absolutePositions; // this is automatically set

    // =======================
    // 🎯 Matrix & Vector (Structural Solver)
    // =======================
    
    // Matrix a vector for the structural solver
    private double[,] matrix ;
    private double[] vector;
    [Header("Structural Solver Properties")]
    [Tooltip("Number of rows in the solver matrix.")]
    public int numRows = 12;
    [ Tooltip("Number of columns in the solver matrix.")]
    public int numCols = 12;



    void Start()
    {

        loads.Add(new Load(name: "Load1", positionRatio: 1, type: LoadType.Force, magnitude: 1, internalQ: false, minMultiplier: 0.5f, maxMultiplier: 2));
        
        constraints.Add(new Constraint(name: "Hinge", positionRatio: 0, type: ConstraintType.Hinge, internalQ: true, movableQ : false));
        constraints.Add(new Constraint(name: "Roller", positionRatio: (float)ratio_roller, type: ConstraintType.Roller, internalQ: true, movableQ: true));

        GameObject parentObject = gameObject.transform.parent.gameObject;

        // Retrieve the BeamPositioning component from the parent
        beamPosition = parentObject.GetComponentInChildren<BeamPositioning>();
        if (beamPosition == null)
        {
            Debug.LogError("BeamPositioning component not found in parent object");
        }

    }

    
    void Update()
    {
        bool drawBeam = meshGenerator.GetComponent<MeshGenerator>().drawBeam;
        if (drawBeam){       
            UpdateBeamProperties(); // Load the beam properties from the BeamProperties script

            // Load at the end of the beam
            // double newratio = UpdateLoadPositionFromGeometry(loads[0]);
            double newratio = 1;
            loads[0].SetPositionRatio((float)newratio); // Ensure the load remain at the end of the beam, e.g. ratio = 1
            EnsureLoadGeometryStayFixed(loads[0]); // Ensure the load geometry remains fixed
            double scaleFactor = UpdatedLoadMagnitudeFromGeometry(loads[0]); // Update the load magnitude from the geometry scales
            force_scaled = force * scaleFactor; // Scale the force magnitude
            
            constraints[0].SetPositionRatio(0f); // Ensure the hinge remain fixed
            
            ratio_roller = constraints[1].GetPositionRatio(); 
            // Update intermediate positions to write the matrix
            //zF_s0 = ratio_roller * beamLength; // Roller support in the middle
            zF_s0 = ratio_roller * beamLength;
            zF_s1 = beamLength - zF_s0; // Fixed support at the end
            
            // Update the vector with the new force magnitude
            UpdateVector();
            UpdateMatrix();
            
            // Update the mathematical segment with the new absolute positions
            // absolute position contains each mathematica segmenet start and end
            absolutePositions = new double[] { 0, ratio_roller*beamLength, newratio*beamLength };
            mathematicalSegment.UpdateSegments(absolutePositions);
        }
        
    }

    void UpdateMatrix(){
        // Note that each row need exactly numCols elements

        // v(0) = 0
        double[] row1 = {0, 0, 0, 1/(E*I), 0, 0, 0, 0, 0, 0, 0, 0 };

        // w(0) = 0
        double[] row2 = {0, 0, 0, 0, 0, -1/(E*A), 0, 0, 0, 0, 0, 0 };

        // M(0) = 0
        double[] row3 = { 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // v(zF_s0) = 0
        double[] row4 = { 1/(E*I)*Math.Pow(zF_s0,3)/6,  1/(E*I)*Math.Pow(zF_s0,2)/2, 1/(E*I)*zF_s0,  1/(E*I), 0, 0, 0, 0, 0, 0, 0, 0 };

        // Delta_w() = w_s1(0) - w_s0(zF_s0) = 0
        double[] row5 = { 0, 0, 0, 0, -(-1/(E*A)) *zF_s0,  -(-1/(E*A)), 0, 0, 0, 0, 0, -1/(E*A) };

        // Delta_phi() = phi_s1(0) - phi_s0(zF_s0) = 0
        double[] row6 = { -(1/(E*I))*Math.Pow(zF_s0,2)/2, -(1/(E*I))*zF_s0, -(1/(E*I)), 0, 0, 0, 0, 0,  1/(E*I), 0, 0, 0 };

        // Delta_v() = v_s1(0) - v_s0(zF_s0) = 0
        double[] row7 = {-(1/(E*I))*Math.Pow(zF_s0,3)/6, -(1/(E*I))*Math.Pow(zF_s0,2)/2, -(1/(E*I))*zF_s0,  -(1/(E*I)), 0, 0, 0, 0, 0,  1/(E*I), 0, 0 };

        // Delta_M() = M_s1(0) - M_s0(zF_s0) = 0
        double[] row8 = { -(-zF_s0), -(-1), 0, 0, 0, 0, 0, -1, 0, 0, 0, 0 };

        // Delta_N() = N_s1(0) - N_s0(zF_s0) = 0
        double[] row9 = { 0, 0, 0, 0, -(-1), 0, 0, 0, 0, 0, -1, 0 };

        // M(zF_s1) = 0
        double[] row10 = {0, 0, 0, 0, 0, 0, -zF_s1, -1, 0, 0, 0, 0 };

        // T(zF_s1) = 0
        double[] row11 = {0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0 };

        // N(zF_s1) = 0
        double[] row12 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0 };

        // The following is necessary to ensure the order of the rows
        // and transform the matrix into the correct format
        // ensure only the rows are in the correct order and the total number of rows
        // match numRows. 

        // Set rows collection for the matrix
        List<double[]> rows = new List<double[]>
        {
            row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12
        };
            
        // reshape the matrix from [][] to [,]
        matrix = new double[numRows, numCols];
        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Length; j++)
            {
                matrix[i, j] = rows[i][j];
            }
        }
    }


    void UpdateVector()
    {
       // Define the vector
       // ensure only the rows are in the correct order and the total number of rows
       // match numRows.
       
        vector = new double[]
        {
           0, 0, 0, 0, 0, 0, 0, 0, 0, 0, force_scaled, 0
        };
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////
    // ///////////////////////////////////// Auxiliaries //////////////////////////////////////////////
    // ////////////////////////////////////////////////////////////////////////////////////////////////


    // Load auxiliary object for the 3D representation
    [Header("Reference")]

    public GameObject meshGenerator;
    public LoadGeometries loadGeometries;
    private List<Load> loads = new List<Load>(); // Editable in Inspector
    private List<Constraint> constraints = new List<Constraint>(); // Editable in Inspector
    private BeamPositioning beamPosition;
    public MathematicalSegment mathematicalSegment;



    double UpdatedLoadMagnitudeFromGeometry(Load load){
        GameObject loadObject = load.GetMagnitudeObject();
        LoadMovement loadMovement = loadObject.GetComponent<LoadMovement>();
        float scaleFac = loadMovement.GetActualScaleFactor();
        float magnitudeOriginal = load.GetMagnitude();
        float magnitude = scaleFac * magnitudeOriginal;
        return (double)magnitude;
    }

    void EnsureLoadGeometryStayFixed(Load load)
    {
        GameObject loadObject = load.GetPointerObject();
        LoadMovement loadMovement = loadObject.GetComponent<LoadMovement>();
        loadMovement.SetMovableQ(false);
    }

    double UpdateLoadPositionFromGeometry(Load load)
    {
        Vector3 basePoint = beamPosition.GetBaseCenter();
        Vector3 beamNormal = beamPosition.GetNormal();
        float beamLength = beamPosition.GetBeamLength();
        Vector3 position = basePoint + load.positionRatio * beamLength * beamNormal;
            //Vector3 position = new Vector3(0, 0, 0);

            
            if (load.GetPointerObject() != null)
            {   
                // for loads the position driver is the pointer:

                float positionRatioOld = load.positionRatio;
                GameObject pointerObejct = load.GetPointerObject();
                Vector3 objectPosition = pointerObejct.transform.position;
                float delta = (position - objectPosition).magnitude;
                if (delta > 0.01*beamLength){
                    float newRatio = (objectPosition - basePoint).magnitude / beamLength;
                    load.SetPositionRatio(newRatio);
                }
                
                load.SetPosition(objectPosition);   
                //constraintObject.transform.position = objectPosition;
                if (load.GetMagnitudeObject() != null)
                {
                    GameObject magnitudeObject = load.GetMagnitudeObject();
                    magnitudeObject.transform.position = objectPosition;
                }
            }
        return (double)loads[0].GetPositionRatio();
    }


    void UpdateBeamProperties()
    {
        BeamProperties beamProperties = GetComponentInParent<BeamProperties>();
        E = beamProperties.E;
        A = beamProperties.A;
        I = beamProperties.I;

        beamLength = meshGenerator.GetComponent<BeamPositioning>().GetBeamLength();
    }

    public double[,] GetMatrix()
    {
        return matrix;
    }

    public double[] GetVector()
    {
        return vector;
    }

    public double[] GetAbsolutePositions()
    {
        return absolutePositions;
    }

    public List<Load> GetLoads()
    {
        return loads;
    }

    public List<Constraint> GetConstraints()
    {
        return constraints;
    }
}
