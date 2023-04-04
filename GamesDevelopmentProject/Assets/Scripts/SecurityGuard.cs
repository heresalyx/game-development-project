using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityGuard : MonoBehaviour
{
    public List<Transform> m_pathPoints;
    public NavMeshAgent m_agent;
    public Animator m_animator;
    public bool m_isRandom;
    private int m_currentPoint = 0;
    private bool m_hasRequestedPoint = true;
    private Vector3 m_currentDestination;
    private static bool m_isPlayerDead = false;

    void Start()
    {
        StartCoroutine(PickNewDestination());
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector3.Distance(m_currentDestination, m_pathPoints[m_currentPoint].position) > 1.0f && !m_hasRequestedPoint)
        {
            m_currentDestination = m_pathPoints[m_currentPoint].position;
            m_agent.destination = m_currentDestination;
        }


        if (!m_hasRequestedPoint && !m_agent.pathPending && m_agent.remainingDistance < 0.1)
        {
            Debug.Log("Request");
            m_hasRequestedPoint = true;
            StartCoroutine(PickNewDestination());
        }
    }

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
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Detected Trigger");

        if (other.gameObject.CompareTag("Robot") && NavMesh.SamplePosition(new Vector3(other.transform.position.x, 0, other.transform.position.z), out NavMeshHit hit, 0.25f, NavMesh.AllAreas) && !m_isPlayerDead)
        {
            StopAllCoroutines();
            m_animator.SetBool("is_Idle", false);
            Debug.Log("Detected Player Trigger");
            m_currentPoint = m_pathPoints.Count - 1;
        }
    }

    public void KilledPlayer()
    {
        m_isPlayerDead = true;
        Destroy(gameObject);
    }
}
