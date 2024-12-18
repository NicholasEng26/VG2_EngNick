using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RTS {
    public class RTSGameController : MonoBehaviour
    {
        // Outlets
        public Canvas uiCanvas;
        public RectTransform selectionBox;

        // Configuration
        float mouseDragThreshold = 3f;

        // State Tracking
        public List<GameObject> currentSelection;
        public Vector2 mouseClickStart;

        // Methods
        void Update() {
            // Mouse Interactions
            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                // Start Drag

                if (mouse.leftButton.wasPressedThisFrame)
                {
                    mouseClickStart = mouse.position.ReadValue();
                }

                // Handle Drag
                if (mouse.leftButton.isPressed)
                {
                    Vector2 mousePosition = mouse.position.ReadValue();
                    if (Vector2.Distance(mouseClickStart, mousePosition) > mouseDragThreshold)
                    {
                        selectionBox.gameObject.SetActive(true);

                        // Compute Midpoint and Assign Position
                        Vector2 boxMidpoint = Vector2.Lerp(mouseClickStart, mousePosition, 0.5f);
                        selectionBox.anchoredPosition = boxMidpoint / uiCanvas.scaleFactor;

                        // Compute Drag Bounds and Assign Size
                        Vector2 box = new Vector2(
                            Mathf.Abs(mouseClickStart.x - mousePosition.x),
                            Mathf.Abs(mouseClickStart.y - mousePosition.y)
                        );
                        selectionBox.sizeDelta = box / uiCanvas.scaleFactor;
                    }
                }

                // Selection Command
                if (mouse.leftButton.wasReleasedThisFrame)
                {
                    if (selectionBox.gameObject.activeInHierarchy)
                    {
                        // Mouse Drag = Get all eligible objects
                        SelectWithinBox();
                    }
                    else
                    {
                        // Single Click
                        SelectUnderMouse();
                    }

                    // Hide selection box
                    selectionBox.gameObject.SetActive(false);
                }

                // Interaction Command
                if (mouse.rightButton.wasPressedThisFrame && currentSelection.Count > 0)
                {
                    foreach (GameObject selection in currentSelection)
                    {
                        RTSCharacterController character = selection.GetComponent<RTSCharacterController>();
                        if (character)
                        {
                            // Get all objects under the mouse cursor
                            Ray selectionRaycast = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
                            RaycastHit[] hits = Physics.RaycastAll(selectionRaycast);

                            // Check for possible interactions
                            foreach (RaycastHit hit in hits)
                            {
                                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                                {
                                    character.SetTarget(hit.collider.gameObject);
                                    break;
                                }

                                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                                {
                                    character.SetTarget(null);
                                    character.SetDestination(hit.point);
                                }
                            }

                        }
                    }
                }
            }
        }
        void SelectUnderMouse()
        {
            // Get all objects under mouse cursor
            Ray selectionRaycast = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.RaycastAll(selectionRaycast);

            // See if any are selectable
            DeselectAll();
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<RTSCharacterController>())
                {
                    currentSelection.Add(hit.collider.gameObject);
                    hit.collider.SendMessage("Select", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        void SelectWithinBox()
        {
            // TODO: Expensive. In a real production game, you would maintain an ongoing list instead
            RTSCharacterController[] characterControllers = FindObjectsOfType<RTSCharacterController>();

            // Loop through each eligible object to see if it's within our selection box boundaries
            DeselectAll();
            DeselectAll();
            foreach (RTSCharacterController character in characterControllers)
            {
                // Translate its 3D position to 2D screen space
                Vector2 characterPosition = Camera.main.WorldToScreenPoint(character.transform.position);

                // Adjust screen space for any UI scaling
                characterPosition = characterPosition / uiCanvas.scaleFactor;

                // Build a rectangle that represents where our selection box is on screen
                Rect anchoredRect = new Rect(
                    selectionBox.anchoredPosition.x - selectionBox.sizeDelta.x / 2f,
                    selectionBox.anchoredPosition.y - selectionBox.sizeDelta.y / 2f,
                    selectionBox.sizeDelta.x,
                    selectionBox.sizeDelta.y
                );

                // Does the character fall inside the box
                if (anchoredRect.Contains(characterPosition))
                {
                    currentSelection.Add(character.gameObject);
                    character.SendMessage("Select", SendMessageOptions.DontRequireReceiver );
                }
            }
        }
        void DeselectAll()
        {
            foreach (GameObject selection in currentSelection)
            {
                selection.SendMessage("Deselect", SendMessageOptions.DontRequireReceiver);
            }

            currentSelection.Clear();
        }

    }
}
