using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    [SerializeField] private AudioSource snd;
    [SerializeField] private AudioClip[] sndClips;

    private void OnEnable()
    {
        if(snd != null)
        {
            PlayAudio();
        }
    }

    public void PlayAudio()
    {
        AudioClip randomClip = sndClips[Random.Range(0, sndClips.Length)];
        snd.PlayOneShot(randomClip);
    }

}
