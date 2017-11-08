using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hunt : MonoBehaviour {

    public BoxCollider player;

    private float radius = 8.0F;
    private Transform previousWaypoint = null;
    private Transform nextWaypoint = null;
    private Transform currentWaypoint = null;
    private float journeyLength;
    private float speed = 10.0F;
    private float previousTime;
    private bool turning = false;
    private bool moving = false;
    private float jumpHeight = 2.0F;
    private Collider collider;

	// Use this for initialization
	void Start () {
        Random.InitState((int)System.DateTime.Now.Ticks);
        collider = GetComponent<Collider>();
        currentWaypoint = transform;
        nextWaypoint = getNextWaypoint();
        turning = true;
    }

    // Update is called once per frame
    void Update () {

        if (turning)
        {
            Vector3 look = Vector3.RotateTowards(
                transform.forward, nextWaypoint.position - transform.position, speed * Time.deltaTime, 0.0F);

            if (transform.rotation != Quaternion.LookRotation(look))
            {
                transform.rotation = Quaternion.LookRotation(look);
            }
            else
            {
                turning = false;
                moving = true;
                journeyLength = Vector3.Distance(currentWaypoint.position, nextWaypoint.position);
                previousTime = Time.time;
            }

        }
        else if (moving)
        {
            float distance = (Time.time - previousTime) * speed;
            if (distance > journeyLength)
            {
                distance = journeyLength;
            }
            float fractionOfJourney = distance / journeyLength;

            if (fractionOfJourney != 1)
            {

                Vector3 nextPosition = new Vector3();
                nextPosition = Vector3.Lerp(currentWaypoint.position, nextWaypoint.position, fractionOfJourney);
                nextPosition.y = jumpHeight * Mathf.Sin(Mathf.PI * fractionOfJourney) + nextPosition.y;
                transform.position = nextPosition;
            }
            else
            {
                transform.position = Vector3.Lerp(currentWaypoint.position, nextWaypoint.position, fractionOfJourney);
                moving = false;
            }
        }
        else
        {
            if (player.transform.position == transform.position)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            previousWaypoint = currentWaypoint;
            currentWaypoint = nextWaypoint;
            nextWaypoint = getNextWaypoint();
            turning = true;
        }

    }

    private Transform getNextWaypoint()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        List<Transform> waypoints = new List<Transform>();
        foreach (Collider c in colliders)
        {
            if (c.tag == "MazePoint" || c.tag == "Player")
            {
                waypoints.Add(c.transform);
            }
        }

        collider.enabled = false;
        List<Transform> visibleWaypoints = new List<Transform>();
        foreach (Transform w in waypoints)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, w.position, out hit))
            {
                if (hit.collider.gameObject.tag == "MazePoint" || hit.collider.gameObject.tag == "Player")
                {
                    visibleWaypoints.Add(w);
                }
            }

        }
        collider.enabled = true;

        if (visibleWaypoints.Contains(currentWaypoint))
        {
            visibleWaypoints.Remove(currentWaypoint);
        }
        if (visibleWaypoints.Contains(previousWaypoint) && visibleWaypoints.Count != 1)
        {
            visibleWaypoints.Remove(previousWaypoint);
        }


        int index = Random.Range(0, visibleWaypoints.Count);
        return visibleWaypoints[index];
    }

}
