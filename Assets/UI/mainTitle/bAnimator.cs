using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class bAnimator : MonoBehaviour
{
    public RectTransform[] buttons;
    public RectTransform lArrrow;
    public RectTransform rArrrow;
    public float aniDuration = 0.5f;

    private List<Animator> lArrowAnimators = new List<Animator>();
    private List<Animator> rArrowAnimators = new List<Animator>();

    private Vector2 lPosition;
    private Vector2 rPosition;

    void Start()
    {
        InitializeArrows();
    }
    private void InitializeArrows()
    {
        foreach (var button in buttons)
        {
            RectTransform lArrow = Instantiate(lArrrow, button.parent);
            RectTransform rArrow = Instantiate(rArrrow, button.parent);
            
            Animator lAnimator = lArrow.GetComponent<Animator>();
            Animator rAnimator = rArrow.GetComponent<Animator>();

            lArrowAnimators.Add(lAnimator);
            rArrowAnimators.Add(rAnimator);

            lArrow.localScale = new Vector3(-1, 1, 1);

        }
    }
    private void AssignButtonEvents()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            Button button = buttons[i].GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnButtonSelected(index));
            }
        }
    }

    private void OnButtonSelected(int index)
    {
        PlayArrowAnimation(lArrowAnimators[index]);
        PlayArrowAnimation(rArrowAnimators[index]);
    }

    private void PlayArrowAnimation(Animator arrowAnimator)
    {
        if (arrowAnimator != null)
        {
            arrowAnimator.SetTrigger("Highlighted");
        }
    }
    private void UpdateButtonArrows(RectTransform button, RectTransform lArrow, RectTransform rArrow)
    {
        float buttonWidth = button.sizeDelta.x;

        Vector2 lPosition = new Vector2(-buttonWidth / 2 - 50, button.anchoredPosition.y);
        Vector2 rPosition = new Vector2(buttonWidth / 2 + 50, button.anchoredPosition.y);

        StopAllCoroutines();
        StartCoroutine(AniPosition(lArrow, lPosition));
        StartCoroutine(AniPosition(rArrow, rPosition));
    }

    private IEnumerator AniPosition(RectTransform arrow, Vector2 target)
    {
        Vector2 aniPosit = arrow.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < aniDuration)
        {
            elapsedTime += Time.deltaTime;
            arrow.anchoredPosition = Vector2.Lerp(aniPosit, target, elapsedTime / aniDuration);
            yield return null;
        }

        arrow.anchoredPosition = target;
    }
}
