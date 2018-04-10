using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    public MySqlDataSnake data;
    public InputField user;
    public InputField password;
    public Text tips;
    public Button loginBtn;
    public Button registBtn;
    //确定按钮
    public Button end;

    private void Start()
    {
        data = new MySqlDataSnake();
        end.gameObject.SetActive(false);
        user.onEndEdit.AddListener((str) =>
        {
            tips.text = "";
        });

        password.onEndEdit.AddListener((str) =>
        {
            tips.text = "";
        });
        loginBtn.onClick.AddListener(() =>
        {

            LoginUser(user.text, password.text);
        });

        registBtn.onClick.AddListener(() =>
        {
            end.gameObject.SetActive(true);
            user.text = "";
            password.text = "";
        });

        end.onClick.AddListener(() =>
        {
            SetUser(user.text, password.text);
            end.gameObject.SetActive(false);
        });
    }

    public bool GetUser(string user)
    {
        if (data.Get(user))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetUser(string user, string pass)
    {

        if (user.Length < 6 || pass.Length < 6)
        {
            tips.text = "账户或密码格式错误";
        }
        else if (user.Contains(" ") || pass.Contains(" "))
        {
            tips.text = "包含非法字符";
        }
        else
        {
            if (GetUser(user))
            {
                if (data.SetUserInfo(new UserInfo(user, pass)))
                {
                    tips.text = "注册成功";
                }
                else
                {
                    tips.text = "未知错误";
                }
            }
            else
            {
                tips.text = string.Format("注册失败,已存在该用户:{0}", user);
            }
        }


    }



    public void LoginUser(string user, string pass)
    {
        print(data.GetPassWord(user));
        if (data.GetPassWord(user) == pass)
        {
            
            tips.text = "登录成功";
        }
        else
        {
            tips.text = "登录失败，此用户未注册";
        }
    }

}
