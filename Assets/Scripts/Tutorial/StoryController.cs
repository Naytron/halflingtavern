
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Page{
	public string pageText;
	public string animClipName;
	public string sceneName;
}
public enum Scene_Names{
	Intro = 1,
	End_Day_1 = 2,
	End_Day_2 = 3,
	End_Day_3 = 4,
	End_Day_4 = 5,
	End_Day_5 = 6,
	End_Day_6 = 7,
	End_Day_7 = 8,
}
public class StoryController : MonoBehaviour {

	public static StoryController instance;

	public Image storyImg;
	public Text storyText;
	public Animator anim;

	[SerializeField] GameObject[] allStoryScenePrefabs;
	GameObject currentScenePrefab;

	private Page[] allPages;
	int currentPage;


	// Use this for initialization
	void Awake () {
		instance = this;
	}

	void OnEnable(){
		setStoryInitials ();
	}

	public void setStoryInitials(){
		//read data from csv file
		if(!CSV_Manager.ins.isFileLoaded)
			CSV_Manager.ins.readFile();
		//create pages of story or load story 
		createStory();
		currentPage = 0;
		setPage (currentPage);
	}

	void setPage(int index){
		if(allPages.Length>0 && index<allPages.Length){
			storyText.text = allPages[index].pageText;
			//create prefab of scene and assign animator
			if(currentScenePrefab != null){
				Destroy (currentScenePrefab.gameObject);
				currentScenePrefab = null;
			}
			currentScenePrefab = Instantiate(getCurrentScenePrefabIndex(allPages[index].sceneName)) as GameObject;
			currentScenePrefab.transform.SetParent(this.transform);
			currentScenePrefab.transform.localPosition = Vector3.zero;
			currentScenePrefab.transform.localScale = Vector3.one;
			currentScenePrefab.transform.SetAsFirstSibling();
			anim = currentScenePrefab.GetComponent<Animator>();
			anim.Play(allPages[index].animClipName);
		}
	}

	GameObject getCurrentScenePrefabIndex(string scName){
		for(int i=0;i<allStoryScenePrefabs.Length;i++){
			if(allStoryScenePrefabs[i].gameObject.name == scName)
				return allStoryScenePrefabs[i];
		}
		print("no scene with name found "+scName);
		return null;
	}

	public void nextButtonClicked(){
		if (currentPage < allPages.Length - 1) {
			currentPage++;
			setPage (currentPage);	
		} else {
			if(GameHandler.instance.currentDay == 1){
				GameHandler.instance.isPlayingTutorial = true;
				//start tutorial
				PopUpsManager.instance.openScreen(MyScreens.Tutorial);
				TutorialController.ins.initTutorial(1);
			}else if(GameHandler.instance.currentDay == 3 && PlayerPrefs.GetInt (MyPrefereces.KEY_IS_DAY3_TUT_DONE) == 0){
				GameHandler.instance.isPlayingTutorial = true;
				//start tutorial
				PopUpsManager.instance.openScreen(MyScreens.Tutorial);
				TutorialController.ins.initTutorial(3);
			}
			else
				GameController.instance.loadDay();
		} 
	}

	public string curr_Chap_Name;
	void createStory(){

		curr_Chap_Name = ((Scene_Names)GameHandler.instance.currentDay).ToString();
		int PageCount = CSV_Manager.ins.getLengthOfChapter(curr_Chap_Name);
		int startIndex =  CSV_Manager.ins.startIndex;
		//get data for chapter from csv storydata and create pages
		allPages = new Page[PageCount];
		for(int i = 0;i<allPages.Length;i++){
			//initialize page variables
			allPages[i] = new Page();
			//store data in pages
			allPages[i].sceneName = CSV_Manager.ins.storyData[startIndex + i , 1];
			allPages[i].animClipName =  CSV_Manager.ins.storyData[startIndex + i , 2];
			allPages[i].pageText =  CSV_Manager.ins.storyData[startIndex + i , 3];// column =3 contain dialogue
		}

//		for(int i=0;i<CSV_Manager.ins.storyData.Length;i++){
//			if(CSV_Manager.ins.storyData[i,0].ToLower().Equals(chap_Name.ToLower())){
//				
//			}
//		}

//		switch(GameHandler.instance.currentDay){
//		case 1:
//			allPages = new Page[5];
//			for(int i=0;i<allPages.Length;i++)
//				allPages[i] = new Page();
//			//Intro scene 1
////			allPages[0].pageText = "Halfling - Ahh, good Ale and good company, I could run a tavern like this. - " +
////				"\" … One more pint, and then I’ll be getting’ home before the misses tans me backside with her kettle spoon\"";
////			allPages[0].animClipName = "Intro_Scene1";
//			break;
//		}
	
	}


}
