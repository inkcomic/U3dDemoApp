using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MyRegDebug : CSLE.RegHelper_Type
{
    public MyRegDebug()//重载一个专门扩展Debug.Log的注册器
        : base(typeof(UnityEngine.Debug), "Debug")
    {
        function = new MyRegDebugFunc(typeof(UnityEngine.Debug));
    }
    public class MyRegDebugFunc : CSLE.RegHelper_TypeFunction
    {
        public MyRegDebugFunc(Type type)
            : base(type)
        {

        }
        public override CSLE.CLS_Content.Value StaticCall(CSLE.CLS_Content content, string function, IList<CSLE.CLS_Content.Value> _params)
        {
            if (function == "Log")//如果是Log函数
            {
                string logformscript = _params[0].value as string;//把第一个参数取出来改改
                if (content.CallType != null)//如果有类型，就有文件名
                {
                    logformscript += "(scriptfile:" + content.CallType.filename + ")";
                }
                if(content.stackExpr.Count>0)//脚本堆栈里有东西，就有表达式，有表达式就有行数
                {
                    logformscript += "(line:" + content.stackExpr.Peek().lineBegin + ")";
                }
                _params[0].value = logformscript;
            }
            return base.StaticCall(content, function, _params);
        }
    }
}
/// <summary>
/// 这个类实现脚本的Logger接口，脚本编译时的信息会从Log输出出来
/// </summary>
class ScriptLogger : CSLE.ICLS_Logger
{

    public void Log(string str)
    {
        UnityEngine.Debug.Log(str);
    }

    public void Log_Error(string str)
    {
        Debug.LogError(str);
    }

    public void Log_Warn(string str)
    {
        Debug.LogWarning(str);
    }
}

public class ScriptMgr
{
    /// <summary>
    /// ScriptMgr用单例模式，主要是为了提供C#Light Env的初始化
    /// </summary>
    public static ScriptMgr Instance
    {
        get
        {
            if (g_this == null)
                g_this = new ScriptMgr();
            return g_this;

        }
    }
    #region forInstance
    static ScriptMgr g_this;
    public CSLE.CLS_Environment env
    {
        get;
        private set;
    }
    private ScriptMgr()
    {
        env = new CSLE.CLS_Environment(new ScriptLogger());
        env.logger.Log("C#LightEvil Inited.Ver=" + env.version);

        RegTypes();
    }
    #endregion


    /// <summary>
    /// 这里注册脚本有权访问的类型，大部分类型用RegHelper_Type提供即可
    /// </summary>
    void RegTypes()
    {
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Debug), null));
        //大部分类型用RegHelper_Type提供即可
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Vector2), "Vector2"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Vector3), "Vector3"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Vector4), "Vector4"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Time), "Time"));

        //env.RegType(new CSLE.RegHelper_Type(typeof(Debug)));
        //env.RegType(CSLE.RegHelper_Type.MakeType(typeof(MyRegDebug), "MyRegDebug"));//换我们扩展过的版本注册

        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(UnityEngine.GameObject), "GameObject"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Component), "Component"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(UnityEngine.Object), "UnityEngine.Object"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Transform), "Transform"));

        //对于AOT环境，比如IOS，get set不能用RegHelper直接提供，就用AOTExt里面提供的对应类替换
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(int[]), "int[]"));//数组要独立注册
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(List<int>), "List<int>"));//模板类要独立注册



        //每一种回调类型要独立注册
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(void), "Action")); //unity 用的dotnet 2.0 没有Action
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Action<int>), "Action<int>"));

        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Action<AssetBundle, string>), "Action<AssetBundle, string>"));

        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(StateMgr), "StateMgr"));

        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(Rect), "Rect"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(ScriptInstanceState), "ScriptInstanceState"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(PrimitiveType), "PrimitiveType"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(App), "App"));

        //UGUI
  //      env.RegType(new CSLE.RegHelper_Type(typeof(Canvas)));
  //      env.RegType(new CSLE.RegHelper_Type(typeof()));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(LocalVersion), "LocalVersion"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(System.Exception), "System.Exception"));
        env.RegType(CSLE.RegHelper_Type.MakeType(typeof(AssetBundle), "AssetBundle"));
        
 

    }

    public bool projectLoaded
    {
        get;
        private set;
    }
    public void LoadProject()
    {
        if (projectLoaded) return;
        try
        {

            string[] files = System.IO.Directory.GetFiles(Application.persistentDataPath/*Application.streamingAssetsPath*/, "*.cs", System.IO.SearchOption.AllDirectories);
            Dictionary<string, IList<CSLE.Token>> project = new Dictionary<string, IList<CSLE.Token>>();
            foreach (var v in files)
            {
                var tokens = env.tokenParser.Parse(System.IO.File.ReadAllText(v));
                project.Add(v, tokens);
            }
            env.Project_Compile(project, true);
            projectLoaded = true;
        }
        catch (Exception err)
        {

            Debug.LogError("编译脚本项目失败，请检查" + err.ToString());
        }
    }
    public void Execute(string code)
    {
        var content = env.CreateContent();


        try
        {
            var tokens = env.ParserToken(code);
            var expr = env.Expr_CompilerToken(tokens);
            expr.ComputeValue(content);
        }
        catch (Exception err)
        {
            var dumpv = content.DumpValue();
            var dumps = content.DumpStack(null);
            var dumpSys = err.ToString();
            Debug.LogError(dumpv + dumps + dumpSys);
        }
    }
}

