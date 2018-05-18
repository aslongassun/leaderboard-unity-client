using UnityEngine;

public class ScoreInfo {

	public string username;
	public string score;
	public string timesupdate;
	public string lastupdate;

	public ScoreInfo(string username, string score){
		this.username = username;
		this.score = score;
	}

	public ScoreInfo(string username, string score, string timesupdate, string lastupdate){
		this.username = username;
		this.score = score;
		this.timesupdate = timesupdate;
		this.lastupdate = lastupdate;
	}
}
