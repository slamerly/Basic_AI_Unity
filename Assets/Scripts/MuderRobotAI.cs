using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MuderRobotAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private List<Transform> targets;

    public float distanceThrehold = 1.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(targets[0].position);
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < targets.Count; i++)
        {
            if (Vector3.Distance(transform.position, targets[i].position) <= distanceThrehold)
            {
                //agent.isStopped = true;
                if (i + 1 < targets.Count)
                    agent.SetDestination(targets[i + 1].position);
            }
        }
        
    }
}
