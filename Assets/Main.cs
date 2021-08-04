using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main
{
    static LabelSerializer labelSerializer;
    /// <summary>
    /// Initialization routine when application starts
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void OnApplicationInitialized()
    {
        labelSerializer = new LabelSerializer();        
    }


}
