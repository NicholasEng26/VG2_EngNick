using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class InteractLight : MonoBehaviour
    {
        public void Interact()
        {
            // Flip/toggle the current active state
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
