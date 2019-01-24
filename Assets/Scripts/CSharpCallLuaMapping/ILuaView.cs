using UnityEngine;
using XLua;

[CSharpCallLua]
public interface ILuaView
{
     GameObject this[string key] { get; set; }
     GameObject slefgo {get; set; }
     void Awake();
     void Start();
     void OnDestroy();
}
[CSharpCallLua]
public delegate ILuaView NewLuaView(GameObject go);
[CSharpCallLua]
public delegate void StartGame();
[CSharpCallLua]
public delegate void LogoutGame();