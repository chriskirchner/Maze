using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpawner : MonoBehaviour {

    public AudioClip[] soundFiles;
    public int delayTime;
    public int sleepTime;
    private AudioSource soundSource;

	// Use this for initialization
	void Start () {
        soundSource = gameObject.GetComponent<AudioSource>();
        Random.InitState((int)System.DateTime.Now.Ticks);
        StartCoroutine(DelayToStartAudio(soundSource, soundFiles, delayTime));
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void PlayAudio(AudioSource audio, AudioClip[] clips)
    {
        bool sameClip = true;
        while (sameClip)
        {
            int index = Random.Range(0, soundFiles.Length);
            if (soundSource.clip != clips[index])
            {
                sameClip = false;
                soundSource.clip = clips[index];
                soundSource.Play();
                StartCoroutine(WaitToPlayAudio(audio, clips, sleepTime));
            }

        }

    }


    private IEnumerator DelayToStartAudio(AudioSource audio, AudioClip[] clips, int delay)
    {
        yield return new WaitForSeconds(delay);
        PlayAudio(audio, clips);
    }

    private IEnumerator WaitToPlayAudio(AudioSource audio, AudioClip[] clips, int delay)
    {
        yield return new WaitUntil(() => !audio.isPlaying);
        yield return new WaitForSeconds(delay);
        PlayAudio(audio, clips);
    }
}
