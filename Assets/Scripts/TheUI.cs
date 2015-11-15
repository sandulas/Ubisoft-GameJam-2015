using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

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

	public GameController gameController;

	public Image darkBackground;

	public CanvasGroup canvasHome;
	public CanvasGroup canvasLevels;


	public void FadeInDarkDelayed(float delay){
		Invoke("FadeInDark", delay);
	}
	void FadeInDark(){
		FadeDark(1);
	}

	public void FadeDark(float alpha){
		darkBackground.CrossFadeAlpha(alpha, 1f, true);
	}

	public void OnButtonPlay(){
//		canvasHome.gameObject.SetActive(false);
		canvasLevels.gameObject.SetActive(true);
		canvasLevels.alpha = 0;
		canvasLevels.DOFade(1, .4f).SetDelay(0.3f).SetEase(Ease.Linear);
		canvasHome.DOFade(0, .3f).OnComplete(()=>{canvasHome.gameObject.SetActive(false);});
	}

	public void OnButtonLevel(int levelNumber){
		canvasLevels.DOFade(0, .3f).OnComplete(()=>{canvasLevels.gameObject.SetActive(false);});
//		canvasLevels.gameObject.SetActive(false);

		GameController.rows = 10;
		gameController.StartGame();

	}
}
