using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefinition : MonoBehaviour
{ 
    public persistentType persistentType;
    public string ID;  
    private void OnValidate()
    {
        if(persistentType == persistentType.ReadWrite)//如果是读写类型
        {
            
        if(ID == string.Empty)//如果ID为空，则生成一个新的GUID
        {
            ID =System.Guid.NewGuid().ToString();
        }

        }
        else
        {
            ID = string.Empty;//如果是只读类型，则ID为空
        }
    }
}
