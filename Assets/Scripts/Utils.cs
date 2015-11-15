using UnityEngine;
using System.Collections;

public enum HAlign{
	left = 0,
	center = 1,
	right = 2,
}

public enum VAlign{
	top = 2,
	center = 1,
	bottom = 0,
}

public class Utils : MonoBehaviour {
	
	//device
	public const string kUnknown  	= "unknown";
	public const string kIphone 	= "iphone";
	public const string kIpad 		= "ipad";
	public const string kIphone5 	= "iphone5";
	
	public const float ipadRatio = 1024.0f / 768.0f;
	public const float iphoneRatio = 480.0f / 320.0f;
	public const float iphone5Ratio = 1136.0f / 640.0f;
	public const float iphone5Ratio169 = 16.0f / 9.0f;

	public const float ipadDPI = 130;

	public enum TouchState{
		unknown = -1,
		touchNone = 0,
		touchBegin = 1,
		touchMove = 2,
		touchEnd = 3,
	}

	public static string getDeviceType(){
//		Debug.Log("screen size = " + Screen.width + " x " + Screen.height);
		float screenRatio = (float)Screen.width / (float)Screen.height;
		
		
		if (screenRatio == iphoneRatio)
			return kIphone;
		if (screenRatio == ipadRatio)
			return kIpad;
		if (screenRatio == iphone5Ratio || (screenRatio > iphone5Ratio169 * 0.9f && screenRatio < iphone5Ratio169 * 1.1f))
			return kIphone5;
		return kUnknown;	
	}
	// NO_USE_ :
	public static bool isRetinaDevice(){

		if (getDeviceType() == kIpad)
			if (Screen.width > 1500)
				return true;
		if (getDeviceType() == kIphone)
			if (Screen.width > 500)
				return true;
		if (getDeviceType() == kIphone5)
			if (Screen.width > 500)
				return true;
		return false;
	}

	public static float GetDPI(){
		// pe unele device-uri cu android screen.dpi returneaza valori dubioase
//		exemple gasite:
//		Xperia Z1 L39h DPI= 98079.22
//		Xiaomi M1 DPI= 12793.34
		float dpi = Screen.dpi;
		if (dpi < 25 || dpi > 500)
			dpi = 160;
		return dpi;
	}

	public static float GetDPIratio(){
		return GetDPI() / ipadDPI;
	}

	// functie care imi returneaza un factor de multiplicare in functie de dpi-ul deviceului .... 
	// ca etalon am ipad 1 ... ca jocul a fost gdndit pentru ipad initial ... 
	public static float GetScreenTouchMultiplier(){
//		float ipadRatio = 0.101562f; // ipad ne-retina ratie intre dpi si diagonala
//		// aflam ratia dintre DPI si diagonala deviceului
//		float ratio = GetDPI() / Mathf.Sqrt((float)(Screen.width * Screen.width + Screen.height * Screen.height));

		return ((float)Screen.height + (float)Screen.width) / (768f + 1024f); 
	}

	public static bool isHighResDevice(){
		if (Screen.width > 1366)
//		if (Screen.width > 1) // used to force simulate high res device
				return true;
		return false;
	}

	//functia returneaza true pentru deviceurile "slow" (ipad 1, iphone 4 ...)
	public static bool isSlowDevice(){ 
#if UNITY_IPHONE
		ArrayList slowDevices = new ArrayList();
		
		slowDevices.Add(iPhoneGeneration.iPhoneUnknown);
		slowDevices.Add(iPhoneGeneration.iPhone);
		slowDevices.Add(iPhoneGeneration.iPhone3G);
		slowDevices.Add(iPhoneGeneration.iPhone3GS);
		slowDevices.Add(iPhoneGeneration.iPhone4);
		
		slowDevices.Add(iPhoneGeneration.iPadUnknown);
		slowDevices.Add(iPhoneGeneration.iPad1Gen);
		
		slowDevices.Add(iPhoneGeneration.iPodTouchUnknown);
		slowDevices.Add(iPhoneGeneration.iPodTouch1Gen);
		slowDevices.Add(iPhoneGeneration.iPodTouch2Gen);
		slowDevices.Add(iPhoneGeneration.iPodTouch3Gen);
		slowDevices.Add(iPhoneGeneration.iPodTouch4Gen);
		
//		Debug.Log(iPhone.generation);
		for (int i = 0; i < slowDevices.Count; i++){
			if (iPhone.generation == (iPhoneGeneration)slowDevices[i]){
//				Debug.Log(iPhone.generation + " " + (iPhoneGeneration)slowDevices[i]);
				return true;	
			}
		}
//		Debug.Log("fals");
//		if (slowDevices.Contains(iPhone.generation))
//			return true;
#endif
		return false;
	}	
	
