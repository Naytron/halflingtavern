using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class GameData  {
	public string version;
	public int PlayerLives;
	public int PlayerDefaultGold; 
	public int[] DrinkTypeLevel;
	public int[] CupLevel;
	public int[] CustomerLevel;
	public int[] FoodLevel;
	public int[] DrinkAddInLevel;
	public List <Day> Days;

}

[System.Serializable]
public class Day{
	public int DayCoinGoal;
	public int DayCoinGoal2star;
	public int DayCoinGoal3star;
	public int DayCompleteTime;
	public int DrinkValueSmall;
	public int DrinkValueLarge;
	public int DrinkRuinedPenalty;
	public int DrinkTypeLevel;
	public int DrinkPrepTime;
	public int CupTypeLevel;
	public int FoodValueSmall;
	public int FoodValueLarge;
	public int FoodTypeLevel;
	public int FoodPrepTime;
	public int FoodRuinedPenalty;
	public int CustomerTypeLevel;
	public int CustomerMaxForDay;
	public int CustomerMaxAtOnce;
	public int CustomerWaitTime;
	public int CustomerDelayMin;
	public int CustomerDelayMax;
	public int OrderHalfCompleteTimeBonus;
	public float OrderJustDrinkPercent;
	public float OrderJustFoodPercent;
	public int DrinkAddInTypeLevel;
	public float DrinkAddInPercent;
	public int UnlockDayCoin;//R
}
