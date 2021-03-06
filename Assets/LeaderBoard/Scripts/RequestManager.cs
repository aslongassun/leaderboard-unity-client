﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using SocketIO;

public class RequestManager : MonoBehaviour {
	
	// Local host
	private static string _Url = "http://localhost";
	private static string _Port = ":" + "3000";

	//Heroku with connect to MongoDB database
	//private static string _Url = "https://vinhua-nodejs.herokuapp.com";
	//private static string _Port = "";

	// Note: Also change socket url in SocketIO prefab to:
	// Tws://vinhua-nodejs.herokuapp.com/socket.io/?EIO=4&transport=websocket

	// Heroku with Redis
	//private static string _Url = "https://vinhhua-nodejs-redis.herokuapp.com";
	//private static string _Port = "";

	// Note: Also change socket url in SocketIO prefab to:
	// ws://vinhhua-nodejs-redis.herokuapp.com/socket.io/?EIO=4&transport=websocket

	// Canvas ui
	public UIManager ui;
	// Dropdow values
	public enum MinutesTime {ALL,ONE,FIVE,TEN};
	// Instance of RequestManager 
	private static RequestManager instance;

	private SocketIOComponent socket;

	void Awake(){
		instance = this;
	}
	public void Start() 
	{
		// Open socket
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On("open", socketOnOpen);
		socket.On("receiveusers",SocketOnReceiveUsers);
		socket.On("error", socketOnError);
		socket.On("close", socketOnClose);
	}

	/// <summary>
	/// Returns a reference to this script instance.
	/// </summary>
	public static RequestManager GetInstance()
	{
		return instance;
	}

	/// <summary>
	/// Simple create a new user with username and passwork
	/// </summary>
	public void CallCreateUser(string name,string password)
	{
		StartCoroutine(SubmitCreateUser(name,password));
	}

	/// <summary>
	/// Update user by user id
	/// </summary>
	public void CallUpdateUser(string userId){
		StartCoroutine(SubmitUpdateUsers(userId));
	}

	/// <summary>
	/// Update user by user id
	/// </summary>
	public void CallLogin(string name, string password){
		StartCoroutine(SubmitLogin(name,password));
	}

	/// <summary>
	/// Admin remove user from leaderboard
	/// </summary>
	public void CallRemoveUser(string userId){
		StartCoroutine(SubmitRemoveUser(userId));
	}

	/// <summary>
	/// Call get user for admin
	/// </summary>
	public void CallGetUsersForAdmin(){
		StartCoroutine(SubmitGetUsersForAdmin());
	}

