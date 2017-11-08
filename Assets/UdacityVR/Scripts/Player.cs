using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject mazeDoor;

    private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        StartCoroutine(enterMaze());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator enterMaze()
    {
        yield return new WaitUntil(() => originalPosition != transform.position);
        mazeDoor.SetActive(true);
    }
    
}
