using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public static MusicScript instance;

    [SerializeField] private AudioSource song1;
    [SerializeField] private AudioSource song2;
    [SerializeField] private AudioSource song3;

    private void Awake()
    {
        instance = this;
    }


    public void PlayRandomMusic()
    {
        song1.volume = 0;
        song2.volume = 0;
        song3.volume = 0;

        int randomSong;
        randomSong = Random.Range(1, 4);

        if (randomSong == 1)
            song1.volume = 0.1f;

        if (randomSong == 2)
            song2.volume = 0.15f;

        if (randomSong == 3)
            song3.volume = 0.1f;
    }
}
