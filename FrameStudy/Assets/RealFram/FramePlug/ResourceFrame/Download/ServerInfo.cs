using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

/// <summary>
/// ������������
/// </summary>
[System.Serializable]
public class ServerInfo
{
    /// <summary>
    /// ��¼���а汾��Ϣ
    /// </summary>
    [XmlElement("GameVersion")]     //ע������������Ԫ�أ����ǵ���Ԫ�أ���XmlElement������XmlAttribute
    public VersionInfo[] GameVersion;
}

/// <summary>
/// �汾��Ϣ
/// ��ǰ��Ϸ�汾��Ӧ�����в���
/// </summary>
[System.Serializable]
public class VersionInfo
{
    /// <summary>
    /// ��ǰ��Ϸ�İ汾��
    /// </summary>
    [XmlAttribute]  //��д������Ĭ�ϲ����Ǳ������ȼ���[XmlAttribute("Version")]
    public string NowVersion;

    /// <summary>
    /// ��ǰ�汾�м����ȸ���
    /// </summary>
    [XmlElement]    //ע������������Ԫ�أ����ǵ���Ԫ�أ���XmlElement������XmlAttribute
    public Patchs[] Patchs;

    //��Ϸ���������ͣ�������ʱû��ӣ�
}

/// <summary>
/// �����ܲ�����
/// </summary>
[System.Serializable]
public class Patchs
{
    /// <summary>
    /// ��ǰ�ȸ��İ汾
    /// ������汾�ǵڼ����ȸ�
    /// </summary>
    [XmlAttribute]
    public int PatchVersion;

    /// <summary>
    /// ����ȸ�����
    /// </summary>
    [XmlAttribute]
    public string Des;

    /// <summary>
    /// ÿ���ȸ����������Щ�ļ�
    /// </summary>
    [XmlElement]    //ע��������ListԪ�أ����ǵ���Ԫ�أ���XmlElement������XmlAttribute
    public List<Patch> Files;
}

/// <summary>
/// ��������
/// </summary>
[System.Serializable]
public class Patch
{
    /// <summary>
    /// ��ǰ�ȸ�����
    /// </summary>
    [XmlAttribute]
    public string Name;

    /// <summary>
    /// ��Ҫ���صĵ�ַ
    /// </summary>
    [XmlAttribute]
    public string Url;

    /// <summary>
    /// ��ǰ����ƽ̨
    /// </summary>
    [XmlAttribute]
    public string Platform;

    /// <summary>
    /// ���������Դ��MD5��
    /// </summary>
    [XmlAttribute]
    public string Md5;

    /// <summary>
    /// ��Դ�Ĵ�С
    /// </summary>
    [XmlAttribute]
    public float Size;
}

