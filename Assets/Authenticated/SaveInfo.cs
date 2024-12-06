using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SaveInfo : MonoBehaviour
{

	[Header("Here's my Global Info")] 
	public string SaveInfoPhpURL = "http://zitosanski.com/LOGINSYSTEMPHP/SaveInfo.php"; // don't forget to add the URL for the php script here.
	public string SecretKey = "123456789";
	public bool UpdateIP = true;

	[Header("Here's the Info for the player")]
	public string PlayerName = "";
	public int Score = 0;

	public string IP = String.Empty;
	public string DbIP = String.Empty;

	/*public static bool ifIsAdmin;
	public static bool ifIsModerator;*/
	
	public int Status = 0;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
		LoginManager.OnLogin += this.GetInfo;
	}

	private void OnDisable()
	{
		LoginManager.OnLogin -= this.GetInfo;
	}

	void GetInfo(string n, string e, int s, int st, string ip)
	{
		PlayerName = n;
		Score = s;
		Status = st;
		DbIP = ip;
		
		if (UpdateIP)
		{
			CheckIP();
		}
	}

	public void CheckIP()
	{
		if (DbIP != IP)
		{
			Debug.Log("The Database IP" + DbIP + " is not same as the local ip, which is:" + IP + ". They will be updated automatically");
			StartCoroutine(SendUpdateIP());
		}
		else
		{
			Debug.Log("they are the same ip address");
		}
	}

	IEnumerator SendUpdateIP()
	{
		string hashString = HashingData.Md5Sum(PlayerName + SecretKey).ToLower();
		
		WWWForm wwwForm = new WWWForm();
		
		wwwForm.AddField("name", PlayerName);
		wwwForm.AddField("IP", IP);
		wwwForm.AddField("score", Score);
		wwwForm.AddField("type", "2"); // This will be to check on the server if we are sending IP correctly
		wwwForm.AddField("hash", hashString);
		
		WWW www = new WWW(SaveInfoPhpURL, wwwForm);

		yield return www;

		if (www.error != null)
		{
			Debug.LogWarning(www.error);
		}
		else
		{
			Debug.Log(www.text == "successipdb" ? string.Format("Save the new IP {0} success!", IP) : www.text);
		}
	}
	
	public void SaveUserInfo(string s)
	{
		int score = int.Parse(s);
		SaveUserInfo(s);
	}
	
	public void SaveUserInfo(int s)
	{
		if (SaveInfoPhpURL != String.Empty)
		{
			StartCoroutine(Save(s));
		}
	}
	// end of comments
	
	IEnumerator Save(int s)
	{
		string hash = HashingData.Md5Sum(PlayerName + SecretKey).ToLower();

		int score = s;
		
		WWWForm Form = new WWWForm();
		
		Form.AddField("name", PlayerName);
		Form.AddField("score", Score);
		Form.AddField("hash", hash);
		Form.AddField("type", "1"); //  This will be to check on the server if we are saving the info for the player
		
		WWW www = new WWW(SaveInfoPhpURL, Form);
		yield return www;

		if (www.error == null)
		{
			Score = score;
		}
		Debug.Log("info saved, great!!");
	}

}
