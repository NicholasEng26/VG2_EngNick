using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Door : MonoBehaviour
    {
        // Outlets
        Animator animator;

        // Configuration
        public GameObject requiredSender;
        public int keyIdRequired = -1;  // Default -1 means no key required

        // Methods
        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Interact(GameObject sender = null)
        {
            bool shouldOpen = false;

            // Is this a valid interaction?
            if (!requiredSender)
            {
                shouldOpen = true;
            }
            else if (requiredSender == sender)
            {
                shouldOpen = true;
            }

            // Check required key if other requirements met
            bool keyRequired = keyIdRequired >= 0;
            bool keyMissing = !PlayerController.instance.keyIdsObtained.Contains(keyIdRequired);
            if (keyRequired && keyMissing)
            {
                shouldOpen = false;
            }

            if (shouldOpen)
            {
                animator.SetTrigger("Open");
            }
        }

    }
}
