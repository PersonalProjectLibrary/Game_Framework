using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

public class BundleHotFix : EditorWindow
{
    /// <summary>
    /// 编辑器菜单下添加打包热更包初始化按钮
    /// 点击按钮初始化，生成热更包界面
    /// </summary>
    [MenuItem("Tools/打包热更包")]
    static void Init()
    {
        BundleHotFix window = (BundleHotFix)EditorWindow.GetWindow(typeof(BundleHotFix),false,"热更包界面",true);//创建热更包界面
        window.Show();//打开当前界面
    }

    //OnGUI操作、ab包路径的选择
    string md5Path = "";
    OpenFileName m_OpenFileName = null;
    
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        //内容名字：ABMD5路径，后接输入框宽500，高20
        md5Path = EditorGUILayout.TextField("ABMD5路径：",md5Path,GUILayout.Width(500),GUILayout.Height(30));

        //添加按钮：选择版本ABMD5文件。使用WindowsFile脚本，实现Unity编辑器下，打开windows窗口的功能
        if (GUILayout.Button("选择版本ABMD5文件", GUILayout.Width(200), GUILayout.Height(30)))
        {
            //设置打开的窗口
            m_OpenFileName = new OpenFileName();
            m_OpenFileName.structSize = Marshal.SizeOf(m_OpenFileName);
            m_OpenFileName.filter = "ABMD5文件(*.bytes)\0*.bytes";
            m_OpenFileName.file = new string(new char[256]);
            m_OpenFileName.maxFile = m_OpenFileName.file.Length;
            m_OpenFileName.fileTitle = new string(new char[64]);
            m_OpenFileName.maxFileTitle = m_OpenFileName.fileTitle.Length;
            m_OpenFileName.initialDir = (Application.dataPath + "/../Version").Replace("/", "\\");//默认路径
            m_OpenFileName.title = "选择MD5窗口";
            m_OpenFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;//直接复制过来的，不用管
            //最终执行的操作，实现的功能作用
            if (LocalDialog.GetOpenFileName(m_OpenFileName))
            {
                Debug.Log(m_OpenFileName.file);
                md5Path = m_OpenFileName.file;
            }
        }

        GUILayout.EndHorizontal();
    }

}
