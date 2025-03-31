.. _index:

Welcome to Augmented Reality for beams project documentation!
====================================================================

Introduction
------------

Education project combining the use of Augmented Reality (AR) for the study of beams theory.

.. image:: /_static/promo.gif
   :alt: Showcase
   :height: 350px

Project Structure
-----------------

.. warning::
   This repository is still under active development and was primarily written live during hands-on teaching sessions. As such, parts of the code may be rough, inconsistent, or temporarily out of place. Some files originate from a more complex parent project, and the structure still needs to be cleaned and streamlined. Use it as a learning resource, but expect changes and improvements over time! 

Repository can be cloned from the following link:

.. code-block:: bash

   git clone https://github.com/multi2mech/AR-simply-supported-beam-edu.git

.. note::
   The project is based on Unity for the AR development and a `custom educational tool for beam theory on C# <https://github.com/multi2mech/simply-supported-beam-edu>`_. 
   
Main requirements to start with the project:
""""""""""""""""""""""""""""""""""""""""""""""""
 - `Unity <https://unity.com/download>`_. 
 - `Android SDK tool for Unity <https://docs.unity3d.com/540/Documentation/Manual/android-sdksetup.html>`_. 
 - `Meta XR Simulator <https://developers.meta.com/horizon/documentation/unity/xrsim-intro/>`_. 
 

 


Main components:
""""""""""""""""""""""""""""""

The proejct structure is as follows:

- :ref:`Geometries` - 3D geometries of loads and contraints (limited for now)
- Scripts with automatic generation of beam given its section, initial point and final points. It also incluse a ear clipping-based trinagulation algorithm.
- Scripts to solve the beam elasticity eqautions given the desired loading scenario.
- Automatic relations between beam, loads and constraints.
- Interactions between controller ray casting and 3D object (you can move loads or constraints)
- Object textures

.. code-block:: text

   project/
   │
   ├── T00_Common/                # Contains general documentation and common scripts
   │    │
   │    ├── py/                 
   │    │    ├── myPatientsModulesV2/     # Contains modules related to patient data processing
   │    │    │   ├── patientMain.py       # Main script for patient data handling
   │    │    │   └── nrrdAux.py           # Auxiliary functions for NRRD file processing
   │    │    │
   │    │    ├── mesh-importer_v2/        # Contains mesh conversion tools
   │    │    │   └── meshConversion.py    # Main script for mesh conversion
   │    │    │
   │    │    ├── myModulesV2/             # General utility modules
   │    │    │   ├── postResults.py       # Functions to post-process results
   │    │    │   └── importMeshFromTXT.py # Functions to import meshes from TXT files
   │    │    │
   │    │    └── myAceplaque/             # Contains core meshing tools
   │    │        └── mesher.py            # Main meshing script
   │    ├── Elements/
   │    │   ├── Elements.nb
   │    │   ├── BoundaryPressureP2   
   │    │   └── O2P1YG   
   │    │
   │    ├── auxFncs/
   │    │   ├── myExpRoutines.m
   │    │   ├── myFileName.m
   │    │   ├── myLoading.m
   │    │   ├── myPostAuxialiars.m   
   │    │   └── Kernel/
   │    │
   │    ├── createMPHmesh.m
   │    └── generate-interpolation-materials.wls
   │    
   ├── T01_Patient0/              # Contains patient-specific data
   │     ├── mesh/               # Patient-specific data
   │     │   ├── ...
   │     │
   │     └── growth.nb/
   │
   └── TXY_PatientABC





.. toctree::
   :maxdepth: 2
   :caption: Unity

   Unity/Unity
   Unity/beamGenerator
   Unity/Geometries
   
.. toctree::
   :maxdepth: 2
   :caption: Beam theory

   Beam/BeamTheory
   Beam/BeamTheoryWithC#
   
.. toctree:: 
   :maxdepth: 2
   :caption: Extra

   FutureDevelopment
   references
   
   