	public static string stripFileNameDeviceSuffix(string fileName){
		string ret = fileName;
		ret = ret.Replace("@2x", "");
		ret = ret.Replace("~ipad", "");
		ret = ret.Replace("~iphone5", "");
		ret = ret.Replace("~iphone", "");
		return ret;
	}
	
	public static string getFileNameDeviceSuffix(){
		string ret = "";
		// device
		if (Utils.getDeviceType() == Utils.kIpad)
			ret += "~ipad";
		if (Utils.getDeviceType() == Utils.kIphone)
			ret += "~iphone";
		if (Utils.getDeviceType() == Utils.kIphone5)
			ret += "~iphone5";
		//retina
		if (Utils.isRetinaDevice())
			ret += "@2x";
		
		return ret;
	}
	
	public static string stripFileNameMultiplierDeviceSuffix(string fileName){
		string ret = fileName;
		ret = ret.Replace("@1x", "");
		ret = ret.Replace("@2x", "");
		ret = ret.Replace("@4x", "");
		return ret;
	}
	
	public static string NO_USE_getFileNameMultiplierDeviceSuffix(){

		int mult = 1;
		if (Utils.getDeviceType() == Utils.kIpad)
			mult *= 2;
		if (Utils.isRetinaDevice())
			mult *= 2;

		return "@" + mult.ToString() + "x";
	}
	
	public static Vector2 getTouchPositionVector2(int mouseButton = 0){
		Vector2 pos = Vector2.zero;
		if (Input.touchCount > 0){
			Touch theTouch = Input.touches[0];
			pos = theTouch.position;
		}
		else{
			if (Input.GetMouseButton(mouseButton) || Input.GetMouseButtonUp(mouseButton) || Input.GetMouseButtonDown(mouseButton)){
				Vector3 posV3 = Input.mousePosition;
				pos = new Vector2(posV3.x, posV3.y);
			}
		}
		return pos;
	}
	
	public static Vector3 getTouchPositionVector3(int mouseButton = 0){
		Vector3 pos = Vector3.zero;
		if (Input.touchCount > 0){
			Touch theTouch = Input.touches[0];
			pos = new Vector3(theTouch.position.x, theTouch.position.y, 0);
		}
		else{
			if (Input.GetMouseButton(mouseButton) || Input.GetMouseButtonUp(mouseButton) || Input.GetMouseButtonDown(mouseButton)){
				pos = Input.mousePosition;
			}
		}
		return pos;
	}
	
	public static TouchState getTouchState(int mouseButton = 0){
		TouchState ret = TouchState.touchNone;
		if (Input.touchCount > 0){
			Touch theTouch = Input.touches[0];
			if (theTouch.phase == TouchPhase.Began)
				ret = TouchState.touchBegin;
			else if (theTouch.phase == TouchPhase.Ended || theTouch.phase == TouchPhase.Canceled)
				ret = TouchState.touchEnd;
			else 
				ret = TouchState.touchMove;
		}
		else{
			if (Input.GetMouseButtonDown(mouseButton)){
				ret = TouchState.touchBegin;
			}
			else if (Input.GetMouseButtonUp(mouseButton)){
				ret = TouchState.touchEnd;
			}
			else if (Input.GetMouseButton(mouseButton)){
				ret = TouchState.touchMove;
			}
		}
		return ret;
	}
	
	
	
	public static int getTouchCount(){
		if (Input.touchCount == 0){
			if (Input.GetMouseButton(0))
				return 1;
		}
		return Input.touchCount;
	}
	
	public static bool getTouchForFingerId(int fingerId, out Touch theTouch){
		theTouch = new Touch();
		foreach (Touch touch in Input.touches){
			if (touch.fingerId == fingerId){
				theTouch = touch;
				return true;
			}
		}
		
		return false;	
	}
	
	public static Vector3 vector2ToVector3(Vector2 v){
		return new Vector3(v.x, v.y, 0);	
	}
	
