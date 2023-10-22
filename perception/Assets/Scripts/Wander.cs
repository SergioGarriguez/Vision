using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    public NavMeshAgent agent;

    public int radius, offset;
    // Start is called before the first frame update
    void Start()
    {
        Wander2();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 2f) Wander2();
        
    }


    void Seek(Vector3 pos)
    {
        agent.destination = pos;
    }

    void Wander2()
    {
        Vector3 localTarget = UnityEngine.Random.insideUnitCircle * radius;
        localTarget += new Vector3(0, 0, offset);
        Vector3 worldTarget = transform.TransformPoint(localTarget);
        worldTarget.y = 0f;

        NavMeshHit hit;


        if (NavMesh.SamplePosition(worldTarget, out hit, 1.0f, NavMesh.AllAreas))
            Seek(hit.position);
        else
        {
            Seek(Vector3.zero);
        }
    }
};
