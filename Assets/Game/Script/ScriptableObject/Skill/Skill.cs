using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public SkillData skillData; //these are attributes of the Skill class, however they are also seen as properties as their access modifiers are public and can be get and set publicly
    public Character attacker;

    //public AudioSource audioSource;

    //void Start()
    //{
    //    // Check if the audio source is already playing
    //    if (!audioSource.isPlaying)
    //    {
    //        // Add a random delay before playing the sound
    //        float randomDelay = Random.Range(0.0f, 1.0f);
    //        Invoke("PlaySound", randomDelay);
    //    }
    //}

    //void PlaySound()
    //{
    //    // Play the sound
    //    audioSource.Play();
    //}
}
