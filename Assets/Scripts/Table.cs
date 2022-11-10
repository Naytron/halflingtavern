using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour {

	public static Table instance;
	CustomerSeat[] AllSeats;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		if (AllSeats == null)
			getSeats ();
	}

	void getSeats(){
		AllSeats = GetComponentsInChildren<CustomerSeat> ();
		allotSeatId ();		
	}

	void allotSeatId(){
		for(int i=0;i<AllSeats.Length;i++){
			AllSeats[i].seatId = i+1;
		}
	}

	public CustomerSeat getSeatById(int sId){
		if (AllSeats == null)
			getSeats ();
		for(int i=0;i<AllSeats.Length;i++){
			if(AllSeats[i].seatId == sId)
				return AllSeats[i];
		}
		return null;
	}

	public void clarTable(){
		if (AllSeats == null)
			getSeats ();
		for(int i=0;i<AllSeats.Length;i++){
			int cCount = AllSeats[i].transform.childCount;
			if(cCount>0){
				for(int j=0;j<cCount;j++)
					Destroy(AllSeats[i].transform.GetChild(j).gameObject);
			}
			AllSeats[i].isSeatVacant = true;
		}
	}
}