	// returneaza proiectia unui punct de pe ecran pe un plan din lumea 3D
	public static Vector3 getScreenPointOnPlane(Vector3 scrPoint, Plane plane, Camera cam){
		Ray ray = cam.ScreenPointToRay(scrPoint);
		float distanceOnRay;
		plane.Raycast(ray, out distanceOnRay);
		return ray.GetPoint(distanceOnRay);
	}
	
	public static Color lightenColor(Color col, float amount){
		float tmp_r = col.r;
		float tmp_g = col.g;
		float tmp_b = col.b;
		
		tmp_r = (1 - col.r) * amount + col.r;
        tmp_g = (1 - col.g) * amount + col.g;
        tmp_b = (1 - col.b) * amount + col.b;
		
		if (tmp_r > 1 || tmp_g > 1 || tmp_b > 1) 
            return col;
        else 
            return new Color(tmp_r, tmp_g, tmp_b, col.a);
	}

	public static Color DesaturateColor(Color col, float amount){
		float L = 0.3f * col.r + 0.6f * col.g + 0.1f * col.b;
		float new_r = col.r + amount * (L - col.r);
		float new_g = col.g + amount * (L - col.g);
		float new_b = col.b + amount * (L - col.b);
		return new Color(new_r, new_g, new_b, col.a);
	}

	public static Color ColorFromHSV(float h, float s, float v, float a = 1)
	{
		// no saturation, we can return the value across the board (grayscale)
		if (s == 0)
			return new Color(v, v, v, a);
		
		// which chunk of the rainbow are we in?
		float sector = h / 60;
		
		// split across the decimal (ie 3.87 into 3 and 0.87)
		int i = (int)sector;
		float f = sector - i;
		
		float p = v * (1 - s);
		float q = v * (1 - s * f);
		float t = v * (1 - s * (1 - f));
		
		// build our rgb color
		Color color = new Color(0, 0, 0, a);
		
		switch(i)
		{
		case 0:
			color.r = v;
			color.g = t;
			color.b = p;
			break;
			
		case 1:
			color.r = q;
			color.g = v;
			color.b = p;
			break;
			
		case 2:
			color.r  = p;
			color.g  = v;
			color.b  = t;
			break;
			
		case 3:
			color.r  = p;
			color.g  = q;
			color.b  = v;
			break;
			
		case 4:
			color.r  = t;
			color.g  = p;
			color.b  = v;
			break;
			
		default:
			color.r  = v;
			color.g  = p;
			color.b  = q;
			break;
		}
		
		return color;
	}

	public static Color SetAlpha(Color color, float alpha){
		return new Color(color.r, color.g, color.b, alpha);
	}
	
	public static float RoundFloat(float number, int decimals = 2){
		return  Mathf.Round(number * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals);	
	}
	
	public static Vector3 RoundVector3(Vector3 vector, int decimals = 2){
		Vector3 ret = vector;
		ret.x = RoundFloat(ret.x, decimals);
		ret.y = RoundFloat(ret.y, decimals);
		ret.z = RoundFloat(ret.z, decimals);
		return ret;
	}
	
	public static Vector3 ClampVector3(Vector3 vector, Vector3 minim, Vector3 maxim){
		Vector3 ret = vector;
		ret.x = Mathf.Clamp(vector.x, minim.x, maxim.x);
		ret.y = Mathf.Clamp(vector.y, minim.y, maxim.y);
		ret.z = Mathf.Clamp(vector.z, minim.z, maxim.z);
		return ret;
	}
	
	public static string Vector3ToString(Vector3 vector, int decimals = 2){
		string text = "";
		text +=  RoundFloat(vector.x, decimals).ToString("F" + decimals.ToString());
		text += ";";
		text +=  RoundFloat(vector.y, decimals).ToString("F" + decimals.ToString());
		text += ";";
		text +=  RoundFloat(vector.z, decimals).ToString("F" + decimals.ToString());
		return text;
	}

	public static string QuaternionToString(Quaternion quaternion, int decimals = 2){
		string text = "";
		text +=  RoundFloat(quaternion.w, decimals).ToString("F" + decimals.ToString());
		text += ";";
		text +=  RoundFloat(quaternion.x, decimals).ToString("F" + decimals.ToString());
		text += ";";
		text +=  RoundFloat(quaternion.y, decimals).ToString("F" + decimals.ToString());
		text += ";";
		text +=  RoundFloat(quaternion.z, decimals).ToString("F" + decimals.ToString());
		return text;
	}
	
