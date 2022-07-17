using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    [Space(10f)] [SerializeField] private AudioClip[] _musicClips;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => !_musicSource.isPlaying);
            _musicSource.clip = _musicClips.GetRandomObject();
            _musicSource.Play();
        }
    }

    public static void PlaySound(AudioClip clip, float pitchRandomRange = 0, bool playExclusivly = false, float volume = 1)
    {
        AudioSource source;
        var destroySource = false;
        if (_instance._soundSource.isPlaying && !playExclusivly)
        {
            source = Instantiate(_instance._soundSource, _instance._soundSource.transform.parent);
            destroySource = true;
        }
        else
        {
            source = _instance._soundSource;
        }
        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.pitch = Random.Range(1 - pitchRandomRange, 1 + pitchRandomRange);
        source.Play();
        if(destroySource)
            _instance.StartCoroutine(DestroySourceEnumerator());

        IEnumerator DestroySourceEnumerator()
        {
            yield return null;
            yield return new WaitUntil(() => !source.isPlaying);
            Destroy(source.gameObject);
        }
    }
}
