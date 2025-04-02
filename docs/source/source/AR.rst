.. _AR:

Augmented Reality
==================

.. raw:: html

    <div style="width: 600px; height: auto; position: relative;">
    <script type="module" src="https://unpkg.com/@google/model-viewer/dist/model-viewer.min.js"></script>
    <!-- Model Viewer Component -->
    <model-viewer 
        id="arViewer"
        auto-rotate
        camera-controls
        autoplay ar ar-modes="webxr scene-viewer quick-look" scale="0.2 0.2 0.2" 
        src="../beam.glb" 
        alt="3D Beam Model"
        environment-image="neutral"
        exposure="1.1"
        skybox-color="black"
        orientation="0 0 -0.7071 0.7071"
        style="width: 100%; height: 500px;"
        >
        <div slot="annotation">
            <button style="
                position: absolute; bottom: 10px; left: 50%;
                transform: translateX(-50%);
                background-color: white; padding: 10px;
                border-radius: 5px; border: none;">
                🌟 View in AR
            </button>
        </div>
        <!-- AR Button -->
        <button slot="ar-button" style="
            position: absolute;
            top: 20px;
            left: 50%;
            transform: translateX(-50%);
            background-color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
    ">
    🚀 View in AR
        </button>
    </model-viewer>
    </div>
    <script type="importmap">
        {
          "imports": {
            "three": "https://cdn.jsdelivr.net/npm/three@^0.172.0/build/three.module.min.js"
          }
        }
        </script>
    <script>
        window.onload = function() {
            const viewer = document.querySelector("#arViewer");
        
            if (!viewer) {
                console.error("Model-viewer element not found!");
                return;
            }
        
            viewer.addEventListener("load", () => {
                console.log("Model loaded, checking animations...");
                console.log("Available animations:", viewer.availableAnimations);
        
                // Define the animations you want
                const selectedAnimations = ["arrow_body.001Action", "Key|constraint_hinge.001|Action", "MAction"];
                let index = 0;
        
                function playNextAnimation() {
                    if (selectedAnimations.length > 0) {
                        viewer.animationName = selectedAnimations[index];
                        viewer.play();
                        console.log("Playing animation:", selectedAnimations[index]);
        
                        // Move to the next animation
                        index = (index + 1) % selectedAnimations.length;
        
                        // Change animation every 5 seconds
                        setTimeout(playNextAnimation, 8000);
                    } else {
                        console.warn("No matching animations found!");
                    }
                }
        
                // Start playing
                playNextAnimation();
            });
        };
    </script>
    



.. toctree::
  :maxdepth: 5  