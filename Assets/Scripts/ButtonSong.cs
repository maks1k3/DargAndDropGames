using UnityEngine;

public class ButtonSong : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource audioSource;
    public void playSound()
    {
        audioSource.Play();
        Debug.Log("sound");
    }
}
