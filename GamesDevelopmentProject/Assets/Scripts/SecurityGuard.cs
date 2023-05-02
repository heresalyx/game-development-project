using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityGuard : MonoBehaviour
{
    public AudioSource m_effectSource;
    public List<Transform> m_pathPoints;
    public NavMeshAgent m_agent;
    public Animator m_animator;
    public bool m_isRandom;
    private int m_currentPoint = 0;
    private bool m_hasRequestedPoint = true;
    private Vector3 m_currentDestination;
    private static bool m_isPlayerDead = false;
    private bool m_isPlaying = false;

    // Request the first destination.
    void Start()
    {
        StartCoroutine(PickNewDestination());
    }

    private void Update()
    {
        // If not yet at the destination and haven't requested a point, travel towards it.
        if (Vector3.Distance(m_currentDestination, m_pathPoints[m_currentPoint].position) > 1.0f && !m_hasRequestedPoint)
        {
            m_currentDestination = m_pathPoints[m_currentPoint].position;
            m_agent.destination = m_currentDestination;
        }

        // If doing nothing and have reached the destination, request a new point.
        if (!m_hasRequestedPoint && !m_agent.pathPending && m_agent.remainingDistance < 0.1)
        {
            m_hasRequestedPoint = true;
            StartCoroutine(PickNewDestination());
        }

        if (m_animator.GetBool("is_Idle") == false && !m_isPlaying)    // Play sound effect if walking.

        {
            m_isPlaying = true;
            m_effectSource.Play();
        }
        else if (m_animator.GetBool("is_Idle") == true && m_isPlaying)    // Stop sound effect if idle.

        {
            m_isPlaying = false;
            m_effectSource.Stop();
        }
    }

    // Stay idle for 3 seconds, before deciding on a new destination.
    private IEnumerator PickNewDestination()
    {
        m_animator.SetBool("is_Idle", true);
        yield return new WaitForSeconds(3.0f);
        m_animator.SetBool("is_Idle", false);
        if (m_isRandom)
            m_currentPoint = Random.Range(0, m_pathPoints.Count - 1);
        else
            m_currentPoint++;
        if (m_currentPoint >= m_pathPoints.Count - 1)
            m_currentPoint = 0;
        m_hasRequestedPoint = false;
        m_agent.speed = 1.5f;
        m_agent.acceleration = 1.0f;
    }

    // If the player is detected, on the guard's area of detection, and isn't dead. Chase after the robot at a faster pace.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Robot") && NavMesh.SamplePosition(new Vector3(other.transform.position.x, 0, other.transform.position.z), out NavMeshHit hit, 0.25f, NavMesh.AllAreas) && !m_isPlayerDead)
        {
            StopAllCoroutines();
            m_animator.SetBool("is_Idle", false);
            m_currentPoint = m_pathPoints.Count - 1;
            m_hasRequestedPoint = false;
            m_agent.speed = 2.0f;
            m_agent.acceleration = 2.0f;
        }
    }

    // Destroy the guard if the player is dead.
    public void KilledPlayer()
    {
        m_isPlayerDead = true;
        Destroy(gameObject);
    }
}
