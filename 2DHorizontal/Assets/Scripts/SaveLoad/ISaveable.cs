using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{ 
    DataDefinition GetDataID(); //声明一个方法，用于获取数据ID
    //声明一个方法，用于注册保存数据,
    void RegisterSaveaDate()
    {
        DataManager.instance.RegisterSaveaDate(this);//注册保存数据
    }
    //如果物体消失或敌人死亡，场景卸载，就从保存数据中删除该物体    
    //大括号中只有一行代码，可以用简写形式，即：
    void UnRegisterSaveDate() => DataManager.instance.UnRegisterSaveDate(this);

    void GetSaveDate(Data data);//获取需要保的存数据
    void loadData(Data data); //把本来存的数据，通知对象（角色（保存时的物品，血量，位置等），怪物，道具等）加载数据

}
 