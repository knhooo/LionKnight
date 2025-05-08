using System.Collections;
using UnityEngine;

public class mapFadevfx : MonoBehaviour
{
    public static mapFadevfx Instance;

    [SerializeField] 
    private GameObject vfxFade;

    private void Awake()
    {
        Instance = this;
        vfxFade.SetActive(false);
    }
    public void vfxFadeIn(float time)
    {
        vfxFade.SetActive(true);
        vfxFade.GetComponent<Animator>().SetTrigger("FadeIn");
        StartCoroutine(vfxFadeOut(time));
    }
    private IEnumerator vfxFadeOut(float time)
    {
        yield return new WaitForSeconds(time);
        vfxFade.GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(time);
        vfxFade.SetActive(false);
    }
}
