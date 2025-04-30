using UnityEngine;

public class ShadowAnimationTrigger : MonoBehaviour
{
    private PlayerData playerData = new PlayerData();

    public void Destroy()
    {
        playerData.money = playerData.lostMoney;
        playerData.lostMoney = 0;
        playerData.lastDeathLocation = 0;

        Destroy(transform.parent.gameObject);
    }
}