	/// <summary>
	/// Send request to get list users for admin from database
	/// </summary>
	private IEnumerator SubmitGetUsersForAdmin(){
		
		string urlGet = _Url + _Port + "/users/admin";

		// Prepare time parameter
		switch ((MinutesTime)ui.minutesDropdown.value) {
			case MinutesTime.ALL:
				urlGet += "/" + 0;
				break;
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
				string jsonFromServer = JsonHelper.fixJsonFromServer(www.downloadHandler.text);
				ui.adminBoardArray = JsonHelper.FromJson<User>(jsonFromServer);
				ui.updateAdminBoard ();
			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	/// <summary>
	/// Send request delete to remove user from leaderboard
	/// </summary>
	private IEnumerator SubmitRemoveUser(string userId)
	{
		
		UnityWebRequest www = UnityWebRequest.Delete(_Url + _Port + "/users/" + userId);

		float startTime = Time.time;

		ui.loadingView.SetActive (true);
		yield return www.Send();
		ui.loadingView.SetActive (false);

		if (www.isNetworkError)
		{
			// There was an error
			Debug.Log(www.error);
		}
		else
		{
			if (www.responseCode == 200)
			{
				getSpentTimesForRequest (startTime, "Delete User");
				// Response code 200 signifies that the server had no issues with the data we went
				CallGetUsersForAdmin();
			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	/// <summary>
	/// Send request put to update user from leaderboard
	/// </summary>
	private IEnumerator SubmitUpdateUsers(string userId)
	{

		ui.loginUser.updatecounter = (int.Parse(ui.loginUser.updatecounter) + 1).ToString();
		byte[] myData = System.Text.Encoding.UTF8.GetBytes("name=" + ui.loginUser.name + "&score=" + ui.loginUser.score+ "&updatecounter=" + ui.loginUser.updatecounter);
		UnityWebRequest www = UnityWebRequest.Put(_Url + _Port + "/users/" + ui.loginUser._id, myData);
		www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		float startTime = Time.time;

		ui.loadingView.SetActive (true);
		yield return www.Send();
		ui.loadingView.SetActive (false);

		if (www.isNetworkError)
		{
			// There was an error
			Debug.Log(www.error);
		}
		else
		{
			if (www.responseCode == 200)
			{
				getSpentTimesForRequest (startTime, "Update User");
			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	/// <summary>
	/// Send request login
	/// </summary>
	private IEnumerator SubmitLogin(string name, string password){

		// Create a form that will contain our data
		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("password", password);

		// Create a POST web request with our form data
		UnityWebRequest www = UnityWebRequest.Post(_Url  + _Port + "/users/login", form);

		// Send the request and yield until the send completes
		float startTime = Time.time;

		ui.loadingView.SetActive (true);
		yield return www.Send();
		ui.loadingView.SetActive (false);

		if (www.isNetworkError)
		{
			// There was an error
			Debug.Log(www.error);
		}
		else
		{
			if (www.responseCode == 200)
			{
				getSpentTimesForRequest (startTime, "Login");
				
				string jsonFromServer = JsonHelper.fixJsonFromServer(www.downloadHandler.text);
				ui.loginUser = JsonHelper.FromJson<User>(jsonFromServer)[0];
				ui.loginWindow.SetActive (false);
				if (ui.loginUser.role == "admin") {
					ui.adminWindow.SetActive (true);
					CallGetUsersForAdmin();
				} else {
					ui.userWindow.SetActive (true);
					// Auto update from througth Socket
				}

				ui.usernameUpdateField.text = ui.loginUser.name;
				ui.scoreUpdateField.text = ui.loginUser.score;

				//InvokeRepeating("CallGetUsers", 0f, TIME_UPDATE_LEADERBOARD);
				//CallGetUsersForAdmin();
				//InvokeRepeating("GetUsersBySocket", 0f, TIME_UPDATE_LEADERBOARD);
				//GetUsersBySocket();

				ui.loginMessage.text = "";
				ui.loginMessageWrapper.SetActive (false);

			}
			else
			{
				
				ui.loginMessage.text = "Username or Password are incorrect!";
				ui.loginMessageWrapper.SetActive (true);

				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	/// <summary>
	/// Send request to create user
	/// </summary>
	private IEnumerator SubmitCreateUser(string name,string password)
	{
		
		// Create a form that will contain our data
		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("password", password);

		// Create a POST web request with our form data
		UnityWebRequest www = UnityWebRequest.Post(_Url + _Port + "/users", form);
		// Send the request and yield until the send completes

		float startTime = Time.time;

		ui.loadingView.SetActive (true);
		yield return www.Send();
		ui.loadingView.SetActive (false);

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
				getSpentTimesForRequest (startTime,"Create User");
			}
			else
			{
				// Any other response signifies that there was an issue with the data we sent
				Debug.Log("Error response code:" + www.responseCode.ToString());
			}
		}
	}

	/// <summary>
	/// Receive data from SocketIO
	/// </summary>
	public void SocketOnReceiveUsers(SocketIOEvent e){
		ObjectFromSocket objectSocket = JsonUtility.FromJson<ObjectFromSocket>(e.data.ToString());
		string jsonFromServer = JsonHelper.fixJsonFromServer(objectSocket.items);
		ui.userBoardArray = JsonHelper.FromJson<User>(jsonFromServer);
		if (ui.loginUser.role == "admin") {
			ui.updateAdminBoard ();
		} else {
			ui.updateUserBoard ();
		}
	}

	/// <summary>
	/// Calculate time from send request to get respone by second
	/// </summary>
	private void getSpentTimesForRequest(float startTime, string methodName){
		float dentalTime = Time.time - startTime;
		Debug.Log ("Spent Times " + methodName + ": " + dentalTime + "s");
	}

	/// <summary>
	/// Call when Socket open
	/// </summary>
	public void socketOnOpen(SocketIOEvent e)
	{
		Debug.Log("SocketIO open received: " + e.name + " " + e.data);
	}

	/// <summary>
	/// Call when Socket error
	/// </summary>
	public void socketOnError(SocketIOEvent e)
	{
		Debug.Log("SocketIO error received: " + e.name + " " + e.data);
	}

	/// <summary>
	/// Call when Socket Close
	/// </summary>
	public void socketOnClose(SocketIOEvent e)
	{	
		Debug.Log("SocketIO close received: " + e.name + " " + e.data);
	}
}
