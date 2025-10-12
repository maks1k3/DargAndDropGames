using UnityEngine;

public class ButtonSong : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public AudioSource audioSource;
    public AudioSource audioSource2;


    void Start()
    {
        if (audioSource2 != null)
        {
            audioSource2.loop = true;
            audioSource2.Play();
            Debug.Log("sound");
        }
    }

    public void playSound()
    {
        audioSource.Play();
        Debug.Log("sound");
    }
    public void playSound2()
    {
        audioSource2.Play();
        Debug.Log("sound2");
    }
}
