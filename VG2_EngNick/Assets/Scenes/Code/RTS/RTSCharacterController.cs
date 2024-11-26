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
        public GameObject selectionIndicator;
        public GameObject attackTarget;
        public GameObject projectilePrefab;

        // Configuration
        public float attackDistance;
        public float attackDelay;

        // State Tracking
        float _timeSinceLastAttack;


        // Methods
        public void Select()
        {
            selectionIndicator.SetActive(true);
        }

        public void Deselect()
        {
            selectionIndicator.SetActive(false);
        }
        void Start()
        {
            animator = GetComponent<Animator>();
            navAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            animator.SetFloat("velocity", navAgent.velocity.magnitude);

            // Increment attack timer
            _timeSinceLastAttack += Time.deltaTime;

            // Prepare to get in range of target
            if (attackTarget)
            {
                // (NEXT CODE SNIPPET GOES IN HERE)
            }

            // Prepare to attack
            SetDestination(attackTarget.transform.position);
            if (Vector3.Distance(attackTarget.transform.position, transform.position) <= attackDistance)
            {
                // (NEXT CODE SNIPPET GOES IN HERE)
            }

            // Perform Attack
            if (_timeSinceLastAttack >= attackDelay)
            {
                // Reset attack timer
                _timeSinceLastAttack = 0;

                // Prepare attack
                // For prototype visibility: Attack from slightly above the player
                Vector3 projectileOrigin = transform.position + new Vector3(0, 2f, 0);
                Vector3 directionToTarget = (attackTarget.transform.position - projectileOrigin).normalized;

                // Shoot projectile
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    projectileOrigin,
                    Quaternion.LookRotation(directionToTarget)
                );

                // For prototype visibility: Make the projectile large
                projectile.transform.localScale = Vector3.one * 10f;
                projectile.GetComponent<Rigidbody>().AddForce(directionToTarget * 50f, ForceMode.Impulse);

                // Cleanup projectile
                Destroy(projectile, 10f);
            }
        }

        public void SetDestination(Vector3 targetPosition)
        {
            navAgent.destination = targetPosition;
        }

        public void SetTarget(GameObject target)
        {
            attackTarget = target;
        }

    }
}
