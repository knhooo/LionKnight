using UnityEngine;
using UnityEngine.Rendering;

public class Stag : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private AudioClip stagStartSound;
    [SerializeField] private AudioClip stagArriveSound;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartStag()
    {
        SoundManager.Instance.audioSource.PlayOneShot(stagStartSound);
        Invoke("PlayAnimation", 0.7f);
    }

    private void PlayAnimation()
    {
        anim.SetBool("isBellRing", true);
    }

    private void StartStagSoundAnimationTrigger()
    {
        SoundManager.Instance.audioSource.PlayOneShot(stagArriveSound);
    }

    private void StartLoadSceneAnimationTrigger()
    {
        sceneLoader.StartLoadScene();
    }
}
