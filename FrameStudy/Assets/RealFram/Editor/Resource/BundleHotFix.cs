using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

public class BundleHotFix : EditorWindow
{
    /// <summary>
    /// �༭���˵�����Ӵ���ȸ�����ʼ����ť
    /// �����ť��ʼ���������ȸ�������
    /// </summary>
    [MenuItem("Tools/����ȸ���")]
    static void Init()
    {
        BundleHotFix window = (BundleHotFix)EditorWindow.GetWindow(typeof(BundleHotFix),false,"�ȸ�������",true);//�����ȸ�������
        window.Show();//�򿪵�ǰ����
    }

    //OnGUI������ab��·����ѡ��
    string md5Path = "";
    /// <summary>
    /// ��ǰ�汾�ȸ��������ȸ��˼��Σ�Ĭ��1
    /// ���������ñ�ʱ����õ�
    /// </summary>
    string hotCount = "1";
    OpenFileName m_OpenFileName = null;
    
    private void OnGUI()
    {
        //��Ӱ�ť��ѡ��汾ABMD5�ļ���ʹ��WindowsFile�ű���ʵ��Unity�༭���£���windows���ڵĹ���
        //WindowsFile�ű�λ�ã�Assets/RealFrame/Editor/Resource/WindowsFile.cs
        GUILayout.BeginHorizontal();
        md5Path = EditorGUILayout.TextField("ABMD5·����",md5Path,GUILayout.Width(500),GUILayout.Height(30));
        if (GUILayout.Button("ѡ��汾ABMD5�ļ�", GUILayout.Width(200), GUILayout.Height(30)))
        {
            //���ô򿪵Ĵ���
            m_OpenFileName = new OpenFileName();
            m_OpenFileName.structSize = Marshal.SizeOf(m_OpenFileName);
            m_OpenFileName.filter = "ABMD5�ļ�(*.bytes)\0*.bytes";
            m_OpenFileName.file = new string(new char[256]);
            m_OpenFileName.maxFile = m_OpenFileName.file.Length;
            m_OpenFileName.fileTitle = new string(new char[64]);
            m_OpenFileName.maxFileTitle = m_OpenFileName.fileTitle.Length;
            m_OpenFileName.initialDir = (Application.dataPath + "/../Version").Replace("/", "\\");//Ĭ��·��
            m_OpenFileName.title = "ѡ��MD5����";
            m_OpenFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;//ֱ�Ӹ��ƹ����ģ����ù�
            //����ִ�еĲ�����ʵ�ֵĹ�������
            if (LocalDialog.GetOpenFileName(m_OpenFileName))
            {
                Debug.Log(m_OpenFileName.file);
                md5Path = m_OpenFileName.file;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        hotCount = EditorGUILayout.TextField("�ȸ������汾��",hotCount,GUILayout.Width(350),GUILayout.Height(30));//��ʾ�ȸ�����
        //���ȸ����İ�ť
        if (GUILayout.Button("��ʼ���ȸ���", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (!string.IsNullOrEmpty(md5Path) && md5Path.EndsWith(".bytes"))
            {
                BundleEditor.Build(true, md5Path, hotCount);//����bundleEditor�����ȸ����
            }
        }
        GUILayout.EndHorizontal();
    }
}
