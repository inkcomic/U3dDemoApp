// C# Example
// Builds an asset bundle from the selected objects in the project view.
// Once compiled go to "Menu" -> "Assets" and select one of the choices
// to build the Asset Bundle

using UnityEngine;
using UnityEditor;
using System.IO;


public class ExportAssetBundles {
    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]//(�������͹�ϵ)
    static void ExportResource () {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle"/* "assetbundle"*//*"unity3d"*/);
        if (path.Length != 0) {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
            Selection.objects = selection;

            FileStream fs = File.Open(path+".log", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("�ļ� " + path + " �е��������£�");
            foreach (Object obj in Selection.objects)
            {
                sw.WriteLine("Name: " + obj.name + "\t\t\tType:" + obj.GetType());
                if (obj.GetType() == typeof(Object))
                {
                    Debug.LogWarning("Name: " + obj.name + ",   Type: " + obj.GetType() + ".   ������unity3d����ʶ����ļ�������δ������ɹ�");
                }
            }
            sw.Flush();
            fs.Flush();
            sw.Close();
            fs.Close();
            /*
            BuildAssetBundleOptions.CollectDependencies ��������������ϵ��Ӧ���ǽ��������������ʹ�õ�������Դһ�����
            
            BuildAssetBundleOptions.CompleteAssets ǿ�ư���������Դ�����磬�����������BuildPipeline.BuildAssetBundle������ʹ��CompleteAssets��������������Ϸ��������⶯����������ͬ������Դ��
                                                   Ӧ���ǣ�����һ���֣�����һ������

            BuildAssetBundleOptions.DisableWriteTypeTree ����д��������������Դ��������������Ϣ��ָ�������ʶ��ʹ��Դ���ױ��ű���Unity�汾�ı䣬����ʹ�ļ���С������һ����ء������ʶֻӰ��Ĭ�ϰ�����������Ϣ��ƽ̨��Դ����

            BuildAssetBundleOptions.DeterministicAssetBundle ȷ����Դ����������Դ��ʹ��һ����ϣ�������ID����Դ���С�
                                                             ��ʹ�������ؽ����ʲ�������ֱ��������Դ�����ؽ���Դ���������ؽ�֮��֤����ͬ��ID����������һ��32λ�Ĺ�ϣ��ռ䣬�������Դ�����������⽫����Ǳ�ڹ�ϣ���ͻ��
                                                             Unity������һ�����󣬶�������������±��롣��ϣ���ǻ�����Դ��GUID�Ͷ��������ID��
                                                             DeterministicAssetBundle���ر���׼��Դ������������Ϊ�̺߳�̨����APIͨ��������������ķ�ʽ�����ʹ��ȡ����Ѱַ��
            */
            //GC
            System.GC.Collect();
        }
    }
    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]//(���������͹�ϵ)
    static void ExportResourceNoTrack () {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle"/*"unity3d"*/);
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.CompleteAssets,BuildTarget.StandaloneWindows);

            FileStream fs = File.Open(path + ".log", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("�ļ� " + path + " �е��������£�");
            foreach (Object obj in Selection.objects)
            {
                sw.WriteLine("Name: " + obj.name + "\t\t\tType: " + obj.GetType());
                if (obj.GetType() == typeof(Object))
                {
                    Debug.LogWarning("Name: " + obj.name + ",   Type: " + obj.GetType() + ".   ������unity3d����ʶ����ļ�������δ������ɹ�");
                }
            }
            sw.Flush();
            fs.Flush();
            sw.Close();
            fs.Close();
        }
        //GC
        System.GC.Collect();
    }
    //------------------------------------------------------------------------------------------------------------------
    //��������ļ����е�ÿ���ļ�Ϊ*.unity3d�ļ�������ԭ����λ��
    [MenuItem("Assets/Build AssetBundles From Directory of Files")]
    static void ExportAssetBundleEachfile2Path()
    {
        //����Ŀ��ͼ��ѡ����ļ���������Դ��
        //��ס���������������������ϵ��Ҳ���ǵݹ�

        // Get the selected directory
        //��ȡѡ���Ŀ¼
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        string unity3dFileName;
        Debug.Log("Selected Folder: " + path);
        if (path.Length != 0)
        {
            int pos=path.LastIndexOf('/');
            if (pos == -1)
            {
                return;
            }
            unity3dFileName = path.Substring(pos+1,path.Length-pos-1);
            Debug.Log("unity3dFileName: " + unity3dFileName);
            path = path.Replace("Assets/", "");

            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
            foreach (string fileName in fileEntries)
            {
                string filePath = fileName.Replace("\\", "/");
                int index = filePath.LastIndexOf("/");
                filePath = filePath.Substring(index);
                Debug.Log(filePath);
                string localPath = "Assets/" + path;
                if (index > 0)
                    localPath += filePath;
                Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
                if (t != null)
                {
                    Debug.Log(t.name);
                    string bundlePath = "Assets/" + path + "/" + t.name + ".unity3d";
                    Debug.Log("Building bundle at: " + bundlePath);
                    // Build the resource file from the active selection.
                    //�Ӽ����ѡ�������Դ�ļ�
                    BuildPipeline.BuildAssetBundle
                    (t, null, bundlePath, BuildAssetBundleOptions.CompleteAssets);
                }

            }
        }
        //GC
        System.GC.Collect();
    }
    [MenuItem("Assets/Build AssetBundles From by Directory")]
    static void ExportAssetBundleByDirectory()
    {
        int dirs = Selection.objects.Length,i;
        string[] filters = new string[]
        {//���˲�������ļ�����
            ".unity3d",
            ".log",
            ".db",
        };

        string savepath = EditorUtility.OpenFolderPanel("����Ŀ¼", System.IO.Directory.GetCurrentDirectory(), "");//.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (savepath == null || savepath=="") savepath = System.IO.Directory.GetCurrentDirectory();
        bool copyfloderstruct = EditorUtility.DisplayDialog("��ѡ��", "�Ƿ����ļ��нṹ?", "����","������");

        for (i = 0; i < dirs; i++)//����ͬ���ļ���
        {
            string path = AssetDatabase.GetAssetPath(Selection.objects[i]);
            string unity3dFileName;
            if (path.Length != 0)
            {
                //Debug.Log("path=" + path);
                if (!System.IO.Directory.Exists(path)) continue;//˵���ⲻ��һ��Ŀ¼
                int pos = path.LastIndexOf('/');
                if (pos<0) pos = 0;
                unity3dFileName = path.Substring(pos);
                //��ȡ�ļ��б�
                string[] fileEntries = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);//������Ŀ¼
                
                //�����ļ�����
                int  size = 0;
                bool[] enable=new bool[fileEntries.Length];
                for ( int how = 0; how < fileEntries.Length; how++)
                {
                    bool filterFlag = false;
                    foreach (string s in filters)
                    {
                        if (fileEntries[how].EndsWith(s, System.StringComparison.OrdinalIgnoreCase))
                        {//=
                            filterFlag = true;
                            break;
                        }
                    }
                    enable[how] = filterFlag;
                    if (!filterFlag) size++;
                }
                if (size != 0)
                {
                    Object[] objects = new Object[size];

                    //�����ļ�
                    int id = 0;
                    for (int k = 0; k < fileEntries.Length; k++)
                    {
                        if (enable[k] == false)
                        {
                            string fileName = fileEntries[k];
                            string localPath = fileName.Replace("\\", "/");
                            objects[id] = AssetDatabase.LoadMainAssetAtPath(localPath);//AssetDatabase.LoadAllAssetsAtPath��֪Ϊ�β����ã�
                            id++;
                        }
                    }
                    //���
                    if (id != 0)
                    {
                        string outpath = savepath;
                        if (copyfloderstruct) outpath += path.Substring(path.IndexOf('/'));

                        if (!System.IO.Directory.Exists(outpath)) System.IO.Directory.CreateDirectory(outpath);

                        //string outpath = path;
                        string str = outpath + unity3dFileName + ".unity3d";
                        // Build the resource file from the active selection.
                        BuildPipeline.BuildAssetBundle(objects[0], objects, str);//, BuildAssetBundleOptions.CompleteAssets,BuildTarget.StandaloneWindows

                        FileStream fs = File.Open(outpath + unity3dFileName + ".log", FileMode.OpenOrCreate);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine("�ļ� " + str + " �е��������£�");
                        foreach (Object obj in objects)
                        {
                            sw.WriteLine("Name: " + obj.name + "\t\t\tType: " + obj.GetType());
                            if (obj.GetType() == typeof(Object))
                            {
                                Debug.LogWarning("Name: " + obj.name + ",   Type: " + obj.GetType()+".   ������unity3d����ʶ����ļ�������δ������ɹ�");
                            }
                        }
                        sw.Flush();
                        fs.Flush();
                        sw.Close();
                        fs.Close();
                        Debug.Log("����ɹ�! " + str);
                    }
                    else
                    {
                        Debug.LogError("û�пɴ�����ļ�! Ŀ¼:" + path);
                    }
                }
                else
                {
                    Debug.LogError("û�пɴ�����ļ�! ȫ��������, Ŀ¼:" + path);
                }
            }
        }
        //GC
        System.GC.Collect();
    }
//------------------------------------------------------------------------------------------------------------------
}