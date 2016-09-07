using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;

public class LocalFile {

    private static string FilePath = Application.persistentDataPath + "/localstorage";                                     // 本地存储文件的路径
    private static string Key_ = "20540654035834f0";

    public static string Load(string _key, bool _isDecrypt = false) {
        string outStr = "";
        FileInfo fileInfo = new FileInfo(FilePath);
        if (!fileInfo.Exists) {
            return outStr;
        }
        StreamReader rs = fileInfo.OpenText();
        string line = "";
        while ((line = rs.ReadLine()) != null) {
            string[] outValue = line.Split(':');
            if (outValue.Length > 0 && outValue[0] != _key)
                continue;
            if (outValue.Length > 1 && !string.IsNullOrEmpty(outValue[1])) {
                outStr = outValue[1];
            }
            break;
        }
        //释放流
        rs.Close();
        rs.Dispose();
        //加密
        if (_isDecrypt) {
            outStr = Decrypt(outStr);
        }
        return outStr;
    }

    /// <summary>  
    /// 128位AES解密  
    /// </summary>  
    /// <param name="Decrypt"></param>  
    private static string Decrypt(string toDecrypt) {
        if (string.IsNullOrEmpty(toDecrypt)) {
            return "";
        }
        // 128-AES key      
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key_);
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return UTF8Encoding.UTF8.GetString(resultArray);
    }

    public static void Save(string _key, string _value, bool _isEncrypt = false) {
        List<string> allLineList = new List<string>();
        FileInfo fileInfo = new FileInfo(FilePath);
        StreamWriter sw;
        if (!fileInfo.Exists) {
            sw = fileInfo.CreateText();
            fileInfo.Refresh();
        } else {
            StreamReader rs = fileInfo.OpenText();
            string line = "";
            while ((line = rs.ReadLine()) != null) {
                string[] outValue = line.Split(':');
                if (outValue.Length > 0 && outValue[0] == _key) {
                    continue;
                }
                allLineList.Add(line);
            }
            rs.Close();
            rs.Dispose();
            sw = new StreamWriter(FilePath);
        }
        if (_isEncrypt) {
            _value = Encrypt(_value);
        }
        //加入新记录
        string record = _key + ":" + _value;
        allLineList.Add(record);
        // 重新写到文本
        for (int i = 0; i < allLineList.Count; i++) {
            sw.WriteLine(allLineList[i]);
        }
        sw.Close();
        sw.Dispose();
    }

    //// 加密字符串 不至于明文暴露密码之类的
    /// <summary>  
    /// 128位AES加密  
    /// </summary>  
    /// <param name="Encrypt"></param>  
    private static string Encrypt(string toEncrypt) {
        if (string.IsNullOrEmpty(toEncrypt)) {
            return "";
        }
        // 128-AES key      
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key_);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }


}
