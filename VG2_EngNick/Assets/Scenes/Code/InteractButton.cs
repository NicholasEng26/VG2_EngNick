using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class InteractButton : MonoBehaviour
    {
        // Configuration
        public GameObject interactionTarget;

        // Methods
        public void Interact()
        {
            if (interactionTarget != null)
            {
                // Doors
                Door targetDoor = interactionTarget.GetComponent<Door>();
                if (targetDoor != null)
                {
                    targetDoor.Interact(gameObject); // Pass ourselves as the sender
                }

                // Lights
                InteractLight targetLight = interactionTarget.GetComponent<InteractLight>();
                if (targetLight != null)
                {
                    targetLight.Interact();
                }
            }


        }
    }
}
