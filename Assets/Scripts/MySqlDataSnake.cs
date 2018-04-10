using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System.Data;
using System;
public struct UserInfo
{
    public string UserName;
    public string PassWord;

    public UserInfo(string name, string pass)
    {
        UserName = name;
        PassWord = pass;
    }
    public override string ToString()
    {
        return string.Format("UserName:{0},PassWord:{1}", UserName, PassWord);
    }

}


public class MySqlDataSnake
{


    /// <summary>
    /// 声明一个mysql数据库链接
    /// </summary>
    private static MySqlConnection connect;

    public static MySqlConnection Connect
    {
        get
        {
            return connect;
        }
        set
        {
            connect = value;
        }
    }

    public static string _database = "userinfo_data";
    public static string _host = "127.0.0.1";
    public static string _id = "root";
    public static string _pwd = "123456";
    public static string _port = "3306";

    public MySqlDataSnake()
    {
        ConnectData(_database, _host, _id, _pwd, _port);
    }
    public static void ConnectData(string database, string host, string id, string pwd, string port)
    {
        string str = string.Format("server = {0}; port = {1} ; database = {2};User Id={3};password ={4};", host, port, database, id, pwd);
        Connect = new MySqlConnection(str);

        try
        {
            Connect.Open();
            Debug.Log("数据库打开成功---->>>>");
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message + "数据库连接失败");
        }

    }
    ~MySqlDataSnake()
    {
        connect.Dispose();
        if (connect.State == ConnectionState.Closed)
        {
            Debug.Log("已释放链接---->>>>>>" + connect.Database + "---->>Compelete");
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="SQLString"></param>
    /// <param name="cmdParms"></param>
    /// <returns></returns>
    public List<UserInfo> Query(string SQLString, params MySqlParameter[] cmdParms)
    {
        try
        {
            Debug.Log(Connect.State);
            MySqlCommand cmd = new MySqlCommand(SQLString);
            cmd.Connection = Connect;
            MySqlDataReader reader = cmd.ExecuteReader();
            List<UserInfo> list = new List<global::UserInfo>();
            while (reader.Read())
            {
                list.Add(new UserInfo(reader["UserName"].ToString(), reader["PassWord"].ToString()));
            }
            cmd.Dispose();
            reader.Close();
            return list;
        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }

        return null;
    }
    /// <summary>
    /// 获取数据库是否存在该名字
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns></returns>
    public bool Get(string userName)
    {
        List<UserInfo> list = Query("SELECT * FROM userinfo_data.userinfo;");

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].UserName == userName)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 根据用户查找密码
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string GetPassWord(string userName)
    {
        List<UserInfo> list = Query("SELECT * FROM userinfo_data.userinfo;");
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].UserName == userName)
            {
                return list[i].PassWord;
            }
        }
        return "";
    }

    /// <summary>
    /// 设置用户信息
    /// </summary>
    /// <param name="info">用户名和密码</param>
    /// <returns></returns>
    public bool SetUserInfo(UserInfo info)
    {
        try
        {
            if (Get(info.UserName))
            {
                Debug.Log(string.Format("已存在该用户{0}", info.UserName));
                return false;
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand(string.Format("INSERT INTO `userinfo_data`.`userinfo` (`UserName`, `PassWord`) VALUES ('{0}', '{1}');", info.UserName, info.PassWord));
                cmd.Connection = connect;
                int line = cmd.ExecuteNonQuery();
                Debug.Log("line:" + line);
                if (line != 0)
                {
                    cmd.Dispose();

                    return true;
                }
                cmd.Dispose();
                return false;
            }
        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
            return false;
        }
    }
}
