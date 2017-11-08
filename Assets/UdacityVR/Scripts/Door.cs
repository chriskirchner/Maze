using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour 
{

    public AudioClip DoorLocked;
    public AudioClip DoorOpening;

    // Create a boolean value called "locked" that can be checked in OnDoorClicked() 
    private bool locked = true;
    // Create a boolean value called "opening" that can be checked in Update() 
    private bool opening = false;

    private AudioSource audio = null;

    private void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    void Update() {
        // If the door is opening and it is not fully raised
        if (opening && !locked)
        {
            // Animate the door raising up
            transform.Translate(0, 2.5f * Time.deltaTime, 0);
        }
    }

    public void OnDoorClicked() {
        // If the door is clicked and unlocked
        if (!locked)
        {
            // Set the "opening" boolean to true
            opening = true;
            audio.clip = DoorOpening;
        }
        else
        {
            // (optionally) Else
            // Play a sound to indicate the door is locked
            audio.clip = DoorLocked;
        }
        audio.Play();
    }

    public void Unlock()
    {
        // You'll need to set "locked" to false here
        locked = false;
    }

    //IEnumerator Play(AudioClip clip)
    //{
    //    AudioSource audio = GetComponent<AudioSource>();

    //    audio.Play();
    //    yield return new WaitForSeconds(audio.clip.length);
    //    audio.clip = clip;
    //    audio.Play();
    //}
}
