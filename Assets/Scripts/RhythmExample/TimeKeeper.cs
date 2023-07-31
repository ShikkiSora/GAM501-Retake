using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeKeeper : MonoBehaviour
{

    public bool metronomeOn = false;
    public AudioClip metronome;

    [Header("Inputs")]
    public AudioSource audioSource;
    public float BPM;
    public float startDelay;
    public int beatsPerBar;

    public float secondsPerBeat;

    [Header("Outputs")]
    public float songPosition;
    public int currentBeat;
    public int currentBar;
    public float beatsAnalogue;
    public int beatsDiscrete;

    [Header("Debug (DELETE)")]
    public TextMeshProUGUI barOutputText;
    public TextMeshProUGUI beatOutputText;

    // Start is called before the first frame update
    void Start()
    {
        secondsPerBeat = 60f / BPM;
        audioSource.Play();

    }

    public void StartMusic()
    {
        audioSource.Play();
        if (audioSource.time == 0f)
        {
            // Initialize variables for a restart
            if (metronomeOn) PlayMetronome();
            beatsDiscrete = 0;
        }
    }

    // Will automatically stop
    public void Restart()
    {
        // These will not update automatically
        beatsDiscrete = 0;
        currentBar = 0;

    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = audioSource.time - startDelay;
        beatsAnalogue = (songPosition - startDelay) / secondsPerBeat; // To make beats start with zero

        if ((int) Mathf.Floor(beatsAnalogue) > beatsDiscrete)
        {
            beatsDiscrete++;
            if (metronomeOn) PlayMetronome();
        }

        if ((currentBar + 1) * beatsPerBar * secondsPerBeat < songPosition - startDelay)
        {
            currentBar++;
        }

        //beatOutputText.text = Mathf.Floor(beatsAnalogue).ToString();
        //barOutputText.text = currentBar.ToString();

    }

    void PlayMetronome()
    {
        audioSource.PlayOneShot(metronome);
    }

}
