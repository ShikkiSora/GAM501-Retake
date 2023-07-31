using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public AudioSource CT;
    public float volume = 0.5f;
    public float maxvolume = 1.0f;
    public float minvolume = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        CT = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CT.volume = volume;
        vcontrol();
    }
    public void vcontrol()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            volume -= 0.1f;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            volume += 0.1f;
        }
        if(volume >= maxvolume)
        {
            volume = 1.0f;
        }
        if (volume <= minvolume)
        {
            volume = 0.0f;
        }
    }
}
