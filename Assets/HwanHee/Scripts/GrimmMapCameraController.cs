using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Splines.Interpolators;

public class GrimmMapCameraController : MonoBehaviour
{
    [SerializeField] private Collider2D bossStartTrigger;
    [SerializeField] private CinemachineConfiner2D cameraConfiner;
    [SerializeField] private BoxCollider2D bossBoundingShape;
    [SerializeField] private Transform[] CameraPos;
    [SerializeField] private float cameraMoveTime;
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
            transform.SetParent(null);
            StartCoroutine(CameraMove());
            StartCoroutine(fadeSprite.StartFadeOut());
        }
    }

    private IEnumerator CameraMove()
    {
        int i = 0;
        while (true)
        {
            float elapsed = 0f;
            Vector3 currentPos = CameraPos[i].transform.position;

            while (elapsed < cameraMoveTime)
            {
                transform.position = Vector3.Lerp(currentPos, CameraPos[i + 1].transform.position, elapsed / cameraMoveTime);

                elapsed += Time.deltaTime;

                yield return null;
            }
            currentPos = CameraPos[i].transform.position;

            i++;

            if (i == CameraPos.Length - 1)
                break;
        }
    }

    private void SwitchConfiner() => cameraConfiner.BoundingShape2D = bossBoundingShape;
}
