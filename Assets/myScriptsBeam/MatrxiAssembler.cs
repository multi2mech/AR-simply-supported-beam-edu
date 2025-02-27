using System;
using System.Collections.Generic;
// using System.Diagnostics;
using System.Linq;
using Structure;
using UnityEngine;
public class MatrixAssembler
{
    private EQ_IV eqns = new EQ_IV();
    public MatrixResult AssembleMatrix(List<MathematicalSegment.Segment> segments)
    {
        int noSegments = segments.Count;
        int noCoeff = 6; // Number of coefficients per segment
        int noUnknowns = noSegments * noCoeff;

        int noBcs = segments.Sum(segment => segment.BCs.Count());

        // Initialize the matrix and vector
        double[,] matrix = new double[noBcs, noUnknowns];
        double[] vector = new double[noBcs];

        int currentRow = 0;

        foreach (var segment in segments)
        {
            
            Debug.Log($"Caricato il segmento {segment.Index}");
            Debug.Log($"E: {segment.E}, I: {segment.I}, A: {segment.A}, p: {segment.p}, q: {segment.q}, Length: {segment.Length}");
            Debug.Log($"Condizioni al contorno:");
            foreach (var bc in segment.BCs)
            {
                BC_TYPE bcType = bc.Type;
                double z = 0;
                ConstraintPosition pos = bc.Position;
                bool InternalQ = bc.InternalQ;  
                Debug.Log($"tipo: {bcType} con InternalQ: {InternalQ}");
                
                if (pos == ConstraintPosition.End)
                {
                    z = segment.Length;
                }
                // else remain zero
                double value = bc.Value;

                // Compute Arow and brow using the EQ_IV class
                //var (ArowTmp, browTemp) = eqns.Eqn(bcType, z: z, E: E, I: I, A: A, p: p, q: q);
                var result = eqns.Eqn(bcType, z: z, segment : segment);
                
                Debug.Log($"tipo: {bcType} in posizione {pos} con valore: {value}");
                // Add the computed values to the matrix and vector
                if (!InternalQ) { 
                    
                    
                    Debug.Log($"Arow: {string.Join(", ", result.Arow.Select(a => $"{a,10:E2}"))}, Brow: {result.brow,10:E2}");
                    for (int i = 0; i < noCoeff; i++)
                    {
                        int colIndex = segment.Index * noCoeff + i;
                        //matrix[currentRow, colIndex] = ArowTmp[i];
                       
                        matrix[currentRow, colIndex] = result.Arow[i];
                    }
    
                    //vector[currentRow] = browTemp + value;
                    vector[currentRow] = result.brow + value;
                }else
                {
                    // conditions is set as start, so is + actual - previous
                    double[] Arow = new double[6];
                    for (int i = 0; i < noCoeff; i++)
                    {
                        int colIndex = segment.Index * noCoeff + i;
                        //matrix[currentRow, colIndex] = ArowTmp[i];
                        //Arow[i] = - result.Arow[i];
                        Arow[i] = result.Arow[i];
                        matrix[currentRow, colIndex] =  Arow[i];
                    }
                    // double brow_temp = - result.brow;
                    double brow_temp = result.brow;
                    
                    Debug.Log($"Arow: {string.Join(", ", Arow.Select(a => $"{a,10:E2}"))}, Brow: {brow_temp,10:E2}");
                    //vector[currentRow] = browTemp + value;
                    
                    
                    //XXXX

                    // load other segmenet
                    
                    MathematicalSegment.Segment previousSegment = segments
                    .Where(s => s.Index < segment.Index) // Find segments with a lower index
                    .OrderByDescending(s => s.Index)     // Sort by index in descending order
                    .FirstOrDefault();                 // Take the first (smallest higher index) segment
                    float previousPosition = (float)previousSegment.Length;
                    Debug.Log($"Caricato il segmento precedente {previousSegment.Index}");
                    Debug.Log($"Previous position: {previousPosition}");
                    result = eqns.Eqn(bcType, z: previousPosition, segment: previousSegment);
                    
                    for (int i = 0; i < noCoeff; i++)
                        {
                            int colIndex = (segment.Index-1) * noCoeff + i;
                            //matrix[currentRow, colIndex] = ArowTmp[i];
                            //matrix[currentRow, colIndex] = result.Arow[i]; 
                            matrix[currentRow, colIndex] = -result.Arow[i]; 
                        }
                    
                    Debug.Log($"Arow: {string.Join(", ", result.Arow.Select(a => $"{a,10:E2}"))}, Brow: {result.brow,10:E2}");
                    //brow_temp = brow_temp + value;
                    brow_temp -= result.brow;
                    brow_temp += value;
                    vector[currentRow] = brow_temp;

                    
                    
                }
                currentRow++;
            }

            
        }


        
        return new MatrixResult { Matrix = matrix, Vector = vector };

    }
}

public class MatrixResult
{
    public double[,] Matrix { get; set; } // Multidimensional array for the matrix
    public double[] Vector { get; set; }  // 1D array for the vector
}

