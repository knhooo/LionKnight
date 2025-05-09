using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profileManage : MonoBehaviour
{
    public GameObject buttonPrefab; 
    public Transform buttonContainer; 
    public List<int> profiles = new List<int> { 1, 2, 3, 4 }; 

    public GameObject leftPrefab;
    public GameObject rightPrefab;

    private List<Button> generatedButtons = new List<Button>();

    private void Start()
    {
        StartCoroutine(GenerateProfileButtonsWithAnimation());
    }

    private IEnumerator GenerateProfileButtonsWithAnimation()
    {
        foreach (int profileNumber in profiles)
        {

            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            Button btnComponent = button.GetComponent<Button>();
            Text btnText = button.GetComponentInChildren<Text>();

            if (btnText != null)
            {
                btnText.text = "Profile " + profileNumber;
            }


            btnComponent.interactable = false;


            Animator animator = button.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Show");
            }


            CreatePrefabsForButton(button);


            generatedButtons.Add(btnComponent);


            yield return new WaitForSeconds(0.5f); 
        }

        yield return new WaitForSeconds(1f);
        EnableButtonFunctions();
    }

    private void CreatePrefabsForButton(GameObject button)
    {
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        if (buttonRect == null) return;

        Vector3 leftPosition = buttonRect.position - new Vector3(buttonRect.rect.width / 2 + 50, 0, 0);
        Vector3 rightPosition = buttonRect.position + new Vector3(buttonRect.rect.width / 2 + 50, 0, 0);

        if (leftPrefab != null)
        {
            Instantiate(leftPrefab, leftPosition, Quaternion.identity, buttonContainer);
        }

        if (rightPrefab != null)
        {
            Instantiate(rightPrefab, rightPosition, Quaternion.identity, buttonContainer);
        }
    }

    private void EnableButtonFunctions()
    {
        foreach (Button button in generatedButtons)
        {
            button.interactable = true;
            int profileNumber = profiles[generatedButtons.IndexOf(button)];
            button.onClick.AddListener(() => OnProfileButtonClick(profileNumber));
        }
    }

    private void OnProfileButtonClick(int profileNumber)
    {
        if (saveLoad.instance != null)
        {
            if (saveLoad.instance.SaveFileExists(profileNumber))
            {
                saveLoad.instance.Load(profileNumber);
            }
            else
            {
                saveLoad.instance.Save(profileNumber);
            }
        }
    }
}