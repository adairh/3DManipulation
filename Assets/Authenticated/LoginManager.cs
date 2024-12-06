using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class LoginManager : MonoBehaviour
{

	public delegate void LoginEvent(string name, string email, int score, int status, string ip);

	public static LoginEvent OnLogin;

	public static string IP = "None";

	[Header("Refrences")] 
	public GameObject PlayerInfo;
	public Animation LoginAnimation;
	public Animation RegisterAnimation;

	[Header("Ip Settings")] 
	public bool GetIpOnAwake = true;

	[Header("UI")] 
	public Text Description = null;
	private static Text _Descrip = null;
	public Image BlackScreen;

	[Header("Scripts and their Refrences")]
	public LoadingEffect Loading = null;
	public static LoadingEffect LoadingCache = null;

	public const string SavedUser = "UserName";
	private Color alpha = new Color(0, 0, 0, 0);
	
	// Private values

	private bool InLogin = true;
	private bool ShowInfo = false;
	private SaveInfo saveInfo = null;

	private void Awake()
	{
		IP = string.Empty;
		if (Loading != null)
		{
			LoadingCache = Loading;
		}

		if (GetIpOnAwake)
		{
			StartCoroutine(GetIP()); // Do my coroutine here and don't forget.
		}

		_Descrip = Description;
		OnLogin += onLogin; //resolve it later
		StartCoroutine(FadeOut()); // Do my coroutine here for fading the GameObj. in the scene
		if (GameObject.Find(SavedUser) == null)
		{
			GameObject person = Instantiate(PlayerInfo, Vector3.zero, Quaternion.identity) as GameObject;
			person.name = person.name.Replace("(Clone)", "");
			saveInfo = person.GetComponent<SaveInfo>();
		}
		else
		{
			saveInfo = GameObject.Find("PlayerInfo").GetComponent<SaveInfo>();
		}
		
	}

	void OnDisable()
	{
		OnLogin -= onLogin;
	}

	public static void UpdateDescription(string descText)
	{
		_Descrip.text = descText;
	}
	
	public void ShowLogin()
	{
		if (!InLogin)
		{
			InLogin = true;
			LoginAnimation.Play("Login_Show");
			RegisterAnimation.Play("Register_Hide");
			UpdateDescription("");
		}
	}


	public void ShowRegister()
	{
		if (InLogin)
		{
			InLogin = false;
			LoginAnimation.Play("Login_Hide");
			RegisterAnimation.Play("Register_Show");
			UpdateDescription("");
		}
	}

	public static string nameOfPlayer = "";
	
	// Resolve our OnLoginEvent function
	public static void OnLoginEvent(string name, string email, int score, int status, string ip)
	{
		if (OnLogin != null)
			OnLogin(name, email, score, status, ip);

		nameOfPlayer = name;
	}

	void onLogin(string n, string e, int s, int st, string ip)
	{
		BlackScreen.gameObject.SetActive(true);
		
		// Write my IEnumerator for fading in.
		StartCoroutine(FadeIn());
	}

	IEnumerator GetIP()
	{
		Loading.ChangeText("Request public IP...", true);
		//Request public IP to the server
		WWW w = new WWW("http://zitosanski.com/LOGINSYSTEMPHP/GetIP.php");
		//Wait for response
		if (w == null) yield return null;
		yield return w;
		Loading.ChangeText("Getting public IP...", true, 2);
		//Get Ip
		string t;
		t = w.text;
		IP = t;
		if (saveInfo != null) { saveInfo.IP = IP; }
	}

	IEnumerator FadeOut()
	{
		alpha.a = 1f;

		while (alpha.a > 0.0f)
		{
			alpha.a -= Time.deltaTime;
			BlackScreen.color = alpha;
			yield return null;
		}
		
		BlackScreen.gameObject.SetActive(false);
	}

	IEnumerator FadeIn()
	{
		alpha = BlackScreen.color;

		while (alpha.a < 1.0f)
		{
			alpha.a += Time.deltaTime;
			BlackScreen.color = alpha;
			yield return null;
		}
			
	}
	
}
