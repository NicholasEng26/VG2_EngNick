using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        // Outlets
        public Transform povOrigin;

        // Confirugration
        public float attackRange;

        // State Tracking
        public List<int> keyIdsObtained;

        // Methods
        void PrimaryAttack()
        {
            RaycastHit hit;
            bool hitSomething = Physics.Raycast(povOrigin.position, povOrigin.forward, out hit, attackRange);
            if (hitSomething)
            {
                Rigidbody targetRigidbody = hit.transform.GetComponent<Rigidbody>();
                if (targetRigidbody)
                {
                    targetRigidbody.AddForce(povOrigin.forward * 100f, ForceMode.Impulse);
                }
            }
        }

        void Awake()
        {
            instance = this;
            keyIdsObtained = new List<int>();
        }
        void Update()
        {
            Keyboard keyboardInput = Keyboard.current;
            Mouse mouseInput = Mouse.current;
            if (keyboardInput != null && mouseInput != null)
            {

                // E KEY Interactions
                if (keyboardInput.eKey.wasPressedThisFrame)
                {
                    Vector2 mousePosition = mouseInput.position.ReadValue();

                    Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 2f))
                    {
                        // Debug: Test first-person interactions
                        print("Interacted with " + hit.transform.name + " from " + hit.distance + "m.");

                        // Doors
                        Door targetDoor = hit.transform.GetComponent<Door>();
                        if (targetDoor)
                        {
                            targetDoor.Interact();
                        }

                        // Buttons
                        InteractButton targetButton = hit.transform.GetComponent<InteractButton>();
                        if (targetButton != null)
                        {
                            targetButton.Interact();
                        }

                    }
                }

                // Left Mouse Interactions
                if (mouseInput.leftButton.wasPressedThisFrame)
                {
                    PrimaryAttack();
                }
            }
        }
    }
}
