// C# Example
// Builds an asset bundle from the selected objects in the project view.
// Once compiled go to "Menu" -> "Assets" and select one of the choices
// to build the Asset Bundle

using UnityEngine;
using UnityEditor;
using System.IO;


public class ExportAssetBundles {
    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]//(包含依耐关系)
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
            sw.WriteLine("文件 " + path + " 中的内容如下：");
            foreach (Object obj in Selection.objects)
            {
                sw.WriteLine("Name: " + obj.name + "\t\t\tType:" + obj.GetType());
                if (obj.GetType() == typeof(Object))
                {
                    Debug.LogWarning("Name: " + obj.name + ",   Type: " + obj.GetType() + ".   可能是unity3d不能识别的文件，可能未被打包成功");
                }
            }
            sw.Flush();
            fs.Flush();
            sw.Close();
            fs.Close();
            /*
            BuildAssetBundleOptions.CollectDependencies 包含所有依赖关系，应该是将这个物体或组件所使用的其他资源一并打包
            
            BuildAssetBundleOptions.CompleteAssets 强制包括整个资源。例如，如果传递网格到BuildPipeline.BuildAssetBundle函数并使用CompleteAssets，它还将包括游戏物体和任意动画剪辑，在同样的资源。
                                                   应该是，下载一部分，其余一并下载

            BuildAssetBundleOptions.DisableWriteTypeTree 禁用写入类型树，在资源包不包含类型信息。指定这个标识将使资源包易被脚本或Unity版本改变，但会使文件更小，更快一点加载。这个标识只影响默认包含的类型信息的平台资源包。

            BuildAssetBundleOptions.DeterministicAssetBundle 确定资源包，编译资源包使用一个哈希表储存对象ID在资源包中。
                                                             这使您可以重建的资产包，并直接引用资源。当重建资源包对象，在重建之后保证有相同的ID。由于它是一个32位的哈希表空间，如果在资源包有许多对象，这将增加潜在哈希表冲突。
                                                             Unity将给出一个错误，而不在这种情况下编译。哈希表是基于资源的GUID和对象的自身ID。
                                                             DeterministicAssetBundle加载被标准资源包慢，这是因为线程后台加载API通常期望对象排序的方式，这会使读取加少寻址。
            */
            //GC
            System.GC.Collect();
        }
    }
    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]//(不包含依耐关系)
    static void ExportResourceNoTrack () {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle"/*"unity3d"*/);
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.CompleteAssets,BuildTarget.StandaloneWindows);

            FileStream fs = File.Open(path + ".log", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("文件 " + path + " 中的内容如下：");
            foreach (Object obj in Selection.objects)
            {
                sw.WriteLine("Name: " + obj.name + "\t\t\tType: " + obj.GetType());
                if (obj.GetType() == typeof(Object))
                {
                    Debug.LogWarning("Name: " + obj.name + ",   Type: " + obj.GetType() + ".   可能是unity3d不能识别的文件，可能未被打包成功");
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
    //单独打包文件夹中的每个文件为*.unity3d文件，放在原来的位置
    [MenuItem("Assets/Build AssetBundles From Directory of Files")]
    static void ExportAssetBundleEachfile2Path()
    {
        //在项目视图从选择的文件夹生成资源包
        //记住，这个函数不跟踪依赖关系，也不是递归

        // Get the selected directory
        //获取选择的目录
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
                    //从激活的选择编译资源文件
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
        {//过滤不打包的文件类型
            ".unity3d",
            ".log",
            ".db",
        };

        string savepath = EditorUtility.OpenFolderPanel("保存目录", System.IO.Directory.GetCurrentDirectory(), "");//.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (savepath == null || savepath=="") savepath = System.IO.Directory.GetCurrentDirectory();
        bool copyfloderstruct = EditorUtility.DisplayDialog("请选择", "是否复制文件夹结构?", "复制","不复制");

        for (i = 0; i < dirs; i++)//处理同级文件夹
        {
            string path = AssetDatabase.GetAssetPath(Selection.objects[i]);
            string unity3dFileName;
            if (path.Length != 0)
            {
                //Debug.Log("path=" + path);
                if (!System.IO.Directory.Exists(path)) continue;//说明这不是一个目录
                int pos = path.LastIndexOf('/');
                if (pos<0) pos = 0;
                unity3dFileName = path.Substring(pos);
                //获取文件列表
                string[] fileEntries = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);//仅本级目录
                
                //过滤文件类型
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

                    //载入文件
                    int id = 0;
                    for (int k = 0; k < fileEntries.Length; k++)
                    {
                        if (enable[k] == false)
                        {
                            string fileName = fileEntries[k];
                            string localPath = fileName.Replace("\\", "/");
                            objects[id] = AssetDatabase.LoadMainAssetAtPath(localPath);//AssetDatabase.LoadAllAssetsAtPath不知为何不能用？
                            id++;
                        }
                    }
                    //打包
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
                        sw.WriteLine("文件 " + str + " 中的内容如下：");
                        foreach (Object obj in objects)
                        {
                            sw.WriteLine("Name: " + obj.name + "\t\t\tType: " + obj.GetType());
                            if (obj.GetType() == typeof(Object))
                            {
                                Debug.LogWarning("Name: " + obj.name + ",   Type: " + obj.GetType()+".   可能是unity3d不能识别的文件，可能未被打包成功");
                            }
                        }
                        sw.Flush();
                        fs.Flush();
                        sw.Close();
                        fs.Close();
                        Debug.Log("打包成功! " + str);
                    }
                    else
                    {
                        Debug.LogError("没有可打包的文件! 目录:" + path);
                    }
                }
                else
                {
                    Debug.LogError("没有可打包的文件! 全部被过滤, 目录:" + path);
                }
            }
        }
        //GC
        System.GC.Collect();
    }
//------------------------------------------------------------------------------------------------------------------
}