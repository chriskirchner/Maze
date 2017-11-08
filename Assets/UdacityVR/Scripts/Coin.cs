using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour 
{
    //Create a reference to the CoinPoofPrefab
    public GameObject CoinPoof;

    public void OnCoinClicked() {
        // Instantiate the CoinPoof Prefab where this coin is located
        GameObject poof = Instantiate(CoinPoof, gameObject.transform);

        // Make sure the poof animates vertically
        AudioSource audio;
        audio = poof.GetComponent<AudioSource>();
        audio.Play();

        ParticleSystem ps;
        ps = poof.GetComponent<ParticleSystem>();
        ps.Play();

        // Destroy this coin. Check the Unity documentation on how to use Destroy
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(WaitUntilDone(ps, audio));
    }

    private IEnumerator WaitUntilDone(ParticleSystem ps, AudioSource audio)
    {
        yield return new WaitUntil(() => !ps.isPlaying && !audio.isPlaying);
        Destroy(gameObject, 0);
    }

}
