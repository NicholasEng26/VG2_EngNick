using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RTS {
    public class RTSGameController : MonoBehaviour
    {
        // State Tracking
        public GameObject currentSelection;

        // Methods
        void Update() {
            // Mouse Interactions
            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                // Selection Command
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    // Get all objects under mouse cursor
                    Ray selectionRaycast = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
                    RaycastHit[] hits = Physics.RaycastAll(selectionRaycast);

                    // See if any are selectable
                    currentSelection = null;
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.GetComponent<RTSCharacterController>() != null)
                        {
                            currentSelection = hit.collider.gameObject;
                        }
                    }
                }

                // Interaction Command
                if (mouse.rightButton.wasPressedThisFrame && currentSelection)
                {
                    RTSCharacterController character = currentSelection.GetComponent<RTSCharacterController>();
                    if (character)
                    {
                        // Get all objects under the mouse cursor
                        Ray selectionRaycast = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
                        RaycastHit[] hits = Physics.RaycastAll(selectionRaycast);

                        // Check for possible interactions
                        foreach (RaycastHit hit in hits)
                        {
                            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                            {
                                // Move player to destination
                                character.SetDestination(hit.point);
                            }
                        }
                    }
                }
            }
        }

    }
}
