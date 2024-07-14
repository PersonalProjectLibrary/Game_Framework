using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

/// <summary>
/// 整个的数据类
/// </summary>
[System.Serializable]
public class ServerInfo
{
    /// <summary>
    /// 记录所有版本信息
    /// </summary>
    [XmlElement("GameVersions")]     //注意这里是数组元素，不是单个元素，用XmlElement，不用XmlAttribute
    public GameVersion[] GameVersions;
}

/// <summary>
/// 版本信息
/// </summary>
/// 某游戏版本及其patch等信息
[System.Serializable]
public class GameVersion
{
    /// <summary>
    /// 游戏版本
    /// </summary>
    [XmlAttribute]  //不写参数，默认参数是变量，等价于[XmlAttribute("Version")]
    public string Version;

    /// <summary>
    /// 该版本的所有热更补丁
    /// </summary>
    [XmlElement]    //注意这里是数组元素，不是单个元素，用XmlElement，不用XmlAttribute
    public Patch[] Patchs;

    //游戏的渠道类型（这里暂时没添加）
}

/// <summary>
/// 热更/补丁包
/// </summary>
[System.Serializable]
public class Patch
{
    /// <summary>
    /// 当前补丁版本
    /// </summary>
    /// 在这个版本是第几次补丁
    [XmlAttribute]
    public int PatchVersion;

    /// <summary>
    /// 这次补丁描述
    /// </summary>
    [XmlAttribute]
    public string Des;

    /// <summary>
    /// 补丁包里的所有文件
    /// </summary>
    [XmlElement]    //注意这里是List元素，不是单个元素，用XmlElement，不用XmlAttribute
    public List<PatchFile> PatchFiles;
}

/// <summary>
/// 补丁文件
/// </summary>
[System.Serializable]
public class PatchFile
{
    /// <summary>
    /// 当前补丁文件名
    /// </summary>
    [XmlAttribute]
    public string Name;

    /// <summary>
    /// 下载的地址
    /// </summary>
    [XmlAttribute]
    public string Url;

    /// <summary>
    /// 当前文件的平台
    /// </summary>
    [XmlAttribute]
    public string Platform;

    /// <summary>
    /// 储存这个文件的MD5码
    /// </summary>
    [XmlAttribute]
    public string Md5;

    /// <summary>
    /// 文件的大小
    /// </summary>
    [XmlAttribute]
    public float Size;
}

