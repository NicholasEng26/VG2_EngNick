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

            if (shouldOpen)
            {
                animator.SetTrigger("Open");
            }
        }
    }
}
