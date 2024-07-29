using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AES
{
    /// <summary>
    /// 字节头，用于确认是否进行过加密，解密
    /// </summary>
    private static string AESHead = "AESEncrypt";

    /// <summary>
    /// 文件加密，传入文件路径；
    /// path：filePath带文件名和后缀，EncrptyKey：设置的密钥
    /// </summary>
    /// <param name="path">待加密的文件的路径</param>
    /// <param name="EncrptyKey">自定义的密钥</param>
    public static void AESFileEncrypt(string path, string EncrptyKey)
    {
        if (!File.Exists(path))return;

        try
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))//打开文件流，读文件
            {
                if (fs != null)
                {
                    //读取字节头，判断是否已经加密过了
                    byte[] headBuff = new byte[10];//AESHead长度为10
                    fs.Read(headBuff, 0, headBuff.Length);//把fs里0到headBuff.Length的字节读出到headBuff中
                    string headTag = Encoding.UTF8.GetString(headBuff);//将byte格式的headBuff转为string格式
                    if (headTag == AESHead)
                    {
#if UNITY_EDITOR
                        Debug.Log(path + "已经加密过了！");
#endif
                        return;
                    }
                    //加密并且写入字节头，fs表示的是： FileStream文件流
                    fs.Seek(0, SeekOrigin.Begin);//下面读之前要把光标置为0位置！！！不然可能后续出现不正确读写操作
                    byte[] buffer = new byte[fs.Length];//存储字节流数据的字节数组buffer
                    fs.Read(buffer, 0, Convert.ToInt32(fs.Length));//把fs文件内容从头读到尾，读出到buffer里
                    fs.Seek(0, SeekOrigin.Begin);//把操作字节流的光标放到起始0位置
                    fs.SetLength(0);//设置字节流长度为0，即清空字节流里的数据（fs里的数据前面已经的读到buffer中了）
                    byte[] headBuffer = Encoding.UTF8.GetBytes(AESHead);//对AESHead进行UTF-8编码转为byte存入headBuffer中
                    fs.Write(headBuffer, 0, headBuffer.Length);//将头节点0到末尾的所有内容，写入字节流fs中（前面把字节流清空过，这里写入后，字节流里也只有头节点数据）
                    byte[] EncBuffer = AESEncrypt(buffer, EncrptyKey);//对buffer进行加密（前面操作，使得buffer里存着字节流数据，这里实际是对字节流数据加密，只不过不是对字节流直接操作，而是放到buffer里操作）
                    fs.Write(EncBuffer, 0, EncBuffer.Length);//把加密后的数据写回字节流中（前面字节流清空-存头节点，现在存加密后的原数据内容，实现对字节流的加密和添加头节点，也方便后面根据头节点是否存在判断是否加密过）
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 文件解密，传入文件路径（会改动加密文件，不适合运行时）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="EncrptyKey"></param>
    public static void AESFileDecrypt(string path, string EncrptyKey)
    {
        if (!File.Exists(path)) return;

        try
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (fs != null)
                {
                    byte[] headBuff = new byte[10];
                    fs.Read(headBuff, 0, headBuff.Length);
                    string headTag = Encoding.UTF8.GetString(headBuff);
                    if (headTag == AESHead)//确定有加密过再进行解密修改文件
                    {
                        byte[] buffer = new byte[fs.Length - headBuff.Length];
                        fs.Read(buffer, 0, Convert.ToInt32(fs.Length - headBuff.Length));//将字节流数据读到buffer中
                        fs.Seek(0, SeekOrigin.Begin);//处理字节流字节的光标放到起始0位置处
                        fs.SetLength(0);//清空字节流
                        byte[] DecBuffer = AESDecrypt(buffer, EncrptyKey);//对加密文件解密
                        fs.Write(DecBuffer, 0, DecBuffer.Length);//将解密后的数据写进字节流
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 文件解密，传入文件路径，返回字节
    /// </summary>
    /// <returns></returns>
    public static byte[] AESFileByteDecrypt(string path, string EncrptyKey)
    {
        if (!File.Exists(path)) return null;
        byte[] DecBuffer = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (fs != null)
                {
                    byte[] headBuff = new byte[10];
                    fs.Read(headBuff, 0, headBuff.Length);
                    string headTag = Encoding.UTF8.GetString(headBuff);
                    if (headTag == AESHead)
                    {
                        byte[] buffer = new byte[fs.Length - headBuff.Length];
                        fs.Read(buffer, 0, Convert.ToInt32(fs.Length - headBuff.Length));
                        DecBuffer = AESDecrypt(buffer, EncrptyKey);//不对字节流进行处理，只是获取解密后的数据
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return DecBuffer;//把解密后的数据返回出去
    }

//*********************************************AES里原有的加密解密方法函数*******************************************************

    /// <summary>
    /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey">加密密钥</param>
    public static string AESEncrypt(string EncryptString, string EncryptKey)
    {
        return Convert.ToBase64String(AESEncrypt(Encoding.Default.GetBytes(EncryptString), EncryptKey));
    }

    /// <summary>
    /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey">加密密钥</param>
    public static byte[] AESEncrypt(byte[] EncryptByte, string EncryptKey)
    {
        if (EncryptByte.Length == 0) { throw (new Exception("明文不得为空")); }
        if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }
        byte[] m_strEncrypt;
        byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
        byte[] m_salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            MemoryStream m_stream = new MemoryStream();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(EncryptKey, m_salt);
            ICryptoTransform transform = m_AESProvider.CreateEncryptor(pdb.GetBytes(32), m_btIV);
            CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);
            m_csstream.Write(EncryptByte, 0, EncryptByte.Length);
            m_csstream.FlushFinalBlock();
            m_strEncrypt = m_stream.ToArray();
            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return m_strEncrypt;
    }

    /// <summary>
    /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey">解密密钥</param>
    public static string AESDecrypt(string DecryptString, string DecryptKey)
    {
        return Convert.ToBase64String(AESDecrypt(Encoding.Default.GetBytes(DecryptString), DecryptKey));
    }

    /// <summary>
    /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey">解密密钥</param>
    public static byte[] AESDecrypt(byte[] DecryptByte, string DecryptKey)
    {
        if (DecryptByte.Length == 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
        byte[] m_strDecrypt;
        byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
        byte[] m_salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            MemoryStream m_stream = new MemoryStream();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(DecryptKey, m_salt);
            ICryptoTransform transform = m_AESProvider.CreateDecryptor(pdb.GetBytes(32), m_btIV);
            CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);
            m_csstream.Write(DecryptByte, 0, DecryptByte.Length);
            m_csstream.FlushFinalBlock();
            m_strDecrypt = m_stream.ToArray();
            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return m_strDecrypt;
    }

}

