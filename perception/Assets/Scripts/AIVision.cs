using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIVision : MonoBehaviour
{
    public Camera frustum;
    public LayerMask mask;


    public GameObject zPrefab;
    public int numZ;
    public GameObject[] allZ;

    public bool hasSeen;


    public NavMeshAgent agent;

    public int radius = 1, offset = 1;

    void Start()
    {
        allZ = new GameObject[numZ];
        for (int i = 0; i < numZ; ++i)
        {
            Vector3 randomVector = new Vector3(0, 0, 0);
            Vector3 pos = this.transform.position + randomVector; // random position
            Vector3 randomize = new Vector3(UnityEngine.Random.Range(-5f, 5f), 2, UnityEngine.Random.Range(-5f, 5f)); // random vector direction
            allZ[i] = (GameObject)Instantiate(zPrefab, pos, Quaternion.LookRotation(randomize));
            //allZ[i].GetComponent<Flocking>().myManager = this;
        }
    }


    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, frustum.farClipPlane, mask);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);

        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject && GeometryUtility.TestPlanesAABB(planes, col.bounds))
            {
                RaycastHit hit;
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = (col.transform.position - transform.position).normalized;
                ray.origin = ray.GetPoint(frustum.nearClipPlane);

                if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        
                        //Wander2();
                        BroadcastMessage("Seek", hit.collider.gameObject.transform.position);

                        for (int i = 0; i < numZ; ++i)
                        {
                            allZ[i].SendMessage("Seek", hit.collider.gameObject.transform.position);
                        }

                        Seek(hit.collider.gameObject.transform.position);
                    }

            }
        }
    }


    void Seeing()
    {
        hasSeen = true;
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

        UnityEngine.AI.NavMeshHit hit;


        if (UnityEngine.AI.NavMesh.SamplePosition(worldTarget, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            Seek(hit.position);
        else
        {
            Seek(Vector3.zero);
        }
    }
}
