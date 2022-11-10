using UnityEngine;
using System.Collections;

public enum AudioClipType{
	ButtonSelect 	= 	0,
	PourDrink		=	1,
	CollectGold		=	2,
}
public class SoundManager : MonoBehaviour {

	public static SoundManager ins;

	[Tooltip("Collection of all game sounds")]
	public AudioClip[] allclips;
	public AudioSource oneShotSource;
	public AudioSource BgSoundSource;
	// Use this for initialization
	void Awake () {
		ins = this;
	}

	public void playOneShotClip(AudioClipType clipType){
		oneShotSource.PlayOneShot (allclips[(int)clipType]);
	}
	public void stopOneShotClip(){
		if (oneShotSource.isPlaying)
			oneShotSource.Stop ();
	}

	public void stopBackgroudSound(){
		BgSoundSource.Stop();
	}
	public void playBackgroudSound(){
		BgSoundSource.Play();
	}

}
