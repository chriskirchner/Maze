using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour 
{
    //Create a reference to the KeyPoofPrefab and Door
    public GameObject KeyPoof;
    public GameObject Door;

    private bool keyCollected = false;

    void Update()
	{
        //Not required, but for fun why not try adding a Key Floating Animation here :)
        transform.RotateAround(transform.position, Vector3.up, 360.0f * Time.deltaTime / 2.0f);
	}

	public void OnKeyClicked()
	{
        // Instatiate the KeyPoof Prefab where this key is located
        GameObject poof = Instantiate(KeyPoof, gameObject.transform);
        // Make sure the poof animates vertically

        AudioSource audio = poof.GetComponent<AudioSource>();
        audio.Play();

        ParticleSystem ps = poof.GetComponent<ParticleSystem>();
        ps.Play();

        // Call the Unlock() method on the Door
        Door door = Door.GetComponent<Door>();
        door.Unlock();

        // Set the Key Collected Variable to true
        keyCollected = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(WaitUntilDone(ps, audio));

    }
    private IEnumerator WaitUntilDone(ParticleSystem ps, AudioSource audio)
    {
        yield return new WaitUntil(() => !ps.isPlaying && !audio.isPlaying);

        // Destroy the key. Check the Unity documentation on how to use Destroy
        Destroy(gameObject, 0);
    }

}

