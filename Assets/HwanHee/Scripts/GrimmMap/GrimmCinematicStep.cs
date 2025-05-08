
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrimmCinematicStep
{
    protected GrimmIntroController controller;
    protected CinematicStep cinematicStep;
    protected float startDelay;

    public GrimmCinematicStep(GrimmIntroController _controller, CinematicStep _cinematicStep)
    {
        controller = _controller;
        cinematicStep = _cinematicStep;
    }
    public virtual void Enter() { }

    public virtual void NextStep()
    {
        cinematicStep += 1;
    }

    public virtual void Exit() { }
}

public class GrimmIntroStep1 : GrimmCinematicStep
{
    public GrimmIntroStep1(GrimmIntroController _controller) : base(_controller, CinematicStep.Intro1)
    {
        startDelay = controller.intro1StartDelay;
    }

    public override void Enter()
    {
        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.shakeAmplitude1, controller.shakeFrequency1, controller.introCameraShake1Duration);

        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio1);

        controller.particleEffect.Play();

        controller.grimmIntroLightSP1.gameObject.SetActive(true);
        controller.grimmIntroLightSP1.StartSpriteFade(controller.fadeIntroEffect1Duration, 0f, 1f);
        controller.grimmIntroLight1.gameObject.SetActive(true);
        controller.grimmIntroLight1.StartLightFade(controller.fadeIntroEffect1Duration, 0f, 1.5f);

        NextStep();
    }

    public override void NextStep()
    {
        base.NextStep();
        controller.ChangeStep(cinematicStep, controller.intro2StartDelay);
        Exit();
    }

    public override void Exit()
    {
        controller.particleEffect.Clear();
    }
}

public class GrimmIntroStep2 : GrimmCinematicStep
{
    public GrimmIntroStep2(GrimmIntroController _controller) : base(_controller, CinematicStep.Intro2)
    {
        startDelay = controller.intro2StartDelay;
    }

    public override void Enter()
    {
        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.shakeAmplitude2, controller.shakeFrequency2, controller.introCameraShake2Duration);

        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio2);

        var emission = controller.particleEffect.emission;
        emission.rateOverTime = controller.particleEmission;

        controller.particleEffect.Play();

        controller.grimmIntroLightSP2.gameObject.SetActive(true);
        controller.grimmIntroLightSP2.StartSpriteFade(controller.fadeIntroEffect2Duration, 0f, 1f);
        controller.grimmIntroLight2.gameObject.SetActive(true);
        controller.grimmIntroLight2.StartLightFade(controller.fadeIntroEffect2Duration, 0f, 1.5f);

        NextStep();
    }

    public override void NextStep()
    {
        base.NextStep();
        controller.ChangeStep(cinematicStep, controller.intro3StartDelay);
        Exit();
    }

    public override void Exit()
    {
        controller.particleEffect.Clear();
    }
}

public class GrimmIntroStep3 : GrimmCinematicStep
{
    public GrimmIntroStep3(GrimmIntroController _controller) : base(_controller, CinematicStep.Intro3)
    {
        startDelay = controller.intro3StartDelay;
    }

    public override void Enter()
    {
        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio3);
        controller.burstAudioLoop.GetComponent<AudioSource>().Play();

        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.shakeAmplitude3, controller.shakeFrequency3, controller.introCameraShake3Duration);
        controller.FVX.SetActive(true);
        controller.grimmSilhouette.GrimmSilhouetteStartGrow(controller.grimmSilhouetteActiveTime, controller.grimmSilhouetteGrowSpeed, controller.grimmSilhouetteOverGrowSpeed);
    }
}

public class FinalGrimmIntro_UI : GrimmCinematicStep
{
    public FinalGrimmIntro_UI(GrimmIntroController _controller) : base(_controller, CinematicStep.FinalIntro_UI)
    {
        startDelay = controller.finalIntroEffectDurtaion;
    }

    public override void Enter()
    {
        ShowGrimmUI();
    }

    private void ShowGrimmUI()
    {
        controller.grimmTextCanvas.gameObject.SetActive(true);

        controller.burstAudioLoop.GetComponent<AudioSource>().Stop();

        SoundManager.Instance.audioSource.PlayOneShot(controller.bossBusrtAudio);
        BGMManager.instance.SetBGM(controller.bossBGM2, 0f, 1f);
        BGMManager.instance.ChangeBGM(0f, false);

        controller.CallIntroFinish();
    }
}

public class GrimmHalfHpStep : GrimmCinematicStep
{
    public GrimmHalfHpStep(GrimmIntroController _controller) : base(_controller, CinematicStep.HalfHP)
    {
    }

