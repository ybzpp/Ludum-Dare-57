using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    private void Awake()
    {
        var audio = GetComponent<AudioSource>();

        if (audio != null )
            audio.PlayDelayed(Random.Range(0f, audio.clip.length));   
    }
}
