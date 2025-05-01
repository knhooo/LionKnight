
public class GrimmIntroStep
{
    protected GrimmIntroController controller;
    public GrimmIntroStep(GrimmIntroController _controller)
    {
        controller = _controller;
    }
    public virtual void Enter() { }
    public virtual void NextStep() { }
}

public class GrimmIntroStep1 : GrimmIntroStep
{
    public GrimmIntroStep1(GrimmIntroController _controller) : base(_controller)
    {
    }

    public override void Enter()
    {
        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.shakeAmplitude1, controller.shakeFrequency1, controller.effect1Duration);

        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio1);

        controller.particleSystemPlay.Play();
        controller.grimmIntroLight1.gameObject.SetActive(true);
        controller.grimmIntroLight1.StartFadeInOut(controller.grimmLight1FadeDuration, 0f, 1f);

        NextStep();
    }
    public override void NextStep()
    {
        controller.ChangeStep(2, controller.effect2StartDelay);
    }
}

public class GrimmIntroStep2 : GrimmIntroStep
{
    public GrimmIntroStep2(GrimmIntroController _controller) : base(_controller)
    {
    }

    public override void Enter()
    {
        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.shakeAmplitude2, controller.shakeFrequency2, controller.effect2Durtaion);

        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio2);

        controller.particleSystemPlay.Stop();
        var emission = controller.particleSystemPlay.emission;
        emission.rateOverTime = controller.particleEmission;

        controller.particleSystemPlay.Stop();
        controller.particleSystemPlay.Play();

        controller.grimmIntroLight2.gameObject.SetActive(true);
        controller.grimmIntroLight2.StartFadeInOut(controller.grimmLight1FadeDuration, 0f, 1f);
        NextStep();
    }

    public override void NextStep()
    {
        controller.ChangeStep(3, controller.effect3StartDelay);
    }
}

public class GrimmIntroStep3 : GrimmIntroStep
{
    public GrimmIntroStep3(GrimmIntroController _controller) : base(_controller)
    {
    }

    public override void Enter()
    {
        SoundManager.Instance.audioSource.PlayOneShot(controller.burstAudio3);
        controller.burstAudioLoop.Play();

        controller.cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(controller.shakeAmplitude3, controller.shakeFrequency3, controller.effect3Durtaion);
        controller.FVX.SetActive(true);
        controller.ActiveGrimmSilhouette();
    }
}