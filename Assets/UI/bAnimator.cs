using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bAnimator : MonoBehaviour
{
    public GameObject bArrow;
    private GameObject leftA;
    private GameObject rightA;
    private Button btt;
    private RectTransform bttRect;

    void Start()
    {
        btt = GetComponent<Button>();
        bttRect = GetComponent<RectTransform>();

        btt.onClick.AddListener(OnButtonSelected);


        InitializeArrows();
    }
    private void InitializeArrows()
    {

        float buttonWidth = bttRect.rect.width;

        Vector2 leftPosition = new Vector2(-buttonWidth / 2 - 50, 0);
        Vector2 rightPosition = new Vector2(buttonWidth / 2 + 50, 0);

        leftA = Instantiate(bArrow, leftPosition, Quaternion.identity);
        rightA = Instantiate(bArrow, rightPosition, Quaternion.identity);

    }
    private void OnButtonSelected()
    {
        Instantiate(bArrow, transform.position, Quaternion.identity);
    }
}
