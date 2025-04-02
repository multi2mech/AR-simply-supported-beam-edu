.. _index:

Welcome to Augmented Reality for beams project documentation!
====================================================================

Introduction
------------

Education project combining the use of Augmented Reality (AR) for the study of beams theory.

.. image:: /_static/promo.gif
   :alt: Showcase
   :height: 350px

.. tip::
   Access the `the repository here <https://github.com/multi2mech/AR-simply-supported-beam-edu>`_. 


Project Structure
-----------------

.. warning::
   *This repository is still under active development and was primarily written live during hands-on teaching sessions*. As such, parts of the code may be rough, inconsistent, or temporarily out of place. Some files originate from a more complex parent project, and the structure still needs to be cleaned and streamlined. Use it as a learning resource, but expect changes and improvements over time! 



Repository can be cloned from the following link:

.. code-block:: bash

   git clone https://github.com/multi2mech/AR-simply-supported-beam-edu.git

.. note::
   The project is based on Unity for the AR development and a `custom educational tool for beam theory on C# <https://github.com/multi2mech/simply-supported-beam-edu>`_. 
   
Requirements
""""""""""""""""""""""""""""""""""""""""""""""""
 - `Unity <https://unity.com/download>`_. 
 - `Android SDK tool for Unity <https://docs.unity3d.com/540/Documentation/Manual/android-sdksetup.html>`_. 
 - `Meta XR Simulator <https://developers.meta.com/horizon/documentation/unity/xrsim-intro/>`_. 
 

 


Main components
""""""""""""""""""""""""""""""

The project structure is as follows:

- :ref:`Geometries` - 3D geometries of loads and contraints (limited for now)
- Scripts with automatic generation of beam given its section, initial point and final points. It also incluse a ear clipping-based trinagulation algorithm.
- Scripts to solve the beam elasticity eqautions given the desired loading scenario.
- Automatic relations between beam, loads and constraints.
- Interactions between controller ray casting and 3D object (you can move loads or constraints)
- Object textures

.. code-block:: text

   root/
   │
   ├── Assets/       # Contains general documentation and common scripts
   │    │
   │    ├── my3Dgeometries/        # Contains 3D geometries of loads and constraints
   │    │
   │    ├── myScriptBeam/          # Contains scripts with automatic generation of beam given its section, initial point and final points. It also incluse a ear clipping-based trinagulation algorithm.
   │    │
   │    ├── myScriptInteractions/  # Contains script to move object following the Meta XRay interaction
   │    │
   │    ├── myScriptsMaterialsAndGeometries/   # Containts script and assets to work with materials and shaders
   │    │   
   │    └── Scenes/                # Contains the Unity scenes (main one and the initial one with the logo)
   │    
   └──  docs/                       # Documentation and resources
         ├── documentation/        # Sphinx based documentation
         │
         └── index.html            # Landing page
   
   
Table of contents
------------------

.. toctree::
   :maxdepth: 2
   :caption: Unity

   
   Unity/beamGenerator
   Unity/Geometries
   Unity/Constraints
   Unity/Loads
   Unity/RayCasting
   Unity/Unity
   
   
.. toctree::
   :maxdepth: 2
   :caption: Beam theory

   Beam/BeamTheory
   Beam/BeamTheoryWithC#
   
.. toctree:: 
   :maxdepth: 2
   :caption: Extra

   AR
   FutureDevelopment
   references
   
   
