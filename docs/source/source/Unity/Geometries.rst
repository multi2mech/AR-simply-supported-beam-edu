.. _Geometries:

Geometries
============

Some usefull 3D geometries are included in the project. They are located in the folder ``Assets/my3Dgeometries``. The included geometries are the intial pointers, the force (arrow point and body) and several constraints:

 - Hinge
 - Roller
 - Fixed

.. raw:: html

    <style>
        .three-column {
            display: flex;
            gap: 20px;
        }

        .column {
            flex: 1;
        }

        iframe {
            width: 100%;
            height: 400px;
            border: 1px solid #ccc;
        }
    </style>

.. raw:: html

    <div class="three-column">
        <div class="column">
            <iframe src="/documentation/_static/hinge.html"></iframe>
        </div>
        <div class="column">
            <iframe src="/documentation/_static/roller.html"></iframe>
        </div>
        <div class="column">
            <iframe src="/documentation/_static/fixed.html"></iframe>
        </div>
    </div>

Note that all the goemetries miss the common "cylinder" because it is used as a second object to marker the position. 

.. raw:: html

    <div class="three-column">
        <div class="column">
            <iframe src="/documentation/_static/common_c.html"></iframe>
        </div>
    </div>

Similarly, the force is splitted into body and arrow head.

.. raw:: html

    <div class="three-column">
        <div class="column">
            <iframe src="/documentation/_static/arrow.html"></iframe>
        </div>
        <div class="column">
            <iframe src="/documentation/_static/arrow_body.html"></iframe>
        </div>
    </div>

.. toctree::
  :maxdepth: 5  