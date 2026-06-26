using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioSource snd;
    [SerializeField] private AudioClip sndClip;

    private void OnEnable()
    {
        if(snd != null)
        {
            PlayAudio();
        }
    }

    public void PlayAudio()
    {
        snd.PlayOneShot(sndClip);
    }

}