    public override void Enter()
    {
        foreach (var eye in controller.eyes)
        {
            eye.gameObject.SetActive(true);
            eye.StartSpriteFade(controller.fadeEyesDuration);
            eye.StartLightFade(controller.fadeEyesDuration, 0f, controller.introEyesLightIntensity);
        }

        foreach (var grimmPulsate in controller.grimmPulsates)
        {
            grimmPulsate.SetPulsateFaster(controller.newPulseValue, controller.newPulseDalyTime);
        }
    }
}

public class GrimmOutroStep1 : GrimmCinematicStep
{
    public GrimmOutroStep1(GrimmIntroController _controller) : base(_controller, CinematicStep.Outro1)
    {
    }

    public override void Enter()
    {
        // 나중에 지울거 -> 보스 죽을 때 실행
        BGMManager.instance.BGMFadeOut();

        controller.isOutro = true;
        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.outroShake1Amplitude, controller.outroShake1Frequency3, 999f);
        controller.StartCameraMove();

        NextStep();
    }

    public override void NextStep()
    {
        base.NextStep();
        controller.ChangeStep(cinematicStep, controller.outro2StartDelay);
        Exit();
    }

    public override void Exit()
    {
    }
}

public class GrimmOutroStep2 : GrimmCinematicStep
{
    public GrimmOutroStep2(GrimmIntroController _controller) : base(_controller, CinematicStep.Outro2)
    {
    }

    public override void Enter()
    {
        controller.fadeSprite.StartFadeInOut(controller.outro1FadeSpriteDuration, controller.outro1FadeSpriteAlpha);

        foreach (var eye in controller.eyes)
        {
            eye.StartLightFade(0f, controller.introEyesLightIntensity, controller.outroEyesLightIntensity);
        }

        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio2);

        NextStep();
    }

    public override void NextStep()
    {
        base.NextStep();
        controller.ChangeStep(cinematicStep, controller.outro3StartDelay);
        Exit();
    }

    public override void Exit()
    {
    }
}

public class GrimmOutroStep3 : GrimmCinematicStep
{
    public GrimmOutroStep3(GrimmIntroController _controller) : base(_controller, CinematicStep.Outro3)
    {
    }

    public override void Enter()
    {
        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.outro3Shake3Amplitude, controller.outro3ShakeFrequency, 999f);

        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio3);
        controller.burstAudioLoop.GetComponent<AudioSource>().Play();
        controller.outroFVX.SetActive(true);

        controller.grimmSilhouette.gameObject.SetActive(true);
        controller.grimmSilhouette.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        controller.grimmSilhouette.gameObject.transform.position = new Vector3(-47.49725f, 5.334857f);
        controller.grimmSilhouette.GetComponent<SpriteRenderer>().color = new Color32(140, 0, 0, 0);
        controller.grimmSilhouette.StartSpriteFade(controller.outroSilhouetteFadeInDuration, 0f, 1f);

        NextStep();
    }

    public override void NextStep()
    {
        base.NextStep();
        controller.ChangeStep(cinematicStep, controller.outro4StartDelay);
        Exit();
    }

    public override void Exit()
    {
    }
}

public class GrimmOutroStep4 : GrimmCinematicStep
{
    public GrimmOutroStep4(GrimmIntroController _controller) : base(_controller, CinematicStep.Outro4)
    {
    }

    public override void Enter()
    {
        controller.outroFadeSprite.StartSpriteFade(controller.outroFadeSpriteDuration, 0f, 1f);

        NextStep();
    }

    public override void NextStep()
    {
        base.NextStep();
        controller.ChangeStep(cinematicStep, controller.finalOutroStartDelay);
        Exit();
    }

    public override void Exit()
    {
    }
}

public class FinalGrimmOutroStep : GrimmCinematicStep
{
    public FinalGrimmOutroStep(GrimmIntroController _controller) : base(_controller, CinematicStep.FinalOutro)
    {
    }

    public override void Enter()
    {
        controller.burstAudioLoop.StartBGMFade(controller.burstAudioLoopFadeOutDuration, 1f, 0f);
        controller.cinemachineCamera.GetComponent<CameraShake>().ForceStopShake();
        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio3);

        controller.grimmSilhouette.StartSpriteFade(controller.outroSilhouetteFadeOutDuration, 1f, 0f);
        controller.CreateGrimmSilhouetteParticle();
        foreach (var grimmPulsate in controller.grimmPulsates)
        {
            grimmPulsate.StopPulsate();
        }
        NextStep();
    }

    public override void NextStep()
    {
        base.NextStep();
        controller.ChangeStep(cinematicStep, controller.devRoomLoadDelay);
    }
}

public class LoadDeveloperRoom : GrimmCinematicStep
{
    public LoadDeveloperRoom(GrimmIntroController _controller) : base(_controller, CinematicStep.LoadDevRoom)
    {
    }

    public override void Enter()
    {
        BGMManager.instance.SetBGM(controller.devRoomBGM, 0f, 1f);
        BGMManager.instance.ChangeBGM(0f, true);

        SceneManager.LoadScene("DeveloperRoom");
    }
}