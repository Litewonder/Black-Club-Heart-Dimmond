using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ForUI : MonoBehaviour
{
    public GameObject PauseMenu,PauseButton;
    public AudioMixer audioMixer;
    public void Playgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Playagain()
    {
        SceneManager.LoadScene(0);
    }

    public void Exitgame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pausegame()
    {
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
        SoundManage.soundmanage.PauseMusic ();
        Playercontrol.Instance.isPause = true;
        SoundManage.soundmanage.isPause = true;
        Time.timeScale = 0f;
    }

    public void Setting()
    {
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
        SoundManage.soundmanage.isPause = true;
    }

    public void Resumegame1()
    {
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
        SoundManage.soundmanage.isPause = false;
    }

    public void Resumegame()
    {
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
        SoundManage.soundmanage.ResumeMusic ();
        Playercontrol.Instance.isPause = false;
        SoundManage.soundmanage.isPause = false;
        Time.timeScale = 1f;
    }

    public void Backtitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void BgmVolume( float volume)
    {
        audioMixer.SetFloat("Bgm", volume);
    }
    public void EffectVolume(float volume)
    {
        audioMixer.SetFloat("Effect", volume);
    }

}
