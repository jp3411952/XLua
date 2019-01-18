using pattern;
using XLua;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Rendering;

public class LuaFacade : Singleton<LuaFacade>
{
    private LuaEnv _lEnv;
    public  LuaFacade()
    {
        if (_lEnv == null)
        {
            _lEnv = new LuaEnv();
        }
        Init();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        _lEnv.AddLoader(CustomLoader);
        _lEnv.DoString(LuaKeyword.require + " 'MainLua'");
    }


    public void Test()
    {
        int a  =  _lEnv.Global.Get<int>("aab");
        Debug.Log(a);
    }
    
    
    public byte[] CustomLoader(ref string filepath)
    {
        if (filepath.Equals("MainLua"))
        {
            return null;
        }
        string apppath = Application.streamingAssetsPath;
        string tempstr ="";
        if(!Directory.Exists(apppath))
        {
            tempstr = string.Format("{0}('路径{1}未找到')", LuaKeyword.print, apppath);
            return System.Text.Encoding.UTF8.GetBytes(tempstr);
        }
        string path = Path.Combine(apppath,filepath+".lua.txt") ;
        if (path.Length > 0 && path.Contains("\\"))
        {
            path = path.Replace("\\", "/");
        }
        if (!File.Exists(path))
        {
            tempstr = string.Format("{0}('文件{1}未找到')", LuaKeyword.print, path);
            return System.Text.Encoding.UTF8.GetBytes(tempstr);
        }
        Byte[] readAllBytes = File.ReadAllBytes(path);
        return readAllBytes;
    }
    

    public LuaEnv GetLuaEvn()
    {
        return _lEnv;
    }
    
    public override void BeforeDeleMe()
    {
        _lEnv.Dispose();
    }
    

}
