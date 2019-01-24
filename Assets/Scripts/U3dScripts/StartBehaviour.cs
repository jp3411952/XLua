using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class StartBehaviour : MonoBehaviour
{

    internal static LuaEnv luaenv;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        luaenv =  LuaFacade.Instance.GetLuaEvn();
        LuaFacade.Instance.CallLuaDelegate(LuaActionKey.StartGame);
            Debug.Log("Game Start");
    }

    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        LuaFacade.Instance.LuaUpdateGc();
    }

    private void OnDestroy()
    {
        LuaFacade.Instance.DeleteMe();
    }
}
