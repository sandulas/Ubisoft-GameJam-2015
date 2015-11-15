using UnityEngine;
using System.Collections;

public class TheSound : MonoBehaviour {

	static TheSound instance;
	static GameObject parentObject;
	public static TheSound GetInstance()
	{
		if( !instance )
		{
			parentObject = GameObject.Find("TheSound");
			if (parentObject != null)
				instance  = parentObject.GetComponent<TheSound>();
			else
				Debug.Log("WTF : no instance found for TheSound");
		}
		return instance;
	}

	public AudioClip soundMusic;

	public AudioClip soundBlockUp;
	public AudioClip soundBlock;

	public AudioClip soundWin;
	public AudioClip soundFail;

	AudioSource source;
	void Start(){

		source = GetComponent<AudioSource>();
		source.clip = soundMusic;
		source.Play();



	}

	public void PlaySoundBlockUp(){
		if (soundBlockUp != null)
			source.PlayOneShot(soundBlockUp);
	}

	public void PlaySoundBlock(){
		if (soundBlock != null)
			source.PlayOneShot(soundBlock);
	}

	public void PlaySoundWin(){
		if (soundWin != null)
			source.PlayOneShot(soundWin);
	}

	public void PlaySoundFail(){
		if (soundFail != null)
			source.PlayOneShot(soundFail);
	}
}
