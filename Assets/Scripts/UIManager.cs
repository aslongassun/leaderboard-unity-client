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

	public InputField loginUsername;
	public InputField loginPassword;
	public InputField createUsername;
	public InputField createPassword;

	public GameObject scoreEntry;
	public GameObject scrollContent;

	public GameObject adminScoreEntry;
	public GameObject adminScrollContent;

	//private ArrayList scoreInfos;
	//private ArrayList adminScoreInfos;

	public User[] userBoardArray;
	public User loginUser;
	public InputField usernameUpdateField;
	public InputField scoreUpdateField;

	public Text adminBoardMessage;

	public Dropdown minutesDropdown;



	public void onChangeMinutesDropdown(int value){
		Debug.Log ("value:" + value);
		RequestManager.GetInstance ().CallGetUsers ();
	}

	public void updateAdminBoard(){

		while(adminScrollContent.transform.childCount > 0) {
			Transform c = adminScrollContent.transform.GetChild(0);
			c.SetParent(null);
			Destroy (c.gameObject);
		}

		foreach(User info in userBoardArray){
			GameObject newScoreEntry = Instantiate(adminScoreEntry);
			newScoreEntry.transform.Find ("Username").GetComponent<Text> ().text = info.name;
			newScoreEntry.transform.Find ("Timesupdate").GetComponent<Text> ().text = info.updatecounter;
			newScoreEntry.transform.Find ("Lastupdate").GetComponent<Text> ().text = info.updatedAt;

			newScoreEntry.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener(delegate{actionRemoveUser(info._id);});

			newScoreEntry.transform.parent = adminScrollContent.transform;
		}

		adminBoardMessage.text = "Number of users:" + userBoardArray.Length;
	}



	public void updateUserBoard(){

		while(scrollContent.transform.childCount > 0) {
			Transform c = scrollContent.transform.GetChild(0);
			c.SetParent(null);
			Destroy (c.gameObject);
		}

		int countRank = 1;
		foreach(User info in userBoardArray){
			GameObject newScoreEntry = Instantiate(scoreEntry);
			newScoreEntry.transform.Find ("Rank").GetComponent<Text> ().text = countRank.ToString();
			newScoreEntry.transform.Find ("Username").GetComponent<Text> ().text = info.name;
			newScoreEntry.transform.Find ("Score").GetComponent<Text> ().text = info.score;
			newScoreEntry.transform.parent = scrollContent.transform;
			countRank++;
		}

	}

	public void actionRemoveUser(string userId){
		Debug.Log ("Remove action!");
		RequestManager.GetInstance ().CallRemoveUser(userId);
	}

	public void actionLogin(){
		Debug.Log ("Login action!");
		RequestManager.GetInstance ().CallLogin (loginUsername.text,loginPassword.text);
	}

	public void actionRegister(){
		Debug.Log ("Register action!");
		loginWindow.SetActive (false);
		registerWindow.SetActive (true);
	}

	public void actionCreateUser(){
		Debug.Log ("Create user action!");
		registerWindow.SetActive (false);
		loginWindow.SetActive (true);

		RequestManager.GetInstance ().CallCreateUser (createUsername.text,createPassword.text);

	}

	public void updateScore(){
		Debug.Log ("Update user score!");
		loginUser.name = usernameUpdateField.text;
		loginUser.score = scoreUpdateField.text;
		RequestManager.GetInstance ().CallUpdateUser (loginUser._id);
	}

	public void gotoLoginWindow(){
		userWindow.SetActive (false);
		registerWindow.SetActive (false);
		adminWindow.SetActive (false);
		loginWindow.SetActive (true);
		CancelInvoke ();
	}
}
