using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MuderRobotAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private List<Transform> targets;

    public float distanceThrehold = 1.0f;
    private int current = 0, delay = 3;
    private GameObject enemy;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextTaget();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targets[current].position) <= distanceThrehold)
        {
            current = (current + 1) % targets.Count;
            Invoke("MoveToNextTaget", delay);
        }

        if (enemy != null)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= distanceThrehold)
            {
                enemy.GetComponent<MuderRobotAI>().Freeze();
            }
        }
    }

    void Freeze()
    {
        agent.isStopped = true;
    }

    void MoveToNextTaget()
    {
        agent.SetDestination(targets[current].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Robot")
        {
            enemy = other.gameObject;
            agent.SetDestination(other.transform.position);
        }
    }
}
