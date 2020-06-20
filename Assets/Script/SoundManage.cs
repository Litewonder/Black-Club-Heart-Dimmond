using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
    public static SoundManage soundmanage;

    public AudioSource audioSorce, background;
    public AudioClip[] audioGroup;

    private int playingIndex;
    private bool canPlayAudio;

    public bool isPause;


    void Start()
    {
        canPlayAudio = true;

        playingIndex = (int)(UnityEngine .Random .Range (5f,9f));
    }


    void Update()
    {
        if (canPlayAudio)
        {
            PlayAudio();

            canPlayAudio = false;
        }

        if (!background.isPlaying && !isPause)
        {
            playingIndex++;

            if (playingIndex >= audioGroup.Length)
            {
                playingIndex = 5;
            }

            canPlayAudio = true;
        }
    }

 

    private void PlayAudio()
    {
        background.clip = audioGroup[playingIndex];
        background.Play();
    }


    private void Awake()
    {
        soundmanage = this;
    }
    

    public void JumpAudio()
    {
        audioSorce.clip = audioGroup[0];
        audioSorce.Play();
    }

    public void HurtAudio()
    {
        audioSorce.clip = audioGroup[1];
        audioSorce.Play();
    }

    public void CollectAudio()
    {
        audioSorce.clip = audioGroup[2];
        audioSorce.Play();
    }

    public void OpendoorAudio()
    {
        audioSorce.clip = audioGroup[3];
        audioSorce.Play();
    }

    public void DestroyAudio()
    {
        audioSorce.clip = audioGroup[4];
        audioSorce.Play();
    }

    public void PauseMusic()
    {
        audioSorce.Pause();
        background.Pause();
    }

    public void ResumeMusic()
    {
        audioSorce.Play();
        background.Play();
    }
}
