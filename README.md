# Augmented Reality Beam Model

Have a look at the [graphical presentation here](https://multi2mech.github.io/AR-simply-supported-beam-edu/)!

This educational project integrates Augmented Reality (AR) using Meta Quest headsets with beam elasticity theory. The goal is to provide an AR/VR-based visualization of fundamental problems related to beam mechanics. As a learning tool, it begins with defining the boundary conditions of an elastic beam. [simply-supported-beam-edu](https://github.com/multi2mech/simply-supported-beam-edu) for more details on the C# code for the beam equations.

## How to Use

Simply download [Unity](https://unity.com/download), clone the repository, and open it!

### Main compontnes:

- [3D geometries](Assets/my3Dgeometries/) of loads and contraints (limited for now)
- Scripts with [automatic generation of beam](Assets/myScriptsBeam/meshGenerator.cs) given its section, initial point and final points. It also incluse a ear clipping-based trinagulation algorithm.
- Scripts to [solve the beam elasticity](Assets/myScriptsBeam/StructuralSolver.cs) eqautions given the desired loading scenario.
- Automatical relation between beam, loads and constraints.
- Interactions scripts between [controllar ray casting](Assets/myScriptsInteractions/) and 3D object (you can move loads or constraints)
- Object textures
