using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class   MyHelper 
{
    private static MyHelper _instance = null;
    public static MyHelper inst
	{
		get
		{
            if (_instance == null)
			{
                _instance = new MyHelper();
			}
            return _instance;
		}
	}

	
	public static string SwfExtension =".swf";
	public static string LevelExtension = ".txt";
	public static string PrefabExtension = ".prefab";
	
	public static string PathOfBlocksUnderResources = "Assets/Resources/LevelBuild/Blocks/";
	public static string PathOfBoccksUnderLvelBuild = "Assets/Tidy Tile Mapper/Blocks/";
	
	public static string AndroidPathOfBlocksUnderResources = "LevelBuild/Blocks/";
	
	public static Vector3 CloseToCamera = new Vector3(0, 0, 0.01f);
	
	public Action ShowAlertCallback = null;


	
	public static int Repeat(int val, int left, int right)
	{
		if (left > right) { var t = left; left = right; right = t; }
		if (right - left == 0) return left;
		
		int diff = right - left + 1;
		while (val < left)  val += diff;
		while (right < val) val -= diff;
		return val;
	}
	
	public static void WaitForSecond(float seconds)
	{
		float start = Time.realtimeSinceStartup;
		while (true)
		{
			if (Time.realtimeSinceStartup - start > seconds)
				break;
		}
	}
	

	

	
	public static bool GetChanceUnderPercent(float percent)
	{
		float f = UnityEngine.Random.Range(0.0f, 1.0f);
		return f <= percent;
	}
	
	public static int Round(float f)
	{
		return (int)Mathf.Round(f);
	}
	
	public static GameObject CreateInstanceFromPrefabUnder(string path, GameObject o)
	{
		var prefab = (Resources.Load(path, typeof(GameObject)) as GameObject);
		GameObject ret = GameObject.Instantiate(prefab) as GameObject;
		ret.name = prefab.name;
		ret.transform.parent = o.transform;
		ret.transform.localPosition = Vector3.zero;
		ret.transform.localScale = Vector3.one;
		return ret;
	}
	
	public static T GetEnumType<T>(string val)
	{
		return (T)Enum.Parse(typeof(T), val);
	}
	
	public static bool NearFloat(float x, float y)
	{
		return Mathf.Abs(x - y) < 0.01f;
	}
	
	// 得到实际值，小于0则是0，大于1则取上限
	public static int GetActualValue(float a, float b)
	{
		float val = a * b;
		if (val < 0.01f)
			val = 0;
		else
		{
			val = Mathf.Ceil(val);
		}
		return (int)Mathf.Round(val);
	}
	
	public static void CheckNotNull(object o, string objectName = "")
	{
		if (o == null)
		{
			Debug.LogError("Object " + objectName + " is null");
		}
	}
	
	public static Transform FindTransform(Transform rootTrans, string name)
	{
		Transform dt = rootTrans.Find(name);
		if (dt)
			return dt;
		else
		{
			foreach (Transform child in rootTrans)
			{
				dt = FindTransform(child, name);
				if (dt)
					return dt;
			}
		}
		return null;
	}
	
	public static T FindObject<T>(string name = null, GameObject parent = null) where T : MonoBehaviour
	{
		if (parent == null)
		{
			UnityEngine.Object[] list = GameObject.FindObjectsOfType(typeof(T));
			if (list.Length == 0)
				return null;
			
			if (name == null || name.Length == 0)
			{
				return list[0] as T;
			}
			else
			{
				for (int i = 0; i < list.Length; i++)
					if (list[i].name == name)
						return list[i] as T;
				return null;
			}
		}
		else
		{
			var p = MyHelper.FindTransform(parent.transform, name);
			if (p != null)
				return p.GetComponent<T>();
			else
				return null;
		}
	}
	
	public static void EnableCollider(GameObject o, bool enabled)
	{
		Collider c = o.gameObject.GetComponent<Collider>();
		if (c != null)
			c.enabled = enabled;
		
		int count = o.gameObject.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var child = o.gameObject.transform.GetChild(i);
			EnableCollider(child.gameObject, enabled);
		}
	}
	
	public static void EnableComponents(GameObject o, bool enabled)
	{
		Component[] components = o.GetComponents<Component>();
		if (components != null)
		{
			foreach (var c in components)
			{
				var m = (c as MonoBehaviour);
				if (m != null)
					m.enabled = enabled;
				var t = (c as MeshRenderer);
				if (t != null)
					t.enabled = enabled;
			}
		}
		
		int count = o.gameObject.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var child = o.gameObject.transform.GetChild(i);
			EnableComponents(child.gameObject, enabled);
		}
	}
	
	public static void SetLayer(GameObject o, string layer)
	{
		if (o == null)
			return;
		
		o.layer = LayerMask.NameToLayer(layer);
		
		int count = o.gameObject.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var child = o.gameObject.transform.GetChild(i);
			SetLayer(child.gameObject, layer);
		}
	}
	
	public static void EnableStatic(GameObject o, bool isStatic)
	{
		if (o == null)
			return;
		
		o.isStatic = isStatic;
		
		int count = o.gameObject.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var child = o.gameObject.transform.GetChild(i);
			EnableStatic(child.gameObject, isStatic);
		}
	}
	
	public static string GetAndroidResourcePath(GameObject o)
	{
		string path = MyHelper.GetFullPath(o);
		return path;
	}	
	
	public static bool IsInEditor
	{
		get
		{
			return Application.isEditor && Application.isPlaying == false;
		}
	}
	
	public static string CurrentSceneName
	{
		get
		{
			return Application.loadedLevelName;
		}
	}
	
	public static bool IsLevelBuild
	{
		get
		{
			return CurrentSceneName == "LevelBuild";
		}
	}
	
	public static bool IsPictureFile(string path)
	{
		string extension = Path.GetExtension(path);
		if (extension == ".png" || extension == ".gif" || extension == ".jpg")
			return true;
		return false;
	}
	
	public static bool IsSwfFile(string path)
	{
		string extension = Path.GetExtension(path);
		return extension == SwfExtension;
	}
	
	public static bool IsLevelFile(string path)
	{
		string extension = Path.GetExtension(path);
		return extension == LevelExtension;
	}
	
	public static bool IsPrefabFile(string path)
	{
		string extension = Path.GetExtension(path);
		return extension == PrefabExtension;
	}
	
	public static string GetFullPathUnderAssets(string path)
	{
		string ret = Application.dataPath + "/" + path;
		ret = ret.Replace("Assets/Assets", "Assets");
		ret = ret.Replace("Assets\\Assets", "Assets");
		return ret;
	}
	
	public static string GetPathUnderAssets(string path)
	{
		string ret = path.Replace(Application.dataPath, "");
		ret = ret.Replace("\\", "/");
		return "Assets" + ret;
	}
	
	public static string GetPathUnderResource(string path)
	{
		path = path.Replace("Assets/Resources/", "");
		string extension = Path.GetExtension(path);
		path = path.Replace(extension, "");
		return path;
	}
	
	public static string GetSwfResourceFolder(string path)
	{
		string swfResourceFolder = MyHelper.GetPathUnderAssets(path);
		swfResourceFolder = swfResourceFolder.Replace("Assets/", "Assets/Resources/");
		swfResourceFolder = Path.GetDirectoryName(swfResourceFolder);
		return swfResourceFolder;
	}
	
	public static string GetFullPath(GameObject o)
	{
		if (o == null)
			return "";
		else
		{
			if (o.transform.parent == null)
				return o.name;
			else
				return GetFullPath(o.transform.parent.gameObject) + "/" + o.name;
		}
	}
	
	public static void RemoveChildren(GameObject o)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Transform c in o.transform)
		{
			RemoveChildren(c.gameObject);
			list.Add(c.gameObject);
		}
		for (int i = 0; i < list.Count; i++)
			GameObject.DestroyImmediate(list[i]);
	}
	
	public static void BuildPath(string path, bool isLastFile)
	{
		string[] splits = path.Split(new string[]{"\\","/"}, StringSplitOptions.RemoveEmptyEntries);
		path = "";
		for (int i = 0; i < splits.Length; i++)
		{
			if (isLastFile && i == splits.Length - 1)
				break;
			
			if (i == 0)
				path = splits[i];
			else
				path = path + "/" + splits[i];
			if (Directory.Exists(path) == false)
				Directory.CreateDirectory(path);
		}
	}
	
	private static Dictionary<string, Camera> cameraDic = new Dictionary<string, Camera>();
	public static Camera GetCamera(string name)
	{
		if (cameraDic.ContainsKey(name) == false)
		{
			Camera[] c = GameObject.FindObjectsOfType(typeof(Camera)) as Camera[];
			for (int i = 0; i < c.Length; i++)
				if (c[i].name == name)
				{
					cameraDic.Add(name, c[i]);
				}
		}
		return cameraDic[name];
	}
	
	public static byte[] ConvertBytesToChars(char[] chars)
	{
		byte[] bytes = new byte[chars.Length];
		for (int i = 0; i < chars.Length; i++)
			bytes[i] = Convert.ToByte(chars[i]);
		return bytes;
	}
	
	public static char[] ConvertBytesToChars(byte[] bytes)
	{
		char[] chars = new char[bytes.Length];
		for (int i = 0; i < chars.Length; i++)
			chars[i] = Convert.ToChar(bytes[i]);
		return chars;
	}
	
	public static byte[] GetBinaryFormatData(object dsOriginal)
    {
        byte[] binaryDataResult = null;
        MemoryStream memStream = new MemoryStream();
        IFormatter brFormatter = new BinaryFormatter();
        brFormatter.Serialize(memStream, dsOriginal);
        binaryDataResult = memStream.ToArray();
        memStream.Close();
        memStream.Dispose();
        return binaryDataResult;
    }
	
    public static object RetrieveObject(byte[] binaryData)
    {
        MemoryStream memStream = new MemoryStream(binaryData);
        IFormatter brFormatter = new BinaryFormatter();
        object obj = brFormatter.Deserialize(memStream);
        return obj;
    }
	
	public static void DeleteFolder(string root)
	{
		if (Directory.Exists(root) == false)
			return;
		
		string[] files = Directory.GetFiles(root);
		foreach (var file in files)
			File.Delete(file);
		
		string[] Directories = Directory.GetDirectories(root);
		foreach (var directory in Directories)
		{
			if (directory.Contains(".svn"))
			{
				continue;
			}
			DeleteFolder(directory);
		}
		
		try
		{
			Directory.Delete(root);
		}
		catch (Exception)
		{
		}
	}
	
	private const char seperator = ' ';
	public static int[] StringToIntArray(string val)
	{
		var c = val.Split(new char[]{seperator}, StringSplitOptions.RemoveEmptyEntries);
		int[] ret = new int[c.Length];
		for (int i = 0; i < c.Length; i++)
		{
			ret[i] = Convert.ToInt32(c[i]);
		}
		return ret;
	}
	
	public static string IntArrayToString(int[] array)
	{
		string ret = "";
		for (int i = 0; i < array.Length; i++)
		{
			if (i == 0)
				ret = array[i].ToString();
			else
				ret += seperator + array[i].ToString();
		}
		return ret.Trim();
	}


    static public GameObject InstantiateFromResources(string pathName)
    {
        GameObject go = null;

        UnityEngine.Object res = Resources.Load(pathName, typeof(GameObject)) as GameObject;
        {
            go = (GameObject)GameObject.Instantiate(res);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }
        res = null;
        Resources.UnloadUnusedAssets();

        return go;
    }

}

