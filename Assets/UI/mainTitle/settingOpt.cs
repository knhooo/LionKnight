using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.Video;

public class settingOpt : MonoBehaviour
{
    public static settingOpt Instance { get; private set; }

    public Toggle fullscreenToggle;
    public Toggle mute;
    public Slider volume;
    public Button saveButton;

    public VideoPlayer videoPlayer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        mute.onValueChanged.AddListener(delegate { Mute(); });
        fullscreenToggle.onValueChanged.AddListener(delegate { Fullscreen(); });
        volume.onValueChanged.AddListener(delegate { Volume(); });
        saveButton.onClick.AddListener(delegate { saveSetting(); });

        loadSetting();
    }

    public void Mute()
    {
        bool isMuted = mute.isOn;

        AudioListener.volume = isMuted ? 0 : volume.value;

        if (videoPlayer != null)
        {
            videoPlayer.SetDirectAudioMute(0, isMuted);
        }
    }
    public void Fullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void Volume()
    {
        if (!mute.isOn)
        {
            float volumeValue = volume.value;

            if (!mute.isOn)
            {
                AudioListener.volume = volumeValue;
            }

            if (videoPlayer != null)
            {
                videoPlayer.SetDirectAudioVolume(0, volumeValue);
            }
        }
    }

    public void saveSetting()
    {
        PlayerPrefs.SetInt("fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("volume", volume.value);
        PlayerPrefs.SetInt("mute", mute.isOn ? 1 : 0);
    }

    public void loadSetting()
    {
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") == 1;
        }
        if (PlayerPrefs.HasKey("volume"))
        {
            volume.value = PlayerPrefs.GetFloat("volume");
        }
        if (PlayerPrefs.HasKey("mute"))
        {
            mute.isOn = PlayerPrefs.GetInt("mute") == 1;
        }
        Mute();
        Volume();
    }
}
