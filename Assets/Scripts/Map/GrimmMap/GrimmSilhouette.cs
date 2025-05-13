using System.Collections;
using UnityEngine;

public class GrimmSilhouette : FadeObject
{
    private float grimmSilhouetteGrowSpeed;
    private float grimmSilhouetteOverGrowSpeed;

    [SerializeField] GrimmIntroController controller;

    public void GrimmSilhouetteStartGrow(float grimmSilhouetteActiveTime, float _grimmSilhouetteGrowSpeed, float _grimmSilhouetteOverGrowSpeed)
    {
        grimmSilhouetteGrowSpeed = _grimmSilhouetteGrowSpeed;
        grimmSilhouetteOverGrowSpeed = _grimmSilhouetteOverGrowSpeed;

        Invoke("StartGrow", grimmSilhouetteActiveTime);
    }

    private void StartGrow()
    {
        gameObject.SetActive(true);
        StartCoroutine(SetGrimmSilhouetteScale());

    }

    private IEnumerator SetGrimmSilhouetteScale()
    {
        Vector3 originScale = transform.localScale;

        float elapsed = 0f;
        while (elapsed <= grimmSilhouetteGrowSpeed)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originScale, new Vector3(0.7f, 0.7f, 0.7f), elapsed / grimmSilhouetteGrowSpeed);
            yield return null;
        }
        StartCoroutine(SetGrimmSilhouetteScaleBigger());
    }

    private IEnumerator SetGrimmSilhouetteScaleBigger()
    {
        Vector3 originScale = transform.localScale;
        Vector3 originPos = transform.position;

        float elapsed = 0f;
        while (elapsed <= grimmSilhouetteOverGrowSpeed)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originScale, new Vector3(60f, 60f, 60f), elapsed / grimmSilhouetteOverGrowSpeed);
            transform.position = Vector3.Lerp(originPos, originPos + new Vector3(0f, -13f, 0f), elapsed / grimmSilhouetteOverGrowSpeed);
            yield return null;
        }

        controller.ChangeStep(CinematicStep.FinalIntro_UI);
    }
}
