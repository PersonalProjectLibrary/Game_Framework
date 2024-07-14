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
    [XmlElement("GameVersion")]     //注意这里是数组元素，不是单个元素，用XmlElement，不用XmlAttribute
    public VersionInfo[] GameVersion;
}

/// <summary>
/// 版本信息
/// </summary>
/// 某游戏版本及其patch等信息
[System.Serializable]
public class VersionInfo
{
    /// <summary>
    /// 游戏版本
    /// </summary>
    [XmlAttribute]  //不写参数，默认参数是变量，等价于[XmlAttribute("Version")]
    public string NowVersion;

    /// <summary>
    /// 该版本的所有热更包
    /// </summary>
    [XmlElement]    //注意这里是数组元素，不是单个元素，用XmlElement，不用XmlAttribute
    public Patchs[] Patchs;

    //游戏的渠道类型（这里暂时没添加）
}

/// <summary>
/// 热更/补丁包
/// </summary>
[System.Serializable]
public class Patchs
{
    /// <summary>
    /// 当前热更的版本
    /// 在这个版本是第几次热更
    /// </summary>
    [XmlAttribute]
    public int PatchVersion;

    /// <summary>
    /// 这次热更描述
    /// </summary>
    [XmlAttribute]
    public string Des;

    /// <summary>
    /// 每个热更包里包含哪些文件
    /// </summary>
    [XmlElement]    //注意这里是List元素，不是单个元素，用XmlElement，不用XmlAttribute
    public List<Patch> Files;
}

/// <summary>
/// 热更/补丁包内容
/// </summary>
[System.Serializable]
public class Patch
{
    /// <summary>
    /// 当前热更包名
    /// </summary>
    [XmlAttribute]
    public string Name;

    /// <summary>
    /// 下载的地址
    /// </summary>
    [XmlAttribute]
    public string Url;

    /// <summary>
    /// 当前包的平台
    /// </summary>
    [XmlAttribute]
    public string Platform;

    /// <summary>
    /// 储存这个资源的MD5码
    /// </summary>
    [XmlAttribute]
    public string Md5;

    /// <summary>
    /// 资源的大小
    /// </summary>
    [XmlAttribute]
    public float Size;
}

