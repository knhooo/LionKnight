using UnityEngine;

public class ShadowAnimationTrigger : MonoBehaviour
{
    //private PlayerData playerData = new PlayerData();

    public void Destroy()
    {
        Player player = PlayerManager.instance.player;
        player.playerData.money += player.playerData.lostMoney;
        player.playerData.lostMoney = 0;
        player.playerData.lastDeathLocation = 0;
        player.playerData.isShadowAlive = false;
        DataManager.instance.SaveData();
        Destroy(transform.parent.gameObject);
    }
}
