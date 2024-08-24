using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public PanelObject[] panels;

    public TMP_Text loadingSeed;
    public GameManager gameManager;

    public Transform inventory;

    public Image image;

    public Sprite vacantSprite;

    public Slider healthBar;
    public Slider staminaBar;
    public Slider timer;

    public TMP_Text greedValue;

    public TMP_Text currentValue;

    public TMP_Text coins;

    private void Start() {
        foreach(PanelObject panel in panels){
            DeactivatePanel(panel.name);
        }
        ActivatePanel("main");
    }
    #region PanelHandle
        private  GameObject GetPanel(string name){
        foreach(PanelObject item in panels){
            if(item.name == name){
                return item.panel;
            }
        }
        return null;
    }
    public void ActivatePanel(string name){
        GameObject panel = GetPanel(name);
        panel.SetActive(true);
    }
    public void DeactivatePanel(string name){
        GameObject panel = GetPanel(name);
        panel.SetActive(false);
    }
    #endregion
    
    #region StartGame
    
    public void StartGame(){

        
        // gameManager.StartDungeon(DungeonState.DungeonSeed);

        StartCoroutine(Generate());

    }
    
    IEnumerator SetSeedChangeUI(){
        DungeonState.DungeonSeed = Random.Range(100000,1000000);
        loadingSeed.text = DungeonState.DungeonSeed.ToString();
        DeactivatePanel("main");
        ActivatePanel("loading");
        yield return null;
    }
    IEnumerator Generate(){
        yield return SetSeedChangeUI();
        yield return StartCoroutine(gameManager.StartDungeon(DungeonState.DungeonSeed));

        DeactivatePanel("loading");
        ActivatePanel("stats");
    }

#endregion
    
    #region Inventory
        
    public void SetInventorySlot(int index, Sprite pickedItem){
        Image slot= inventory.GetChild(index).gameObject.GetComponent<Image>();
        slot.sprite = pickedItem;

    }
    public void EmptyInventorySlot(int index){
        SetInventorySlot(index,vacantSprite);

    }

    public Transform GetInventorySlotTransform(int index){
        return inventory.GetChild(index);
    }
    #endregion

#region Game
    public void Pause(){
        Time.timeScale = 0;
        ActivatePanel("PausePlay");
    }

    public void Play(){
        Time.timeScale = 1;
        DeactivatePanel("PausePlay");
    }

    
#endregion

}

[System.Serializable]
public class PanelObject{
    public GameObject panel;

    public string name;


}