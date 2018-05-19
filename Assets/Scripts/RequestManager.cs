using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour {
	private static float TIME_UPDATE_LEADERBOARD = 2f;
	//reference to this script instance
	private static RequestManager instance;

	//private static string _Url = "http://localhost";
	private static string _Url = "https://vinhua-nodejs.herokuapp.com";
	//private static string _Port = "3000";

	public UIManager ui;
	public enum MinutesTime
	{
		ALL,
		ONE,
		FIVE,
		TEN
	}

	void Awake(){
		instance = this;
	}

	/// <summary>
	/// Returns a reference to this script instance.
	/// </summary>
	public static RequestManager GetInstance()
	{
		return instance;
	}

	public void CallCreateUser(string name,string password)
	{
		StartCoroutine(SubmitCreateUser(name,password));
	}

	public void CallGetUsers()
	{
		StartCoroutine(SubmitGetUsers());
	}

	public void CallUpdateUser(string userId){
		StartCoroutine(SubmitUpdateUsers(userId));
	}

	public void CallLogin(string name, string password){
		StartCoroutine(SubmitLogin(name,password));
	}

	public void CallRemoveUser(string userId){
		StartCoroutine(SubmitRemoveUser(userId));
	}

	private IEnumerator SubmitRemoveUser(string userId)
	{
		Debug.Log("Submitting update=");

		Debug.Log("Id update=" + userId);
		UnityWebRequest www = UnityWebRequest.Delete(_Url + "/users/" + userId);
		yield return www.Send();

		if (www.isNetworkError)
		{
			// There was an error
			Debug.Log(www.error);
		}
		else
		{
			if (www.responseCode == 200)
			{
				// Response code 200 signifies that the server had no issues with the data we went
				Debug.Log("Form sent complete!");
				//CallGetUsers ();
			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	private IEnumerator SubmitUpdateUsers(string userId)
	{
		Debug.Log("Submitting update=");

		Debug.Log("Id update=" + ui.loginUser._id);
		Debug.Log("Name update=" + ui.loginUser.name);
		Debug.Log("Score update=" + ui.loginUser.score);

		// Create a PUT web request with our form data
		//byte[] myData = System.Text.Encoding.UTF8.GetBytes("{\"name\":\"user15\"}");
		ui.loginUser.updatecounter = (int.Parse(ui.loginUser.updatecounter) + 1).ToString();
		//byte[] myData = System.Text.Encoding.UTF8.GetBytes("{ name: '15' }");
		byte[] myData = System.Text.Encoding.UTF8.GetBytes("name=" + ui.loginUser.name + "&score=" + ui.loginUser.score+ "&updatecounter=" + ui.loginUser.updatecounter);
		UnityWebRequest www = UnityWebRequest.Put(_Url + "/users/" + ui.loginUser._id, myData);
		www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		// Send the request and yield until the send completes
		yield return www.Send();

		if (www.isNetworkError)
		{
			// There was an error
			Debug.Log(www.error);
		}
		else
		{
			if (www.responseCode == 200)
			{
				// Response code 200 signifies that the server had no issues with the data we went
				Debug.Log("Form sent complete!");
				Debug.Log("Response:" + www.downloadHandler.text);
				//CallGetUsers ();
			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	private IEnumerator SubmitLogin(string name, string password){

		// Create a form that will contain our data
		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("password", password);

		// Create a POST web request with our form data
		// UnityWebRequest www = UnityWebRequest.Post(_Url + ":" + _Port + "/users/login", form);
		UnityWebRequest www = UnityWebRequest.Post(_Url + "/users/login", form);
		// Send the request and yield until the send completes
		yield return www.Send();

		if (www.isNetworkError)
		{
			// There was an error
			Debug.Log(www.error);
		}
		else
		{
			if (www.responseCode == 200)
			{
				// Response code 200 signifies that the server had no issues with the data we went
				Debug.Log("Login success!");

				Debug.Log("Response:" + www.downloadHandler.text);
				string jsonFromServer = JsonHelper.fixJsonFromServer(www.downloadHandler.text);
				ui.loginUser = JsonHelper.FromJson<User>(jsonFromServer)[0];

				ui.loginWindow.SetActive (false);
				// TODO: Check role
				if (ui.loginUser.role == "admin") {
					ui.adminWindow.SetActive (true);
				} else {
					ui.userWindow.SetActive (true);
				}

				ui.usernameUpdateField.text = ui.loginUser.name;
				ui.scoreUpdateField.text = ui.loginUser.score;
				InvokeRepeating("CallGetUsers", 0f, TIME_UPDATE_LEADERBOARD);

			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	private IEnumerator SubmitCreateUser(string name,string password)
	{
		Debug.Log("Submitting score");

		// Create a form that will contain our data
		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("password", password);

		// Create a POST web request with our form data
		UnityWebRequest www = UnityWebRequest.Post(_Url + "/users", form);
		// Send the request and yield until the send completes
		yield return www.Send();

		if (www.isNetworkError)
		{
			// There was an error
			Debug.Log(www.error);
		}
		else
		{
			if (www.responseCode == 200)
			{
				// Response code 200 signifies that the server had no issues with the data we went
				Debug.Log("Form sent complete!");
				Debug.Log("Response:" + www.downloadHandler.text);
			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	private IEnumerator SubmitGetUsers(){
		
		string urlGet = _Url + "/users";
		if (ui.loginUser.role == "admin") {
			urlGet = urlGet + "/admin";
			switch ((MinutesTime)ui.minutesDropdown.value) {
				case MinutesTime.ONE:
					urlGet += "/" + 1;
					break;
				case MinutesTime.FIVE:
					urlGet += "/" + 5;
					break;
				case MinutesTime.TEN:
					urlGet += "/" + 10;
					break;
			}
		}

		UnityWebRequest www = UnityWebRequest.Get (urlGet);
		// Send the request and yield until the send completes
		yield return www.Send();

		if (www.isNetworkError)
		{
			// There was an error
			Debug.Log(www.error);
		}
		else
		{
			if (www.responseCode == 200)
			{
				// Response code 200 signifies that the server had no issues with the data we went
				Debug.Log("Form sent complete!");
				Debug.Log("Response:" + www.downloadHandler.text);
				string jsonFromServer = JsonHelper.fixJsonFromServer(www.downloadHandler.text);
				ui.userBoardArray = JsonHelper.FromJson<User>(jsonFromServer);
				if (ui.loginUser.role == "admin") {
					ui.updateAdminBoard ();
				} else {
					ui.updateUserBoard ();
				}
			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}
}
