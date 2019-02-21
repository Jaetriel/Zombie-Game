using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public int numberOfZombies = 5;//How many zombies do we want in our game?

    private GameObject[] allWaypoints;// list to store all waypoints
    private GameObject zombiePrefab;

    // Use this for initialization
    void Start()
    {
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        zombiePrefab = GameObject.FindGameObjectWithTag("Zombie");
        zombiePrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int nrZombiesExist = GameObject.FindGameObjectsWithTag("Zombie").Length;
        while (nrZombiesExist < numberOfZombies)
        {

            //instatiates a copy of zombiePrefab. I places it at the position of one of the waypoints (randomly picks an index in the waypoint array). It gives it a new rotation and adds the object that this script is attached to as parent
            Instantiate(zombiePrefab, allWaypoints[Random.Range(0, allWaypoints.Length - 1)].transform.position, new Quaternion(), transform).SetActive(true);
            //count them manually instead of calling FindGameObjectsWithTag every time
            nrZombiesExist++;
        }
    }
}
