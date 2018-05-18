using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public GameObject loginWindow;
	public GameObject registerWindow;
	public GameObject userWindow;
	public GameObject adminWindow;


	public Button login;
	public Button register;
	public Text username;
	public Text password;

	public GameObject scoreEntry;
	public GameObject scrollContent;

	public GameObject adminScoreEntry;
	public GameObject adminScrollContent;

	private ArrayList scoreInfos;
	private ArrayList adminScoreInfos;

	void Start(){
		
		scoreInfos = new ArrayList (){
			new ScoreInfo("User1","1000"),
			new ScoreInfo("User1","1100"),
			new ScoreInfo("User1","1200"),
			new ScoreInfo("User1","1000"),
			new ScoreInfo("User1","1100"),
			new ScoreInfo("User1","1200"),
			new ScoreInfo("User1","1000"),
			new ScoreInfo("User1","1100"),
			new ScoreInfo("User1","1200"),
			new ScoreInfo("User1","1100"),
			new ScoreInfo("User1","1200"),
			new ScoreInfo("User1","1000"),
			new ScoreInfo("User1","1100"),
			new ScoreInfo("User1","1200")
		};

		int countRank = 1;
		foreach(ScoreInfo info in scoreInfos){
			GameObject newScoreEntry = Instantiate(scoreEntry);
			newScoreEntry.transform.Find ("Rank").GetComponent<Text> ().text = countRank.ToString();
			newScoreEntry.transform.Find ("Username").GetComponent<Text> ().text = info.username;
			newScoreEntry.transform.Find ("Score").GetComponent<Text> ().text = info.score;
			newScoreEntry.transform.parent = scrollContent.transform;
			countRank++;
		}

		adminScoreInfos = new ArrayList (){
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1000","2","2018/01/01 10:10:10"),
			new ScoreInfo("User1","1200","3","2018/01/01 10:10:10")
		};

		foreach(ScoreInfo info in adminScoreInfos){
			GameObject newScoreEntry = Instantiate(adminScoreEntry);
			newScoreEntry.transform.Find ("Username").GetComponent<Text> ().text = info.username;
			newScoreEntry.transform.Find ("Timesupdate").GetComponent<Text> ().text = info.timesupdate;
			newScoreEntry.transform.Find ("Lastupdate").GetComponent<Text> ().text = info.lastupdate;
			newScoreEntry.transform.parent = adminScrollContent.transform;
		}
	}

	public void actionLogin(){
		Debug.Log ("Login action!");
		loginWindow.SetActive (false);
		if (username.text == "admin" && password.text == "admin") {
			adminWindow.SetActive (true);
		} else {
			userWindow.SetActive (true);
		}

	}

	public void actionRegister(){
		Debug.Log ("Register action!");
		loginWindow.SetActive (false);
		registerWindow.SetActive (true);
	}

	public void actionCreateUser(){
		Debug.Log ("Create user action!");
		registerWindow.SetActive (false);
		userWindow.SetActive (true);
	}

	public void updateScore(){
		Debug.Log ("Update user score!");
	}

	public void gotoLoginWindow(){
		userWindow.SetActive (false);
		registerWindow.SetActive (false);
		adminWindow.SetActive (false);
		loginWindow.SetActive (true);
	}
}
