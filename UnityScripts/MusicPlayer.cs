using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {

    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> clips=new List<AudioClip>();
    private int currClipIndex = 0;
    private bool playing=false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if(!playing)
        {
            audioSource.clip = clips[currClipIndex];
            audioSource.Play();

            StartCoroutine(NextClipAfter(audioSource.clip.length));
            playing = true;
        }
	}

    IEnumerator NextClipAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (currClipIndex < clips.Count - 1) currClipIndex++;
        else currClipIndex = 0;
        playing = false;
    }
}
