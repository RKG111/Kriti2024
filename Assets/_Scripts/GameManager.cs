using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject Player;

    [SerializeField]
    public DungeonGenerator generator;

    public UIManager uIManager;
    GameObject tempParent;

    private bool paused = false;

    private bool inGame = false;
    private void Awake() {
        DungeonState.PlayerInstance = Player;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)&&inGame){
            // uIManager.Pause();
            PausePlay();

        }
    }

    public IEnumerator StartDungeon(int seed){
        
        Random.InitState(seed);
        // uIManager.ActivatePanel("loading");
        generator.SetSeedAndGenerate(seed);
        // DungeonState.PlayerInstance = Instantiate(Player,(Vector3Int)DungeonState.rooms[0].roomCenter,Quaternion.identity);
        Player.transform.position = (Vector3Int)DungeonState.rooms[0].roomCenter;
        generator.PlaceItems();
        generator.PlaceEnemies();
        inGame = true;
        // uIManager.DeactivatePanel("loading");
        yield return null;

    }


    public void PausePlay(){
        if(!paused){
            paused = true;
            Time.timeScale = 0;
            uIManager.ActivatePanel("PausePlay");
        }
        else{
            paused = false;
            Time.timeScale = 1;
            uIManager.DeactivatePanel("PausePlay");
        }
    }

    public void ExitDungeon(){

        uIManager.DeactivatePanel("death");
        uIManager.ActivatePanel("main");
        // int cost = Mathf.RoundToInt(Player.GetComponent<Inventory>().GetTotalCost()/2.1f);
        generator.DestroyDungeon();
        uIManager.coins.text = Player.GetComponent<CharacterStats>().coins.ToString();
        // Player.GetComponent<CharacterStats>().AddCoin(cost);

        inGame = false;

    }

    // public void Death(){
    //     // uIManager.DeactivatePanel("death");
    //     generator.DestroyDungeon();
    //     // uIManager.ActivatePanel("main");
    //     Player.GetComponent<CharacterStats>().coins/=2;
    //     Player.GetComponent<Inventory>().emptyInventory();
    //     uIManager.coins.text = Player.GetComponent<CharacterStats>().coins.ToString();

    //     inGame = false;

    // }

    
}
