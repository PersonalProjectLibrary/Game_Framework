using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;

public class ILRuntimeManager : Singleton<ILRuntimeManager>
{
    AppDomain m_AppDomain;

    public void Init()
    {
        LoadHotFixAssembly();
    }

    void LoadHotFixAssembly()
    {

    }
}
