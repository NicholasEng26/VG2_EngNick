using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS
{
    public class RTSCharacterController : MonoBehaviour
    {

        // Outlets
        Animator animator;
        NavMeshAgent navAgent;

        // Methods
        void Start()
        {
            animator = GetComponent<Animator>();
            navAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            animator.SetFloat("velocity", navAgent.velocity.magnitude);
        }

        public void SetDestination(Vector3 targetPosition)
        {
            navAgent.destination = targetPosition;
        }
    }
}
