using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TheUI : MonoBehaviour {

	static TheUI instance;
	static GameObject parentObject;
	public static TheUI GetInstance()
	{
		if( !instance )
		{
			parentObject = GameObject.Find("TheUI");
			if (parentObject != null)
				instance  = parentObject.GetComponent<TheUI>();
			else
				Debug.Log("WTF : no instance found for TheUI");
		}
		return instance;
	}


	public Image darkBackground;

	public void FadeInDarkDelayed(float delay){
		Invoke("FadeInDark", delay);
	}
	void FadeInDark(){
		FadeDark(1);
	}

	public void FadeDark(float alpha){
		darkBackground.CrossFadeAlpha(alpha, 1f, true);
	}
}
