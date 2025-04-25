using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GrimmMapCameraController : MonoBehaviour
{
    [SerializeField] private Collider2D bossStartTrigger;
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private BoxCollider2D bossBoundingShape;
    [SerializeField] private Vector2 CameraPos1;
    [SerializeField] private Vector2 CameraPos2;
    [SerializeField] private Vector2 CameraPos3;
    [SerializeField] private FadeSprite fadeSprite;

    Player player;

    private void Awake()
    {
        // player = PlayerManager.instance.player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == bossStartTrigger.transform)
        {
            CameraMove();
            StartCoroutine(fadeSprite.StartFadeOut());
        }
    }



    private void CameraMove()
    {
    }

    private void SwitchConfiner() => confiner.BoundingShape2D = bossBoundingShape;
}
