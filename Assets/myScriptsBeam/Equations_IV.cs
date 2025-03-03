using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathNet.Numerics.RootFinding;
using Structure;
using UnityEngine;

public class EQ_IV
{
    public EQ_IV()
    {
        // Constructor logic (if needed)
    }

    public (double[] Arow, double brow) Eqn(BC_TYPE eqType, double? z = null, MathematicalSegment.Segment segment = null)
    {
        double? E = (double)segment.E;
        double? I = (double)segment.I;
        double? A = (double)segment.A;
        double? p = (double)segment.p;
        double? q = (double)segment.q;
        double? Length = (double) (segment.Length);
        
        if ((eqType == BC_TYPE.v) || (eqType == BC_TYPE.delta_v))
        {
            if (z.HasValue && E.HasValue && I.HasValue && q.HasValue)
                return VFun(z.Value, E.Value, I.Value, q.Value);
        }
        else if ((eqType == BC_TYPE.v1) || (eqType == BC_TYPE.delta_phi))
        {
            if (z.HasValue && E.HasValue && I.HasValue && q.HasValue)
                return V1Fun(z.Value, E.Value, I.Value, q.Value);
        }
        else if (eqType == BC_TYPE.M || eqType == BC_TYPE.delta_M)
        {
            if (z.HasValue && E.HasValue && I.HasValue && q.HasValue)
                return M(z.Value, E.Value, I.Value, q.Value);
        }
        else if (eqType == BC_TYPE.T || eqType == BC_TYPE.delta_T)
        {
            if (z.HasValue && E.HasValue && I.HasValue && q.HasValue)
                return T(z.Value, E.Value, I.Value, q.Value);
        }
        else if (eqType == BC_TYPE.v4)
        {
            if (z.HasValue && E.HasValue && I.HasValue && q.HasValue)
                return V4Fun(z.Value, E.Value, I.Value, q.Value);
        }
        else if ((eqType == BC_TYPE.w) || (eqType == BC_TYPE.delta_w))
        {
            if (z.HasValue && E.HasValue && A.HasValue && p.HasValue)
                return WFun(z.Value, E.Value, A.Value, p.Value);
        }
        else if (eqType == BC_TYPE.N || eqType == BC_TYPE.delta_N)
        {
            if (z.HasValue && E.HasValue && A.HasValue && p.HasValue)
                return NFun(z.Value, E.Value, A.Value, p.Value);
        }
        else if (eqType == BC_TYPE.w2)
        {
            if (z.HasValue && E.HasValue && A.HasValue && p.HasValue)
                return W2Fun(z.Value, E.Value, A.Value, p.Value);
        }
        else
        {
            throw new ArgumentException($"Unknown BC type: {eqType}");
        }

        throw new ArgumentException("Invalid or missing parameters.");
    }

    public (double[] Arow, double brow) VFun(double z, double E, double I, double q)
    {
        double[] Arow = { 1 / (E * I) * Math.Pow(z, 3) / 6, 1 / (E * I) * Math.Pow(z, 2) / 2, 1 / (E * I) * z, 1 / (E * I), 0, 0 };
        double brow = -1 * (q * Math.Pow(z, 4)) / (24 * E * I);
        return (Arow, brow);
    }

    public (double[] Arow, double brow) V1Fun(double z, double E, double I, double q)
    {
        double[] Arow = { 1 / (E * I) * Math.Pow(z, 2) / 2, 1 / (E * I) * z, 1 / (E * I), 0, 0, 0 };
        double brow = -1 * (q * Math.Pow(z, 3)) / (6 * E * I);
        return (Arow, brow);
    }

    public (double[] Arow, double brow) M(double z, double E, double I, double q)
    {
        double[] Arow = { -1 * z, -1 , 0, 0, 0, 0 };
        double brow = (q * Math.Pow(z, 2)) / (2 );
        return (Arow, brow);
    }

    public (double[] Arow, double brow) T(double z, double E, double I, double q)
    {
        double[] Arow = { -1 , 0, 0, 0, 0, 0 };
        double brow = (q * z) ;
        return (Arow, brow);
    }

    public (double[] Arow, double brow) V4Fun(double z, double E, double I, double q)
    {
        double[] Arow = { 0, 0, 0, 0, 0, 0 };
        double brow = -1 * q / (E * I);
        return (Arow, brow);
    }

    public (double[] Arow, double brow) WFun(double z, double E, double A, double p)
    {
        double[] Arow = { 0, 0, 0, 0, -1 / (E * A) * z, -1 / (E * A) };
        double brow = (p * Math.Pow(z, 2)) / (E * A);
        return (Arow, brow);
    }

    public (double[] Arow, double brow) W1Fun(double z, double E, double A, double p)
    {
        double[] Arow = { 0, 0, 0, 0, -1 / (E * A), 0 };
        double brow = (p * z) / (E * A);
        return (Arow, brow);
    }
    public (double[] Arow, double brow) NFun(double z, double E, double A, double p)
    {
        double[] Arow = { 0, 0, 0, 0, -1 , 0 };
        double brow = (p * z) ;
        return (Arow, brow);
    }

    public (double[] Arow, double brow) W2Fun(double z, double E, double A, double p)
    {
        double[] Arow = { 0, 0, 0, 0, 0, 0 };
        double brow = p / (E * A);
        return (Arow, brow);
    }

    public float Get_v(float distanceAlongBeam, MathematicalSegment.Segment segment)
    {
        double v = 0.0f;
        double z = (double)distanceAlongBeam - segment.Start;
        double E = segment.E;
        double I = segment.I;
        double c1 = segment.c1;
        double c2 = segment.c2;
        double c3 = segment.c3;
        double c4 = segment.c4;
        double qIIIInt = segment.qIIIInt;
        //UnityEngine.Debug.Log("E: " + E + " I: " + I + " z: " + z + " c1: " + c1 + " c2: " + c2 + " c3: " + c3 + " c4: " + c4 + " qIIIInt: " + qIIIInt);
       
        v = (1/E*I) * (qIIIInt +c1* Math.Pow(z, 3) / 6 + c2 * Math.Pow(z, 2) / 2 + c3 * z + c4);
        // UnityEngine.Debug.Log("v: " + v);
        return (float)v;
    }

    public float Get_M_normalized(float distanceAlongBeam, MathematicalSegment.Segment segment)
    {
        double M = 0.0f;
        double z = (double)distanceAlongBeam - segment.Start;
        double E = segment.E;
        double I = segment.I;
        double c1 = segment.c1;
        double c2 = segment.c2;
       
        M = (1/E*I) * (c1* z + c2);
        
        return (float)M;
    }
}