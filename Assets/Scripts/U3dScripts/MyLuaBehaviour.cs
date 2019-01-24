using System;
using UnityEngine;
using UnityEngine.UI;
using XLua;



[LuaCallCSharp]
public class MyLuaBehaviour :MonoBehaviour
{
    internal static LuaEnv _env;
    private ILuaView _luaView;

    private NewLuaView newluaView;
    public GameObject[] objs;
    void Awake()
    {
        _env = LuaFacade.Instance.GetLuaEvn();
        newluaView  = _env.Global.GetInPath<NewLuaView>("CreateLoginView");
        if (newluaView == null)
        {
            Debug.Log("未找到LoginView.New");
            return;
        }
        _luaView = newluaView(this.gameObject);
        if (_luaView == null)
        {
            Debug.Log("_luaView not create");
            return;
        }
        foreach (var obj in objs)
        {
            _luaView[obj.name] = obj;
        }

        if (_luaView != null)
        {
            _luaView.Awake();
        }
        
    }

    private void Start()
    {
        if (_luaView != null)
        {
            _luaView.Start();
        }
    }

    private void OnDestroy()
    {
        if (_luaView != null)
             _luaView.OnDestroy();
        _luaView = null;
        newluaView = null;
    }
}
