
using UnityEditor.UI;
using UnityEngine.XR;

public class GrimmCinematicStep
{
    protected GrimmIntroController controller;
    protected CinematicStep cinematicStep;
    protected float startDelay;

    public GrimmCinematicStep(GrimmIntroController _controller)
    {
        controller = _controller;
    }
    public virtual void Enter() { }

    public virtual void NextStep()
    {
        cinematicStep += 1;
        controller.ChangeStep(cinematicStep, controller.intro2StartDelay);
        Exit();
    }

    public virtual void Exit() { }
}

public class GrimmIntroStep1 : GrimmCinematicStep
{
    public GrimmIntroStep1(GrimmIntroController _controller) : base(_controller)
    {
        cinematicStep = CinematicStep.Intro1;
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

    public override void Exit()
    {
        controller.particleEffect.Clear();
    }
}

public class GrimmIntroStep2 : GrimmCinematicStep
{
    public GrimmIntroStep2(GrimmIntroController _controller) : base(_controller)
    {
        cinematicStep = CinematicStep.Intro2;
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

    public override void Exit()
    {
        controller.particleEffect.Clear();
    }
}

public class GrimmFinalIntroStep : GrimmCinematicStep
{
    public GrimmFinalIntroStep(GrimmIntroController _controller) : base(_controller)
    {
        cinematicStep = CinematicStep.FinalIntro;
        startDelay = controller.intro3StartDelay;
    }

    public override void Enter()
    {
        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio3);
        controller.burstAudioLoop.Play();

        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.shakeAmplitude3, controller.shakeFrequency3, controller.introCameraShake3Duration);
        controller.FVX.SetActive(true);
        controller.ActiveGrimmSilhouette();
    }
}

public class GrimmHalfHpStep : GrimmCinematicStep
{
    public GrimmHalfHpStep(GrimmIntroController _controller) : base(_controller)
    {
        cinematicStep = CinematicStep.HalfHP;
    }

    public override void Enter()
    {
        foreach (var eye in controller.eyes)
        {
            eye.gameObject.SetActive(true);
            eye.StartSpriteFade(controller.fadeEyesDuration);
            eye.StartLightFade(controller.fadeEyesDuration);
        }

        foreach(var grimmPulsate in controller.grimmPulsates)
        {
            grimmPulsate.SetPulssateFaster(controller.newPulseValue, controller.newPulseDalyTime);
        }
    }
}