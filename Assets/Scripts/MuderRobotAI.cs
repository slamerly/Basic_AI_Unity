using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MuderRobotAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public List<Transform> targets;

    //public bool touched = false;
    public float distanceThrehold = 1.0f;
    public int delayBeforeNextDetection = 5;
    private int current = 0, delay = 3;
    public float tmp = 0;
    private GameObject enemy;
    private bool isRedTeam = false, saveEnemyColor = false;

    void Start()
    {
        GameObject[] GameObjectTargets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in GameObjectTargets)
        {
            targets.Add(target.transform);
        }
        System.Random rand = new System.Random();
        targets = targets.OrderBy(e => rand.Next()).ToList();

        if (gameObject.tag == "RedTeam")
            isRedTeam = true;
        ChangeColor(isRedTeam);

        agent = GetComponent<NavMeshAgent>();
        MoveToNextTaget();
    }

    void Update()
    {
        tmp += Time.deltaTime;
        if (Vector3.Distance(transform.position, targets[current].position) <= distanceThrehold)
        {
            current = (current + 1) % targets.Count;
            Invoke("MoveToNextTaget", delay);
        }

        if(enemy != null && enemy.GetComponent<MuderRobotAI>().isRedTeam != saveEnemyColor)
        {
            enemy = null;
            MoveToNextTaget();
        }

        if (tmp >= delayBeforeNextDetection && enemy != null)
        {
            agent.SetDestination(enemy.transform.position);
            if (Vector3.Distance(transform.position, enemy.transform.position) <= distanceThrehold)
            {
                tmp = 0;
                enemy.GetComponent<MuderRobotAI>().Freeze();
                //Invoke("MoveToNextTaget", delay);
                enemy = null;
                MoveToNextTaget();
            }
        }
    }

    void Freeze()
    {
        //touched = true;
        enemy = null;
        //agent.isStopped = true;
        Debug.Log("Catch a " + gameObject.tag);

        if (isRedTeam)
        {
            isRedTeam = false;
            gameObject.tag = "BlueTeam";
        }
        else
        {
            isRedTeam = true;
            gameObject.tag = "RedTeam";
        }

        tmp = 0;

        ChangeColor(isRedTeam);

        Invoke("MoveToNextTaget", delay);
    }

    void MoveToNextTaget()
    {
        //touched = false;
        agent.SetDestination(targets[current].position);
    }

    void ChangeColor(bool isRedTeam)
    {
        if (isRedTeam)
        {
            GameObject bodyRobot = transform.GetChild(0).gameObject;
            for (int i = 0; i < bodyRobot.transform.childCount; i++)
            {
                bodyRobot.transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
            bodyRobot.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
        else
        {
            GameObject bodyRobot = transform.GetChild(0).gameObject;
            for (int i = 0; i < bodyRobot.transform.childCount; i++)
            {
                bodyRobot.transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
            }
            bodyRobot.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (tmp >= delayBeforeNextDetection)
        {
            if (isRedTeam)
            {
                if (other.gameObject.tag == "BlueTeam")
                {
                    enemy = other.gameObject;
                    saveEnemyColor = false;
                }
            }
            else
            {
                if (other.gameObject.tag == "RedTeam")
                {
                    enemy = other.gameObject;
                    saveEnemyColor = true;
                }
            }
        }
    }
}
