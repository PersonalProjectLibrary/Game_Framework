using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;//xml序列化

[System.Serializable]
public class ABMD5
{
    [XmlAttribute("ABMD5List")]
    public List<ABMD5Base> ABMD5List {  get; set; }
}

[System.Serializable]//可序列化
public class ABMD5Base
{
    /// <summary>
    /// 资源名称
    /// </summary>
    [XmlAttribute("Name")]//可xml序列化
    public string Name { get; set; }
    /// <summary>
    /// MD5码
    /// </summary>
    [XmlAttribute("Md5")]
    public string Md5 {  get; set; }
    /// <summary>
    /// 文件大小
    /// </summary>
    [XmlAttribute("Size")]
    public float Size {  get; set; }
}
