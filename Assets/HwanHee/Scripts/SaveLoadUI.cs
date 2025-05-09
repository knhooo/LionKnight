using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private GameObject[] SaveUI;
    [SerializeField] private GameObject EmptyUI;

    private void Awake()
    {
        foreach(GameObject saveUI in SaveUI)
        {
            saveUI.SetActive(false);
        }   

        EmptyUI.SetActive(true);
    }
}
