using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    
    public Dictionary<string, int> Inventory;
    public List<PickaxeMaterial> OwnedPickaxes = new() {PickaxeMaterial.Stone};
    public PickaxeMaterial EquippedPickaxe = PickaxeMaterial.Stone;

}
