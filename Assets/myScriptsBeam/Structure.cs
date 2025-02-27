using System;
using System.Linq;

namespace Structure
{
    public class BoundaryCondition
    {
        public BC_TYPE Type { get; set; } // Boundary condition type
        public ConstraintPosition Position { get; set; } // Specify position as Start or End
        public double Value { get; set; } // Value of the boundary condition
        // Computed property for InternalQ
        public bool InternalQ
        {
             get
            {
                // Return true if the Type matches any of the specified "delta_" types
                return Type == BC_TYPE.delta_v ||
                    Type == BC_TYPE.delta_w ||
                    Type == BC_TYPE.delta_phi ||
                    Type == BC_TYPE.delta_M ||
                    Type == BC_TYPE.delta_N ||
                    Type == BC_TYPE.delta_T;
            }
        }
    }
    
    public enum ConstraintPosition
    {
        Start,
        End
    }
    
    
    public enum BC_TYPE
    {
        v,
        v1,
        v4,
        M,
        T,
        w,
        w2,
        N,
        delta_v,
        delta_w,
        delta_phi,
        delta_M,
        delta_N,
        delta_T,
    }
    


    
    }