using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public DataService DataService;

    Dictionary<string, int> _data;

    void Start() {
        _data = DataService.Data.Inventory;
    }

    void AdjustItem(string itemName, int amount) {
        bool hasItem = _data.ContainsKey(itemName);
        int currentAmount = _data[itemName];
        _data[itemName] = hasItem ? currentAmount + amount : amount;
    }
}
