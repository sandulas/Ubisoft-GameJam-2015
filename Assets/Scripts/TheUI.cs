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
	public CanvasGroup canvasFailed;
	public CanvasGroup canvasSucces;

	public Text levelNumberLabel;



	public void FadeInDarkDelayed(float delay){
		Invoke("FadeInDark", delay);
	}
	void FadeInDark(){
		FadeDark(1);
	}

	public void FadeDark(float alpha){
		darkBackground.CrossFadeAlpha(alpha, 1f, true);
	}

	public void ShowSucces(float delay = 0){
		StartCoroutine(ShowSuccesCO(delay));
	}

	IEnumerator ShowSuccesCO(float delay){
		yield return new WaitForSeconds(delay);
		canvasSucces.gameObject.SetActive(true);
		canvasSucces.alpha = 0;
		canvasSucces.DOFade(1, .4f).SetDelay(0.3f).SetEase(Ease.Linear);
	}

	public void ShowFailed(float delay){
		StartCoroutine(ShowFailedCO(delay));
	}

	IEnumerator ShowFailedCO(float delay){
		yield return new WaitForSeconds(delay);
		canvasFailed.gameObject.SetActive(true);
		canvasFailed.alpha = 0;
		canvasFailed.DOFade(1, .4f).SetDelay(0.3f).SetEase(Ease.Linear);
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

		gameController.StartGame(levelNumber);

	}

	public void OnButtonFailedMenu(){
		canvasLevels.gameObject.SetActive(true);
		canvasLevels.alpha = 0;
		canvasLevels.DOFade(1, .4f).SetDelay(0.3f).SetEase(Ease.Linear);
		canvasFailed.DOFade(0, .3f).OnComplete(()=>{canvasFailed.gameObject.SetActive(false);});
	}

	public void OnButtonFailedRetry(){
		canvasFailed.DOFade(0, .3f).OnComplete(()=>{canvasFailed.gameObject.SetActive(false);});

		gameController.StartGame();
	}

	public void OnButtonSuccesMenu(){
		canvasLevels.gameObject.SetActive(true);
		canvasLevels.alpha = 0;
		canvasLevels.DOFade(1, .4f).SetDelay(0.3f).SetEase(Ease.Linear);
		canvasSucces.DOFade(0, .3f).OnComplete(()=>{canvasSucces.gameObject.SetActive(false);});
	}

	public void OnButtonSuccesNext(){

		canvasSucces.DOFade(0, .3f).OnComplete(()=>{canvasSucces.gameObject.SetActive(false);});

		if (gameController.currentLevelNumber != -1 && gameController.currentLevelNumber != 8)
			gameController.currentLevelNumber++;

		gameController.StartGame();
	}

	public void ShowLevelNumber(){
		StartCoroutine(ShowLevelNumberCO());
	}

	IEnumerator ShowLevelNumberCO(){
		yield return new WaitForSeconds(0.1f);
		levelNumberLabel.text = gameController.currentLevelNumber.ToString();

		levelNumberLabel.gameObject.SetActive(true);
		levelNumberLabel.rectTransform.localScale = Vector3.zero;
		levelNumberLabel.color = Utils.SetAlpha(levelNumberLabel.color, 1);

		levelNumberLabel.rectTransform.DOScale(Vector3.one * 1.5f, 1f).OnComplete(()=>{levelNumberLabel.gameObject.SetActive(false);});
		levelNumberLabel.DOColor(Utils.SetAlpha(levelNumberLabel.color, 0), 0.3f).SetDelay(0.6f);

	}

}
