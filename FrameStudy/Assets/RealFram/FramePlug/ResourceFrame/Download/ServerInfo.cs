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
    [XmlAttribute("GameVersion")]
    public VersionInfo[] GameVersion;
}

/// <summary>
/// 版本信息
/// 当前游戏版本对应的所有补丁
/// </summary>
[System.Serializable]
public class VersionInfo
{
    /// <summary>
    /// 当前游戏的版本号
    /// </summary>
    [XmlAttribute]  //不写参数，默认参数是变量，等价于[XmlAttribute("Version")]
    public string nowVersion;

    //当前版本有几个热更包
    [XmlAttribute]
    public Patchs[] Patchs;

    //游戏的渠道类型（这里暂时没添加）
}

/// <summary>
/// 单个总补丁包
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

    //每个热更包里包含哪些文件
    [XmlAttribute]
    public List<Patch> Files;
}

/// <summary>
/// 单个补丁
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
    /// 需要下载的地址
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

