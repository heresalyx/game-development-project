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

    void Start()
    {
        PickNewDestination();
        StartCoroutine(CheckDestination());
    }

    // Update is called once per frame
    private IEnumerator CheckDestination()
    {
        while (true)
        {
            //if (NavMesh.SamplePosition(m_pathPoints[m_pathPoints.Count - 1].position, out NavMeshHit hit, 0.25f, NavMesh.AllAreas))
            Debug.Log("test");
            Debug.Log(!m_agent.pathPending);
            Debug.Log(m_agent.remainingDistance < 0.1);
            if (!m_agent.pathPending && m_agent.remainingDistance < 0.1)
            {
                m_animator.SetBool("is_Idle", true);
                yield return new WaitForSeconds(3.0f);
                PickNewDestination();
                m_animator.SetBool("is_Idle", false);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void PickNewDestination()
    {

        m_agent.SetDestination(m_pathPoints[m_currentPoint].position);
        if (m_isRandom)
            m_currentPoint = Random.Range(0, m_pathPoints.Count);
        else
            m_currentPoint++;
        if (m_currentPoint >= m_pathPoints.Count)
            m_currentPoint = 0;
    }
}
