using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBGM;
    private int bgmIndex;
    public bool canPlaySFX;
    private void Awake()
    {

        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 1f);
    }

    private void Update()
    {
        if (!playBGM)
            StopAllBgm();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlaySfx(int _sfxIndex, Transform _source)

    {
        if (canPlaySFX == false)
            return;

        //if (sfx[_sfxIndex].isPlaying) aynı anda 2 ses çalması için
        //  return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }


    }

    public void StopSfx(int _Index) => sfx[_Index].Stop();

    public void StopSFXWithTime(int _Index)
    {
        StartCoroutine(DecreaseVolume(sfx[_Index]));
    }

    public void StopBGMWithTime(int _Index)
    {
        StartCoroutine(DecreaseVolume(sfx[_Index]));
    }

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume>.1f) 
        {
            _audio.volume -= _audio.volume * .2f;
         yield return new WaitForSeconds(.6f); // Sesi daha hızlı azaltmak için
            
         
            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayBGM(int _BGMIndex)
    {
        bgmIndex = _BGMIndex;

        StopAllBgm();
        bgm[_BGMIndex].Play();
    }

    public void StopAllBgm()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
    private void AllowSFX() => canPlaySFX = true;

}
