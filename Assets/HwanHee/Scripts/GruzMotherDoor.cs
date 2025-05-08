using System.Collections;
using UnityEngine;

public class GruzMotherDoor : MonoBehaviour
{
    [SerializeField] AudioClip doorAudio;
    [SerializeField] Transform doorClosePoint;
    [SerializeField] Transform doorOpenPoint;
    [SerializeField] GameObject doorCloseEffect;
    [SerializeField] GameObject[] doorOpenEffect;

    [SerializeField] float doorSpeed = 5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Vector2.Distance(transform.position, doorClosePoint.position) < 0.1f)
            {
                StartOpenDoor();
            }
            else
            {
                StartCloseDoor();
            }
        }
    }

    public void StartCloseDoor()
    {
        StartCoroutine(CloseDoor());
    }

    public void StartOpenDoor()
    {
        SoundManager.Instance.audioSource.PlayOneShot(doorAudio);
        for (int i = 0; i < 2; i++)
        {
            doorOpenEffect[i].SetActive(true);
        }

        StartCoroutine(OpenDoor());
    }

    private IEnumerator CloseDoor()
    {
        while (Vector2.Distance(transform.position, doorClosePoint.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, doorClosePoint.position, Time.deltaTime * doorSpeed);
            yield return null;
        }
        SoundManager.Instance.audioSource.PlayOneShot(doorAudio);
        doorCloseEffect.gameObject.SetActive(true);
    }

    private IEnumerator OpenDoor()
    {
        while (Vector2.Distance(transform.position, doorOpenPoint.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, doorOpenPoint.position, Time.deltaTime * doorSpeed);
            yield return null;
        }
    }
}
