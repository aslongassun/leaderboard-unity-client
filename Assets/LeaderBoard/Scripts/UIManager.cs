using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	// Canvas longin window
	public GameObject loginWindow;
	// Canvas register user window
	public GameObject registerWindow;
	// Canvas user score board window
	public GameObject userWindow;
	// Canvas admin score board window
	public GameObject adminWindow;
	// Canvas loading view
	public GameObject loadingView;
	// Button login
	public Button login;
	// Button register
	public Button register;
	// Input username login
	public InputField loginUsername;
	// Input password login
	public InputField loginPassword;
	// Input username for create user
	public InputField createUsername;
	// Input password for create user
	public InputField createPassword;
	// User ScoreEntry of scroll view
	public GameObject scoreEntry;
	// User Scroll view content
	public GameObject scrollContent;
	// Admin ScoreEntry of scroll view
	public GameObject adminScoreEntry;
	// Admin Scroll view content
	public GameObject adminScrollContent;
	// List users show on score board
	public User[] userBoardArray;
	// List users show on score board
	public User[] adminBoardArray;
	// Current login user
	public User loginUser;
	// Update field username
	public InputField usernameUpdateField;
	// Update field score
	public InputField scoreUpdateField;
	// Message on admin score board
	public Text adminBoardMessage;
	// Message on login
	public Text loginMessage;
	// Login message wrapper
	public GameObject loginMessageWrapper;
	// Dropdow gameobject
	public Dropdown minutesDropdown;

	/// <summary>
	/// Trigger event onchange dropdown
	/// </summary>
	public void onChangeMinutesDropdown(int value){
		RequestManager.GetInstance ().CallGetUsersForAdmin();
		return;
	}

	/// <summary>
	/// Update admin score board
	/// </summary>
	public void updateAdminBoard(){

		while(adminScrollContent.transform.childCount > 0) {
			Transform c = adminScrollContent.transform.GetChild(0);
			c.SetParent(null);
			Destroy (c.gameObject);
		}

		foreach(User info in adminBoardArray){
			GameObject newScoreEntry = Instantiate(adminScoreEntry);
			newScoreEntry.transform.Find ("Username").GetComponent<Text> ().text = info.name;
			newScoreEntry.transform.Find ("Timesupdate").GetComponent<Text> ().text = info.updatecounter;
			newScoreEntry.transform.Find ("Lastupdate").GetComponent<Text> ().text = info.updatedAt;
			newScoreEntry.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener(delegate{actionRemoveUser(info._id);});
			newScoreEntry.transform.parent = adminScrollContent.transform;
		}

		adminBoardMessage.text = "result: " + adminBoardArray.Length + " users";

	}


	/// <summary>
	/// Update user score board
	/// </summary>
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

	/// <summary>
	/// Call action to remove user by user id
	/// </summary>
	public void actionRemoveUser(string userId){
		RequestManager.GetInstance ().CallRemoveUser(userId);
	}

	/// <summary>
	/// Call action to login with username and password
	/// </summary>
	public void actionLogin(){
		RequestManager.GetInstance ().CallLogin (loginUsername.text,loginPassword.text);
	}

	/// <summary>
	/// Move to register user window
	/// </summary>
	public void actionRegister(){
		loginWindow.SetActive (false);
		registerWindow.SetActive (true);
	}

	/// <summary>
	/// Call action to register new user with username and password
	/// </summary>
	public void actionCreateUser(){
		registerWindow.SetActive (false);
		loginWindow.SetActive (true);
		RequestManager.GetInstance ().CallCreateUser (createUsername.text,createPassword.text);
	}

	/// <summary>
	/// Call action to update user info
	/// </summary>
	public void updateScore(){
		loginUser.name = usernameUpdateField.text;
		loginUser.score = scoreUpdateField.text;
		RequestManager.GetInstance ().CallUpdateUser (loginUser._id);
	}

	/// <summary>
	/// Move to login window
	/// </summary>
	public void gotoLoginWindow(){
		userWindow.SetActive (false);
		registerWindow.SetActive (false);
		adminWindow.SetActive (false);
		loginWindow.SetActive (true);
		loginMessage.text = "";
		loginMessageWrapper.SetActive (false);
		CancelInvoke ();
	}
}
