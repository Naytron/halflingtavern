
using UnityEngine;
using System.Collections;
using System;

public class MyPrefereces : MonoBehaviour {

	public const string KEY_PREFERENCES_INITIALIZED 	= 	"key_player_preferences_initialized";
	public const string KEY_CURRENT_PLAYING_DAY 		= 	"key_current_playing_day";
	public const string KEY_CURRENT_UNLOCKED_DAY 		= 	"key_current_unlocked_day";
	public const string KEY_IS_STORY_PLAYED		 		= 	"key_is_story_played";
	public const string KEY_IS_DAY3_TUT_DONE		 	= 	"key_is_day3_tut_done";
	public const string KEY_TOTAL_COINS			 		= 	"key_total_coins";
	public const string KEY_POWERUP_COUNT_SPEED	 		= 	"key_powerup_count_speed";
	public const string KEY_POWERUP_COUNT_LIFE	 		= 	"key_powerup_count_life";
	public const string KEY_POWERUP_COUNT_BOOT	 		= 	"key_powerup_count_boot";
	public const string KEY_POWERUP_COUNT_DOUBLE_COIN	= 	"key_powerup_count_double_coin";
//	public const string KEY_PLAYER_CURRENT_LIFE			= 	"key_player_current_life";


	// Use this for initialization
	void Awake () {
		initializePreferences ();
	}

	void initializePreferences(){
		if (PlayerPrefs.GetInt (KEY_PREFERENCES_INITIALIZED, 0) == 0) {
			print("Prefs initialized");
			PlayerPrefs.SetInt (KEY_PREFERENCES_INITIALIZED, 1);
			PlayerPrefs.SetInt (KEY_IS_STORY_PLAYED, 0);
			PlayerPrefs.SetInt (KEY_IS_DAY3_TUT_DONE, 0);
			PlayerPrefs.SetInt (KEY_CURRENT_PLAYING_DAY, 1);
			PlayerPrefs.SetInt (KEY_CURRENT_UNLOCKED_DAY, 1);
			PlayerPrefs.SetInt (KEY_TOTAL_COINS  , 1000);
			PlayerPrefs.SetInt (KEY_POWERUP_COUNT_SPEED  , 20);
			PlayerPrefs.SetInt (KEY_POWERUP_COUNT_LIFE  , 20);
			PlayerPrefs.SetInt (KEY_POWERUP_COUNT_BOOT  , 20);
			PlayerPrefs.SetInt (KEY_POWERUP_COUNT_DOUBLE_COIN  , 20);
//			PlayerPrefs.SetInt (KEY_PLAYER_CURRENT_LIFE  , 3);
			PlayerPrefs.Save();
		}
	}	

	public void resetDays(){
		PlayerPrefs.SetInt (KEY_IS_STORY_PLAYED, 0);
		PlayerPrefs.SetInt (KEY_CURRENT_PLAYING_DAY, 1);	
		Application.LoadLevel (Application.loadedLevel);
//		PopUpsManager.instance.openScreen (MyScreens.Splash);	
	}
// Update is called once per frame
//	void Update () {
//	
//	}
}
