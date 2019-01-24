using XLua;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering;

public class LuaFacade
{
    private static LuaFacade _luafacade;
    internal static LuaEnv _lEnv;
    internal static float lastGCTime = 0;
    internal const float GCInterval = 1;//1 second 

    private Dictionary<uint,Action> luaDelegate = new Dictionary<uint, Action>();
    public  LuaFacade()
    {
        Init();
    }
    
    public static LuaFacade Instance
    {
        get
        {
            if (_luafacade == null)
            {
                _luafacade = new LuaFacade();
            }
            return _luafacade;
        }
        set{}
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        
        if (_lEnv == null)
        {
            _lEnv = new LuaEnv();
        }
        _lEnv.AddLoader(CustomLoader);
        _lEnv.DoString(LuaKeyword.require + " 'MainLua'");
        RegisterAction();
    }


    public void RegisterAction()
    {
        Action startGame =  _lEnv.Global.Get<Action>("StartGame");
        AddDelegate(LuaActionKey.StartGame, startGame);
    }
    
    
    public byte[] CustomLoader(ref string filepath)
    {
        if (filepath.Equals("MainLua"))
        {
            return null;
        }
        string apppath = Application.streamingAssetsPath;
        string path = "";
        if (filepath.Contains("View"))
        {
            apppath = Path.Combine(apppath,"Views") ;
        }else if (filepath.Contains("Cmd"))
        {
            apppath = Path.Combine(apppath,"Commands") ;
        }
        else if (filepath.Contains("Proxy"))
        {
            apppath = Path.Combine(apppath,"Proxys") ;
        }
       
      
        string tempstr ="";
        if(!Directory.Exists(apppath))
        {
            tempstr = string.Format("{0}('路径{1}未找到')", LuaKeyword.print, apppath);
            return System.Text.Encoding.UTF8.GetBytes(tempstr);
        }
        path = Path.Combine(apppath,filepath+".lua.txt") ;
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


    public void LuaUpdateGc()
    {
        if (Time.time <= lastGCTime + GCInterval)
        {
            _lEnv.Tick();
            lastGCTime = Time.time;
        }
        
    }

    public LuaEnv GetLuaEvn()
    {
        return _lEnv;
    }

    public Action GetDelegate(LuaActionKey key)
    {
        Action ac;
        luaDelegate.TryGetValue((uint) key, out ac);
        return ac;
    }

    public void CallLuaDelegate(LuaActionKey key)
    {
        Action ac = GetDelegate(key);
        ac();
    }
    
    public void AddDelegate(LuaActionKey key, Action action)
    {
        Action ac = GetDelegate(key);
    
        if (ac != null)
        {
            ac = null;
        }
        luaDelegate[(uint) key] = action;
    }
    
    public void CleanLuaDelegate()
    {
        foreach (var dg in luaDelegate)
        {
            luaDelegate[dg.Key] = null;
        }
        luaDelegate.Clear();
    }
    
    public  void DeleteMe()
    {
        CleanLuaDelegate();
        _lEnv.Dispose();
    }
    

}
