using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingZombie : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject zombie;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = zombie.transform.rotation;
        transform.position = zombie.transform.position;
    }
}
