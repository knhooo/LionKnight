using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine.Splines;
using NUnit.Framework.Constraints;

public class implementBox : MonoBehaviour
{
    [SerializeField] private GameObject go;
    public Text geotxt;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            go.gameObject.SetActive(true);
            Time.timeScale = 0f;

        }
    }
    public void GeoHUD(int money, int geo)
    {
        PlayerPrefs.SetInt("geoValue", money);
        PlayerPrefs.Save();

        geo = PlayerPrefs.GetInt("geoValue");
        geotxt.text = money.ToString();
    }

    public void HpHUD(int hp, float curHp, int maxHp)
    {
        PlayerPrefs.SetInt("hpValue", hp);
        PlayerPrefs.Save();

        hp = PlayerPrefs.GetInt("hpValue");

        curHp = (float)hp / maxHp;

        switch (curHp)
        {
            case > 0.75f:
                break;

            case > 0.5f and <= 0.75f:
                break;

            case > 0.25f and <= 0.5f:
                break;

            case > 0f and <= 0.25f:
                break;

            default:
                break;
        }
    }

    public void soulHUD(int soul, int mp, int maxMp)
    {
        PlayerPrefs.SetInt("soulValue", mp);
        PlayerPrefs.Save();

        mp = PlayerPrefs.GetInt("soulValue");
        soul = mp % maxMp;

        for (int t = 0; t < soul; t++)
        {
            if (t == 0)
            {
                return;
            }
        }

    }

    public class buttonAni : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject[] btts;

        public RectTransform rct;
        public RectTransform rct2;

        public void Apply()
        {
            foreach (GameObject btt in btts)
            {
                Button button = btt.GetComponent<Button>();
                button.onClick.AddListener(() => bttOnClick(btt));
            }
        }
        public void bttOnClick(GameObject btt)
        {
            Instantiate(prefab, rct.position, Quaternion.identity);
            Instantiate(prefab, rct2.position, Quaternion.identity);

            Vector2 bttPosition = btt.transform.position;

            rct.position = new Vector2(bttPosition.x / 2 + 50, bttPosition.y);
            rct2.position = new Vector2(bttPosition.x / 2 - 50, bttPosition.y);

            Vector2 rctFlip = rct2.localScale;
            rctFlip.x = -1;
            rct2.localScale = rctFlip;
        }
    }
}
