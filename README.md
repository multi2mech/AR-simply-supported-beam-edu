# Augmented Reality Beam Model for Education

Have a look at the [graphical presentation here](https://multi2mech.github.io/AR-simply-supported-beam-edu/)!

This educational project integrates Augmented Reality (AR) using Meta Quest headsets with beam elasticity theory. The goal is to provide an AR/VR-based visualization of fundamental problems related to beam mechanics. As a learning tool, it begins with defining the boundary conditions of an elastic beam. [simply-supported-beam-edu](https://github.com/multi2mech/simply-supported-beam-edu) for more details on the C# code for the beam equations.

![promo.gif](https://github.com/multi2mech/AR-simply-supported-beam-edu/blob/main/docs/promo.gif?raw=true)

## How to Use

Read the [documentation here](https://multi2mech.github.io/AR-simply-supported-beam-edu/documentation).

Simply download [Unity](https://unity.com/download), clone the repository, and open it!

You may need to install:
- [Android SDK tool for Unity](https://docs.unity3d.com/540/Documentation/Manual/android-sdksetup.html)
- [Meta XR Simulator](https://developers.meta.com/horizon/documentation/unity/xrsim-intro/)

### Main compontnes:

- [3D geometries](Assets/my3Dgeometries/) of loads and contraints (limited for now)
- Scripts with [automatic generation of beam](Assets/myScriptsBeam/meshGenerator.cs) given its section, initial point and final points. It also incluse a ear clipping-based trinagulation algorithm.
- Scripts to [solve the beam elasticity](Assets/myScriptsBeam/StructuralSolver.cs) eqautions given the desired loading scenario.
- Automatic relations between beam, loads and constraints.
- Interactions between [controller ray casting](Assets/myScriptsInteractions/) and 3D object (you can move loads or constraints)
- Object textures

## Disclaimer

⚠️⚠️ This repository is still under active development and was primarily written live during hands-on teaching sessions. As such, parts of the code may be rough, inconsistent, or temporarily out of place. Some files originate from a more complex parent project, and the structure still needs to be cleaned and streamlined. Use it as a learning resource, but expect changes and improvements over time! ⚠️⚠️

## Extra

This is a hands-on educational project designed for exploring beam elasticity theory in Augmented Reality (AR) and Virtual Reality (VR). You can download and experiment with the Unity project, modifying it to fit your needs. Alternatively, you can directly [download the .apk](https://github.com/multi2mech/AR-simply-supported-beam-edu/releases) file and install it on your Meta Quest headset for immediate use.