	public static Vector3 StringToVector3(string text){
		Vector3 vector = Vector3.zero;
		string[] words = text.Split(';');
		if (words.Length != 3){
			Debug.Log("StringToVector3 Failed:" + text);
			return vector;
		}
		vector.x = float.Parse(words[0]);
		vector.y = float.Parse(words[1]);
		vector.z = float.Parse(words[2]);
		return vector;
	}

	public static Quaternion StringToQuaternion(string text){
		Quaternion quaternion = Quaternion.identity;
		string[] words = text.Split(';');
		if (words.Length != 4){
			Debug.Log("StringToQuaternion Failed:" + text);
			return quaternion;
		}
		quaternion.w = float.Parse(words[0]);
		quaternion.x = float.Parse(words[1]);
		quaternion.y = float.Parse(words[2]);
		quaternion.z = float.Parse(words[3]);
		return quaternion;
	}
	
	public static int ColorToInt(Color32 colorRGB){
		int ret = colorRGB.r * 256 * 256 + colorRGB.g * 256 + colorRGB.b;
		return ret;
	}
	
	public static Color32 IntToColor(int colorINT){
		Color32 color32 = new Color32(0, 0, 0, 255);
		color32.b = (byte)(colorINT % 256);
		color32.g = (byte)((colorINT / 256) % 256);
		color32.r = (byte)((colorINT / 256 / 256) % 256);
		return color32;
	}
	
	public static string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}
	 
	public static Color32 HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}
	
	public static Bounds Rotate90Bounds(Bounds bounds, Vector3 toRotate){
		// calculam noile extents ... adica rotim extents
		bounds.extents = Quaternion.Euler(toRotate) * bounds.extents;
		bounds.extents = new Vector3(Mathf.Abs(bounds.extents.x), Mathf.Abs(bounds.extents.y), Mathf.Abs(bounds.extents.z));
		
		toRotate.Normalize();
		// calculam noua pozitie in functie de dimensiune si directia de rotire
		// pentrul cazul cand trebuia dupa rotatie snap la grid ... nu mai este necesar pentru ca pozitionarea se face cu precizie de 0.5 unitati si scalarea cu precizie de 1
		float sizeSum = 0;
		if (toRotate.x != 0){
			sizeSum = bounds.size.y + bounds.size.z;
			if (Utils.RoundFloat(sizeSum, 0) % 2 == 1){
				if (bounds.size.y < bounds.size.z){
					bounds.center += new Vector3(0, 0.5f, 0.5f);	
				}
				else{
					bounds.center += new Vector3(0, -0.5f, -0.5f);
				}
			}
		}
		else if (toRotate.y != 0){
			sizeSum = bounds.size.x + bounds.size.z;
			if (Utils.RoundFloat(sizeSum, 0) % 2 == 1){
				if (bounds.size.z < bounds.size.x){
					bounds.center += new Vector3(0.5f, 0, 0.5f);	
				}
				else{
					bounds.center += new Vector3(-0.5f, 0, -0.5f);
				}
			}
		}
		else if (toRotate.z != 0){
			sizeSum = bounds.size.x + bounds.size.y;
			if (Utils.RoundFloat(sizeSum, 0) % 2 == 1){
				if (bounds.size.y < bounds.size.x){
					bounds.center += new Vector3(0.5f, 0.5f, 0);	
				}
				else{
					bounds.center += new Vector3(-0.5f, -0.5f, 0);
				}
			} 
		}
		

		return bounds;
	}

	public static float DistanceToRay( Vector3 X0, Ray ray) {
		Vector3 X1  = ray.origin; // get the definition of a line from the ray
		Vector3 X2  = ray.origin + ray.direction;
		Vector3 X0X1 = (X0-X1); 
		Vector3 X0X2 = (X0-X2); 
		
		return ( Vector3.Cross(X0X1,X0X2).magnitude / (X1-X2).magnitude ); // magic
	}

	// verifica daca un gameobject este in layerMask
	public static bool IsInLayerMask(GameObject obj, LayerMask mask){
		return ((mask.value & (1 << obj.layer)) > 0);
	}
	// schimba layer-ul unui gameObject si aplica recursiv noul layer al toti copii
	public static void ChangeLayerRecursive(GameObject targetObject, int newLayer){
		if (targetObject == null)
			return;
		if (targetObject.layer == newLayer)
			return;
		targetObject.layer = newLayer;
		foreach(Transform childObject in targetObject.transform)
			ChangeLayerRecursive(childObject.gameObject, newLayer);
	}

	// functie care imparte timpul din numar de secunde (float) in minute, seconde si sutimi (int)
	public static void GetSplitedTime(float time, out int minutes, out int seconds, out int hundreds){
		time = RoundFloat(time, 2);
		minutes  = Mathf.FloorToInt(time) / 60;
		seconds  = Mathf.FloorToInt(time) % 60;
		string txt = time.ToString("F2");
		hundreds = int.Parse(txt.Substring(txt.Length - 2, 2));
	}

	
	// obsolete: // nu cred ca mai e nevoie de ele .... 
	
	public static float roundZecimals(float number, int zecimals = 3){ // functia cred ca e gresita
		float ret = number;
		ret *= 10f * zecimals; // aici e greseala ... cred ... sunt prea obosit sa imi bat capu
		ret = Mathf.Round(ret);
		ret /= 10f * zecimals;
		return ret;
	}
	
	public static Vector3 roundZecimals(Vector3 vector, int zecimals = 3){
		Vector3 ret = vector;
		ret.x = roundZecimals(ret.x, zecimals);
		ret.y = roundZecimals(ret.y, zecimals);
		ret.z = roundZecimals(ret.z, zecimals);
		return ret;
	}
	
	public static float GetBoundsDiagonal(Bounds bounds){
		return Mathf.Sqrt(bounds.size.x * bounds.size.x + bounds.size.y * bounds.size.y + bounds.size.z * bounds.size.z);
	}

	public static bool AreEqualBounds(Bounds boundsA, Bounds boundsB){
		if (boundsA.center == boundsB.center && boundsA.size == boundsB.size)
			return true;
		return false;
	}

	public static Bounds CopyOfBounds(Bounds original){
		return new Bounds(original.center, original.size);
	}
