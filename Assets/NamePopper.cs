using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NamePopper : MonoBehaviour
{
    public string itemName;

    [SerializeField]

    private TMP_Text nameText;

    private void OnEnable() {
        Transform selfParent = transform.parent;
        itemName = selfParent.GetComponent<Item>().itemName;
        nameText = selfParent.GetComponent<Item>().nameText;
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player"){
            nameText.text = itemName;
        }
    }
    void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Player"){
            nameText.text = "";
        }
    }
}
