using UnityEngine;
using System.Collections;
 
public class HUDFPS : MonoBehaviour 
{
 
// Attach this to a GUIText to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.
#if DEV
public  float updateInterval = 0.5F;
 
private float accum   = 0; // FPS accumulated over the interval
private int   frames  = 0; // Frames drawn over the interval
private float timeleft; // Left time for current interval
 
float fps;

	public static float storeFloat = 0;
	public static float storeFloat2 = 0;
	public static float storeFloat3 = 0;
	public static string showText = "";
	
void Start()
{
    timeleft = updateInterval;  
//	QualitySettings.antiAliasing = 0;
	Application.targetFrameRate = 120;
}

void Update()
{
    timeleft -= Time.deltaTime;
    accum += Time.timeScale/Time.deltaTime;
    ++frames;
 	
    // Interval ended - update GUI text and start new interval
    if( timeleft <= 0.0 )
    {
        // display two fractional digits (f2 format)
	fps = accum/frames;
//	string format = "";//System.String.Format("{0:F2} FPS",fps);
//	guiText.text = format;
 	
//	if(fps < 30)
//		guiText.material.color = Color.yellow;
//	else 
//		if(fps < 10)
//			guiText.material.color = Color.red;
//		else
//			guiText.material.color = Color.green;
	//	DebugConsole.Log(format,level);
        timeleft = updateInterval;
        accum = 0.0F;
        frames = 0;
    }
}
	
	void OnGUI () {

		GUI.contentColor = new Color(1, 0, 0, 1);
		if (Utils.isHighResDevice()){
			GUI.skin.label.fontSize = 16;	
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
		}
//		GUI.Label(new Rect(0, Screen.height - 40, 100, 20), " " + Globals.gameState.ToString());

		GUI.Label(new Rect(0, Screen.height - 25, 100, 20), System.String.Format("{0:F2} FPS",fps));
//		Debug.Log(fps);
		if (showText != ""){
			GUI.contentColor = Color.black;
			GUI.Label(new Rect(Screen.width * 0.05f, 0, Screen.width * 0.95f, 100), showText);
		}
		
	}
#endif
}