//////////////////
// ALIGN BOUNDS
/////////////////

	// Align X
	public static Vector3 GetPositionAlignXLeftOut(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.x = alignTo.center.x - alignTo.extents.x - align.extents.x;
		return ret;
	}
	public static Vector3 GetPositionAlignXLeftIn(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.x = alignTo.center.x - alignTo.extents.x + align.extents.x;
		return ret;
	}
	public static Vector3 GetPositionAlignXCenter(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.x = alignTo.center.x;
		return ret;
	}
	public static Vector3 GetPositionAlignXRightIn(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.x = alignTo.center.x + alignTo.extents.x - align.extents.x;
		return ret;
	}
	public static Vector3 GetPositionAlignXRightOut(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.x = alignTo.center.x + alignTo.extents.x + align.extents.x;
		return ret;
	}

	// Align Y
	public static Vector3 GetPositionAlignYLeftOut(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.y = alignTo.center.y - alignTo.extents.y - align.extents.y;
		return ret;
	}
	public static Vector3 GetPositionAlignYLeftIn(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.y = alignTo.center.y - alignTo.extents.y + align.extents.y;
		return ret;
	}
	public static Vector3 GetPositionAlignYCenter(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.y = alignTo.center.y;
		return ret;
	}
	public static Vector3 GetPositionAlignYRightIn(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.y = alignTo.center.y + alignTo.extents.y - align.extents.y;
		return ret;
	}
	public static Vector3 GetPositionAlignYRightOut(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.y = alignTo.center.y + alignTo.extents.y + align.extents.y;
		return ret;
	}

	// Align Z
	public static Vector3 GetPositionAlignZLeftOut(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.z = alignTo.center.z - alignTo.extents.z - align.extents.z;
		return ret;
	}
	public static Vector3 GetPositionAlignZLeftIn(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.z = alignTo.center.z - alignTo.extents.z + align.extents.z;
		return ret;
	}
	public static Vector3 GetPositionAlignZCenter(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.z = alignTo.center.z;
		return ret;
	}
	public static Vector3 GetPositionAlignZRightIn(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.z = alignTo.center.z + alignTo.extents.z - align.extents.z;
		return ret;
	}
	public static Vector3 GetPositionAlignZRightOut(Bounds align, Bounds alignTo){
		Vector3 ret = align.center;
		ret.z = alignTo.center.z + alignTo.extents.z + align.extents.z;
		return ret;
	}
}
