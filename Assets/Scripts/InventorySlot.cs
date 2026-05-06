using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    [SerializeField] Image itemImage;
    Item item;

    void Start() {
        
    }

    void Update() {
        
    }

    bool isEmpty() {
        return item == null;
    }
}
