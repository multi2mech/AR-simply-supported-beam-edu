using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Structure;
using System;
using Oculus.Interaction.Input;

public class MathematicalSegment : MonoBehaviour
{
    public BeamProperties beamProperties; // Reference to the LoadScheme component
    private List<IPositionable> orderedElments = new List<IPositionable>();

    private List<IPositionable> allElements = new List<IPositionable>();

    private List<Segment> segments = new List<Segment>();
    // Start is called before the first frame update
    void Start()
    {    
    }
    public List<Segment> GetSegments(){
        return segments;
    }
    public void SetResults(List<double> coefficientList){
        
        for (int i = 0; i < segments.Count; i++)
        {   int noCoeff = 6;
            segments[i].c1 = coefficientList[noCoeff*i+0];
            segments[i].c2 = coefficientList[noCoeff*i+1];
            segments[i].c3 = coefficientList[noCoeff*i+2];
            segments[i].c4 = coefficientList[noCoeff*i+3];
            segments[i].c5 = coefficientList[noCoeff*i+4];
            segments[i].c6 = coefficientList[noCoeff*i+5];
        }

    }

    public void UpdateSegments(double[] absolutePositions){
        
        segments.Clear();
        int noOfSegments = absolutePositions.Length-1;
        for (int i = 0; i < noOfSegments; i++)
        {
            Segment segment = new Segment();
            segment.Start = absolutePositions[i];
            segment.End = absolutePositions[i+1];
            segment.Index = i;
            segment.E = beamProperties.E;
            segment.A = beamProperties.A;
            segment.I = beamProperties.I;
            segments.Add(segment);
        }

    }


    public class Segment{
        public int Index;
        public double E  = 1f;
        public double A = 1f;
        public double I = 1f;
        private double start;
        public double c1 = 0;
        public double c2 = 0;
        public double c3 = 0;
        public double c4 = 0;
        public double c5 = 0;
        public double c6 = 0;
        public double Start           // Start position of the segment
        {
            get => start;
            set
            {
                start = value;
                UpdateLength();
            }
        }

        private double end;
        public double End             // End position of the segment
        {
            get => end;
            set
            {
                end = value;
                UpdateLength();
            }
        }
        
        private double length;
        public double Length => length; 


        private void UpdateLength()
        {
            length = Math.Abs(End - Start); // Ensure length is positive
        }
        
        public List<BoundaryCondition> BCs = new List<BoundaryCondition>();
        public double p = 0;
        public double q = 0;
        public double qInt => q * Length;    // Replace with numerical integration
        public double qIInt => q * Math.Pow(Length,2)/2;    // Replace with numerical integration
        public double qIIInt => q * Math.Pow(Length,3)/6;    // Replace with numerical integration
        public double qIIIInt => q * Math.Pow(Length,4)/24;    // Replace with numerical integration
        public double pInt => p * Length;      // Replace with numerical integration

    }
    

    


}


