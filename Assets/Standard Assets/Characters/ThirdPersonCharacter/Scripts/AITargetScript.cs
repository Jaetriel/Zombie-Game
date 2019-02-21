using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Linq;
using UnityEngine.AI;

public class AITargetScript : MonoBehaviour
{
    public int deathTimeout = 5;
    private AICharacterControl characterController;
    //GameObject hitTarget;
    private enum AIState { WANDERING,CHASING,DEAD };
    private GameObject[] allWaypoints;
    private int currentWaypoint = 0;
    private ThirdPersonCharacter tpCharacter;
    private AIState state = AIState.WANDERING;


    protected bool CanSeePlayer()
    {
        //function to determine whether the AI character can see the player

        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position; // get player position

        //we only want to look ahead so we check if the player in a 90 degree arc. (Vec3.angle returns an absolute value, so 45 degrees either way means <=45)
        if (Vector3.Angle(transform.forward, playerPosition - transform.position) <= 45f)
        {
            LayerMask layerMask = LayerMask.NameToLayer("Zombie");// a mask used for the raycast to ignore any zombies

            RaycastHit hit;// variable to store the hit so we can check it

            //We now check if a ray cast from us (the zombie) to the player hits anything (except zombies) also move it up a little to avoid the floor
            if (Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), playerPosition - transform.position, out hit, Mathf.Infinity, layerMask))
            {
                return (hit.collider.tag.Equals("Player"));//return whether or not the thing we hit is the player
            }
        }//or if any of these tests failed, i can't see them.
        return false;
    }

    public void kill()
    {
        this.state = AIState.DEAD;
    }


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<AICharacterControl>();
        //hitTarget = GameObject.Find("AI Target");
        tpCharacter = GetComponent<ThirdPersonCharacter>();
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        //shuffle array to make unique wandering path
        System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);
        allWaypoints = allWaypoints.OrderBy(x => rnd.Next()).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AIState.WANDERING:

                characterController.SetTarget(allWaypoints[currentWaypoint].transform);
                Debug.Log(currentWaypoint);
                if ((Vector3.Distance(characterController.target.transform.position, transform.position) < 2.0f))
                {
                    //...make me target the next one
                    currentWaypoint++;
                    //make sure that we don't fall off the end of the array but lop back round
                    currentWaypoint %= allWaypoints.Length;
                }

                //can I see the player? if so, the chase is on!
                if (CanSeePlayer())
                {
                    state = AIState.CHASING;
                }
                break;
            case AIState.CHASING:

                //set target to player and go back to wandering if you cant see him
                characterController.SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
                if (!CanSeePlayer())
                {
                    //i can't see him, so back to wandering...
                    state = AIState.WANDERING;
                }
                break;
            case AIState.DEAD:

                this.GetComponent<Animator>().enabled = false; //stop trying to animate myself
                this.GetComponent<NavMeshAgent>().enabled = false; //stop trying to navigate
                this.GetComponent<AICharacterControl>().enabled = false; // stop the AI

                this.GetComponent<Rigidbody>().isKinematic = true; //Make myself kinematic (ragdoll)

                foreach (Rigidbody rbody in GetComponentsInChildren<Rigidbody>())
                {
                    rbody.isKinematic = false;
                }

                foreach (Collider col in GetComponentsInChildren<Collider>())
                {
                    col.enabled = true;
                }

                Destroy(gameObject, deathTimeout); // remove myself from the game after timeout

                break;
        
                /*
                if (state == AIState.WANDERING)
        {
            characterController.SetTarget(allWaypoints[currentWaypoint].transform);
            Debug.Log(currentWaypoint);
            if ((Vector3.Distance(characterController.target.transform.position, transform.position) < 2.0f))
            {
                //...make me target the next one
                currentWaypoint++;
                //make sure that we don't fall off the end of the array but lop back round
                currentWaypoint %= allWaypoints.Length;
            }
            //can I see the player? //if so, the chase is on!
            if (CanSeePlayer())
            {
                state = AIState.CHASING;
            }
            */
        }
    
    }
}
