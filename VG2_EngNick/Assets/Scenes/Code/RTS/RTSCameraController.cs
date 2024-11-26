using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RTS
{
    public class RTSCameraController : MonoBehaviour
    {
        // Configuration
        public float moveSpeed;

        // State Tracking
        Vector3 movement;

        // Methods
        void OnMove(InputValue value)
        {
            Vector2 direction = value.Get<Vector2>();
            movement = new Vector3(direction.x, 0, direction.y);
        }

        void Update()
        {
            // Compute target position based on input
            Vector3 checkPosition = transform.position + movement * moveSpeed * Time.deltaTime;

            // Move up a little to there is room to raycast downward at the ground
            // You can adjust this number as a threshold for vertical landscapes that should also block the camera
            checkPosition += new Vector3(0, 10f, 0);

            // Check if new camera position would be above ground
            bool hitGround = Physics.Raycast(checkPosition, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

            if (hitGround)
            {
                transform.position += movement * moveSpeed * Time.deltaTime;
            }
        }

    }
}
