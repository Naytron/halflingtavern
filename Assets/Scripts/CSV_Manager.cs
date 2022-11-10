using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSV_Manager : MonoBehaviour {

	public static CSV_Manager ins;

	[SerializeField] TextAsset csvFile;
	public string[,] storyData;
//	const int totalCol = 4;
	public bool isFileLoaded;
	public static int RowCount = 0 , ColumnCount = 4;

	// Use this for initialization
	void Awake () {
		ins = this;
//		print(" "+csvFile.text);
//		readFile();	
	}

	string str1;
	bool isCommaStr;
	int colNo;
	public void readFile(){
		isFileLoaded = false;
		string[] allRow = csvFile.text.Split(new string[]{"\n"} ,System.StringSplitOptions.RemoveEmptyEntries);
		storyData = new string[allRow.Length,ColumnCount];
		RowCount = allRow.Length ;

		for(int i=0;i<allRow.Length;i++){
			//split row into columns
			string[] col = allRow[i].Split(new char[]{','} ,System.StringSplitOptions.RemoveEmptyEntries);

			for(int j=0 , colNo = 0;j < col.Length;j++){

				if(isCommaStr || col[j].Contains("\""))
				{
					col[j].Trim();
					if(col[j].StartsWith("\"")){
						isCommaStr = true;
						str1 = "";
						col[j] = col[j].Remove(0,1);
					}else if(col[j].Contains("\"")){
						isCommaStr = false;	
						col[j] = col[j].Remove(col[j].Length-2 ,1);
					}
					//append all strings
					str1 += col[j];
				}else
					str1 = col[j];

//				print("Row: "+i+" "+j+" "+col.Length+" "+col[colNo]);
				if(!isCommaStr){
					storyData[i,colNo] = str1;
					colNo++;
//					print("Row: "+i+" "+str1+" "+isCommaStr );
				}
 			}
		}
		isFileLoaded = true;		
		print("CSV read done ");
	}

	public int startIndex;
	public int getLengthOfChapter(string chap_Name){
		int len = 0;
		startIndex = -1;
		for(int i=0;i<RowCount;i++){
			if(storyData[i,0].ToLower().Equals(chap_Name.ToLower())){
				len++;
				if(startIndex == -1){
					startIndex = i;
				}
			}
		}
		return len;
	}

}


//using UnityEngine;
//using System.Collections;
//using System.IO;
//using System.Collections.Generic;
//
//public class CsvManager : MonoBehaviour {
//	
//	List<string> gameInfo;
//	// Use this for initialization
//	void Start () {
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//	//filedname
//	public string[] getDataOfGame(){
//		
//		gameInfo = new List<string>();
//		//add card field names
//		CardFieldNamesTable fnms = MyDatabaseManager.instance.getCardsFieldNamesFromTable ();
//		
//		//get all progress wall cards data
//		for(int i=0;i<ProgressWall.instance.progressContainers.Length;i++){
//			//add heading
//			gameInfo.Add("\n Progress wall "+(i+1)+" Cards");
//			string cardHead = "";
//			//fileldnames: ce_NameF={0}, ce_SalF={1}, ce_InfoF={2}, ce_AgeF={3}, ce_ExpF={4}, ce_GenderF={5}, ce_Industry_ExpF={6}, ce_Technical_SkillF={7}, ce_Sales_SkillF={8}, ce_Customer_FocusF={9}, ce_Team_PlayerF={10}, ce_Cult_DexF={11}, ce_CommF={12}]", ce_NameF, ce_SalF, ce_InfoF, ce_AgeF, ce_ExpF, ce_GenderF, ce_Industry_ExpF, ce_Technical_SkillF, ce_Sales_SkillF, ce_Customer_FocusF, ce_Team_PlayerF, ce_Cult_DexF, ce_CommF);
//			//[CardTable: cardId={0}, ce_Name={1}, ce_Sal={2}, ce_Info={3}, ce_UserPic={4}, ce_Age={5}, ce_Exp={6}, ce_Gender={7}, ce_Industry_Exp={8}, ce_Technical_Skill={9}, ce_Sales_Skill={10}, ce_Customer_Focus={11}, ce_Team_Player={12}, ce_Cultural_Dexterity={13}, ce_Communication={14}, isCardSelected={15}]", cardId, ce_Name, ce_Sal, ce_Info, ce_UserPic, ce_Age, ce_Exp, ce_Gender, ce_Industry_Exp, ce_Technical_Skill, ce_Sales_Skill, ce_Customer_Focus, ce_Team_Player, ce_Cultural_Dexterity, ce_Communication, isCardSelected);
//			
//			//			cardHead += "id , "+fnms.ce_NameF+","+fnms.ce_SalF+","+fnms.ce_InfoF+","+fnms.ce_AgeF;
//			//			cardHead += "id , "+fnms.ce_NameF+","+fnms.ce_SalF+","+fnms.ce_InfoF+","+fnms.ce_AgeF;
//			//
//			//			gameInfo.Add(cardHead);
//			gameInfo.Add("id , " + fnms.ToString());
//			
//			
//			//add cards
//			int childs = ProgressWall.instance.progressContainers[i].transform.childCount;
//			if(childs>0){
//				for(int j=0;j<childs;j++){
//					Card cd = ProgressWall.instance.progressContainers[i].transform.GetChild(j).GetComponent<Card>();
//					string cData = "";
//					if(!cd.isBlankCardProp){
//						cData = cd.thisCard.ToString();
//					}else{
//						cData = cd.getBlankCardData().ToString();
//					}
//					print("adding card data "+cData);
//					gameInfo.Add(cData);
//				}
//			}
//		}	
//		
//		//get all notes
//		
//		gameInfo.Add("\npostit Notes");
//		postItNoteData[] pData = GameHandler.instance.getPostitnotesInfo ();
//		for(int i=0;i<pData.Length;i++){
//			gameInfo.Add("\""+pData[i].noteData+"\"");
//		}
//		
//		//Notes
//		
//		gameInfo.Add("\nNotes");
//		for(int i=0;i<6;i++){
//			gameInfo.Add( "Progress Level "+(i+1)+","+"\""+GameHandler.instance.GameNotes[i]+"\"");
//		}
//		//		SaveGamedataToCsv (gameInfo.ToArray() , Application.persistentDataPath + "/csvTest1.csv" );
//		
//		return gameInfo.ToArray ();
//	}
//	
//	void SaveGamedataToCsv(string[] data , string fileName){
//		
//		//create a file
//		File.WriteAllLines (fileName,data);
//		//		print ("data exported successfully ");
//		//		PopUpsManager.instance.showSingleButtonPopUp ("Data exported successfully");
//	}
//	
//	public void exportCsvToLocation(string fileLocation){
//		try{
//			SaveGamedataToCsv (getDataOfGame() , fileLocation + ".csv");
//			PopUpsManager.instance.showSingleButtonPopUp ("Data exported successfully");
//		}catch(System.Exception e){			
//			PopUpsManager.instance.showSingleButtonPopUp ("Failed exporting data");
//		}
//	}
//}
