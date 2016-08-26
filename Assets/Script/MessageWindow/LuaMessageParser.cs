using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;

public class LuaMessageParser : MonoBehaviour {

    public string file { get; private set; }
    private Script m_script;

	// Use this for initialization
	void Start () {
        m_script = new Script();
        m_script.DoFile(file);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
