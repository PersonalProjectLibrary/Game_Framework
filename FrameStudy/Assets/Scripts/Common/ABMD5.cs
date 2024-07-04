using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;//xml���л�

[System.Serializable]
public class ABMD5
{
    [XmlAttribute("ABMD5List")]
    public List<ABMD5Base> ABMD5List {  get; set; }
}

[System.Serializable]//�����л�
public class ABMD5Base
{
    /// <summary>
    /// ��Դ����
    /// </summary>
    [XmlAttribute("Name")]//��xml���л�
    public string Name { get; set; }
    /// <summary>
    /// MD5��
    /// </summary>
    [XmlAttribute("Md5")]
    public string Md5 {  get; set; }
    /// <summary>
    /// �ļ���С
    /// </summary>
    [XmlAttribute("Size")]
    public float Size {  get; set; }
}
