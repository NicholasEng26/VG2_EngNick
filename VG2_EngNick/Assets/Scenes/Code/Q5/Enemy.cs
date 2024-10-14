using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    public class Enemy : MonoBehaviour
    {
        // Outlets
        NavMeshAgent navAgent;

        // Configuration
        public Transform priorityTarget;
        public Transform target;
        public Transform patrolRoute;

        // State Tracking
        int patrolIndex;
        public float chaseDistance;

        // Methods
        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (patrolRoute)
            {
                // Which patrol point is active?
                target = patrolRoute.GetChild(patrolIndex);

                // How far is the patrol point?
                float distance = Vector3.Distance(transform.position, target.position);
                print("Distance: " + distance); // DEBUG distance so we can configure a threshold.

                // Target the next point when we are close enough
                if (distance <= 2.5f)
                {
                    patrolIndex++;
                    if (patrolIndex >= patrolRoute.childCount)
                    {
                        patrolIndex = 0;
                    }
                }
            }

            if (priorityTarget)
            {
                // Keep track of our priority target
                float priorityTargetDistance = Vector3.Distance(transform.position, priorityTarget.position);

                // If the priority target gets too close, follow it and highlight ourselves
                if (priorityTargetDistance <= chaseDistance)
                {
                    target = priorityTarget;
                    GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    GetComponent<Renderer>().material.color = Color.white;
                }
            }

            if (target)
            {
                navAgent.SetDestination(target.position);
            }
        }

    }
}
