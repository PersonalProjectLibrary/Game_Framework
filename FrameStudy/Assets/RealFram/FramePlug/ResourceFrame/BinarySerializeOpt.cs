using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using ProtoBuf;

public class BinarySerializeOpt
{
    #region Protobuf
    /************************适用于本地文件保存的Protobuf序列化和反序列化************************************/
    /// <summary>
    /// 最简单的将类序列化为文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool ProtoSerialize(string path,System.Object obj)
    {
        try
        {
            using (Stream file = File.Create(path))
            {
                Serializer.Serialize(file, obj);//将类存下来
                return true;
            }
        }
        catch (Exception e) 
        { 
            Debug.LogError(e);
            return false;
        }
    }

    /// <summary>
    /// Protobuf反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T ProtoDeSerialize<T>(string path)where T : class
    {
        try
        {
            using (Stream file = File.OpenRead(path)) { return Serializer.Deserialize<T>(file); }
        }
        catch (Exception e) 
        { 
            Debug.LogError(e);
            return null;
        }
    }

    /************************适用于网络数据传输的Protobuf序列化和反序列化************************************/
    public static byte[] ProtoSerialize(System.Object obj)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                byte[] res = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(res, 0, res.Length);
                return res;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }
    public static T ProtoDeSerialize<T>(byte[] msg) where T : class
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(msg, 0, msg.Length);
                ms.Position = 0;
                return Serializer.Deserialize<T>(ms);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }
    #endregion

    #region Xml
    /// <summary>
    /// 类序列化成xml
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool Xmlserialize(string path, System.Object obj)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    //XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    //namespaces.Add(string.Empty, string.Empty);
                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    xs.Serialize(sw, obj);
                }
            }
            return true;
            ;
        }
        catch (Exception e) { Debug.LogError("此类无法转换成xml " + obj.GetType() + "," + e); }
        return false;
    }

    /// <summary>
    /// 编辑器使读取xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T XmlDeserialize<T>(string path) where T : class
    {
        T t = default(T);
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(fs);
            }
        }
        catch (Exception e) { Debug.LogError("此xml无法转成二进制: " + path + "," + e); }
        return t;
    }

    /// <summary>
    /// Xml的反序列化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static System.Object XmlDeserialize(string path, Type type)
    {
        System.Object obj = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlSerializer xs = new XmlSerializer(type);
                obj = xs.Deserialize(fs);
            }
        }
        catch (Exception e) { Debug.LogError("此xml无法转成二进制: " + path + "," + e); }
        return obj;
    }

    /// <summary>
    /// 运行时使读取xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T XmlDeserializeRun<T>(string path) where T : class
    {
        T t = default(T);
        TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(path);

        if (textAsset == null)
        {
            UnityEngine.Debug.LogError("cant load TextAsset: " + path);
            return null;
        }
        try
        {
            using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(stream);
            }
            ResourceManager.Instance.ReleaseResouce(path, true);
        }
        catch (Exception e) { Debug.LogError("load TextAsset exception: " + path + "," + e); }
        return t;
    }
    #endregion

    #region Binary
    /// <summary>
    /// 类转换成二进制
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool BinarySerilize(string path, System.Object obj)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
            }
            return true;
        }
        catch (Exception e) { Debug.LogError("此类无法转换成二进制 " + obj.GetType() + "," + e); }
        return false;
    }

    /// <summary>
    /// 读取二进制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T BinaryDeserilize<T>(string path) where T : class
    {
        T t = default(T);
        TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(path);

        if (textAsset == null)
        {
            UnityEngine.Debug.LogError("cant load TextAsset: " + path);
            return null;
        }
        try
        {
            using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                t = (T)bf.Deserialize(stream);
            }
            ResourceManager.Instance.ReleaseResouce(path, true);
        }
        catch (Exception e) { Debug.LogError("load TextAsset exception: " + path + "," + e); }
        return t;
    }
    #endregion
}
