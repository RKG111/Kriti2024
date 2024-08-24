using UnityEngine;
using System.Reflection;
//using V_AnimationSystem;

public class GameAsset : MonoBehaviour {

    private static GameAssets _i;
    public Sprite s_ShootFlash;
    
    public Sprite s_Torch;
    public Sprite s_Bandage;
    public Sprite s_Gun;
    public Sprite s_Bullets;
    public Sprite s_Shoes;
    public Sprite s_HealthPotion;
    public Sprite s_Oil;
    public Sprite s_Sword_2;

    public static GameAssets i {
        get {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

}
