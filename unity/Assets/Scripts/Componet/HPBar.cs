using UnityEngine;
using System.Collections;

public class HPBar : MonoBehaviour {

    //主摄像机对象
    private Camera  my_camera;
    //红色血条贴图
    public Texture2D blood_red;
    //黑色血条贴图
    public Texture2D blood_black;
    //默认NPC血值
    [HideInInspector]
    public uint nHP = 100;
    [HideInInspector]
    public uint nMaxHP = 100;

    private float hpHeight = 1.0f;

   
	// Use this for initialization
	void Start () {
        //得到摄像机对象
        my_camera = Camera.main;

        //注解1
        //得到模型原始高度
        float size_y = collider.bounds.size.y;
        //得到模型缩放比例
        float scal_y = transform.localScale.y;
        //它们的乘积就是高度
        hpHeight = (size_y * scal_y);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnGUI()
    {
        //得到NPC头顶在3D世界中的坐标
        //默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + hpHeight, transform.position.z);
        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
        Vector2 position = my_camera.WorldToScreenPoint(worldPosition);
        
        //得到真实NPC头顶的2D坐标
        position = new Vector2(position.x, Screen.height - position.y);
        //注解2
        //计算出血条的宽高
        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red));

        //根据实际分辨率调整
        //position *= GlobalDefine.inst.ViewportRatio;
        
        //根据实际分辨率调整
        bloodSize /=( GlobalDefine.ViewportRatio*2);
        
        //通过血值计算红色血条显示区域
        int blood_width = (int)(bloodSize.x * nHP / nMaxHP);

        //根据实际分辨率调整
        //blood_width /= (int)(GlobalDefine.inst.ViewportRatio * 2);

        //先绘制黑色血条
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, bloodSize.x, bloodSize.y), blood_black);
        //在绘制红色血条
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, blood_width, bloodSize.y), blood_red);



        {
            string strHP=string.Format("{0}/{1}",nHP,nMaxHP);
            //计算NPC名称的宽高
            Vector2 strHPSize = GUI.skin.label.CalcSize(new GUIContent(strHP));
            //根据实际分辨率调整
            strHPSize *= GlobalDefine.ViewportRatio;

            //设置显示颜色为黄色
            GUI.color = Color.blue;
            //绘制NPC名称
            GUI.Label(new Rect(position.x - (strHPSize.x / 2), position.y /*- strHPSize.y/2*/ - bloodSize.y, strHPSize.x, strHPSize.y), strHP);
        }
        ////注解3
        ////计算NPC名称的宽高
        //Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(name));
        ////根据实际分辨率调整
        //nameSize *= GlobalDefine.inst.ViewportRatio;

        ////设置显示颜色为黄色
        //GUI.color = Color.yellow;
        ////绘制NPC名称
        //GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y - bloodSize.y, nameSize.x, nameSize.y), name);




    }
 
}
