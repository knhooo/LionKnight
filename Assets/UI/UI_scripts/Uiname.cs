using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UIname
{
    #region Base.fadeUI
    public class fadeUI : MonoBehaviour
    {
        public bool isActivated = true;
        
        private Image imageComponent;
        private Text textComponent;
        private CanvasGroup canvasGroup;

        public float firstTime = 3f;
        public float waitTime = 3f;

        void Start()
        {
            imageComponent = GetComponent<Image>();
            textComponent = GetComponentInChildren<Text>();
            canvasGroup = GetComponent<CanvasGroup>();
            
            if(isActivated == true)
            {
                canvasGroup.alpha = 0f;
                StartCoroutine(SceneLoadDelay());
            }
        }
        IEnumerator SceneLoadDelay()
        {
            yield return new WaitForSeconds(firstTime);
            StartCoroutine(FadeInOut());
        }
        IEnumerator FadeInOut()
        {
            float duration = 1f;
            for (float t = 0f; t <= 1f; t += Time.deltaTime / duration)
            {
                canvasGroup.alpha = t;
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            for (float t = 1f; t >= 0f; t -= Time.deltaTime / duration)
            {
                canvasGroup.alpha = t;
                yield return null;
            }
        }
    }
    #endregion

    #region Shop UI
    [System.Serializable]
    public class Achievement
    {
        public string achievementName;
        public bool isCompleted;
    }

    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int itemPrice;
        public GameObject itemPrefab;
    }
    #endregion

    #region HUD
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
        #endregion

        
        //public class buttonAni : MonoBehaviour
        //{
        //    [SerializeField] private GameObject prefab;
        //    [SerializeField] private GameObject[] btts;

        //    public RectTransform rct;
        //    public RectTransform rct2;

        //    public void Apply()
        //    {
        //        foreach (GameObject btt in btts)
        //        {
        //            Button button = btt.GetComponent<Button>();
        //            button.onClick.AddListener(() => bttOnClick(btt));
        //        }
        //    }
        //    public void bttOnClick(GameObject btt)
        //    {
        //        Instantiate(prefab, rct.position, Quaternion.identity);
        //        Instantiate(prefab, rct2.position, Quaternion.identity);

        //        Vector2 bttPosition = btt.transform.position;

        //        rct.position = new Vector2(bttPosition.x / 2 + 50, bttPosition.y);
        //        rct2.position = new Vector2(bttPosition.x / 2 - 50, bttPosition.y);

        //        Vector2 rctFlip = rct2.localScale;
        //        rctFlip.x = -1;
        //        rct2.localScale = rctFlip;

    }
}
