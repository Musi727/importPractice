using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;

public class MusicMgr : BaseManager<MusicMgr>
{
    //“Ù¿÷ ∫Õ “Ù–ß
    public AudioSource BkMusic;
    private float bkValue = 0.5f;

    public List<AudioSource> soundList = new List<AudioSource>();
    public GameObject soundObj;
    public float soundValue =0.5f;

    //‘⁄Update¿Ô≈–∂œ“Ù–ß «∑Ò≤•ÕÍ£¨≤•ÕÍæÕ“∆≥˝µÙ
    public MusicMgr()
    {
        MonoMgr.Instance.AddUpdateListener(Update);
    }
    private void Update()
    {
        for( int i = soundList.Count - 1; i >=0; --i )
        {
            if(!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    #region “Ù¿÷

    /// <summary>
    /// ≤•∑≈“Ù¿÷
    /// </summary>
    public void PlayBkMusic(string name)
    {
        if(BkMusic == null)
        {
            GameObject obj = new GameObject("BkMusic");
            BkMusic = obj.AddComponent<AudioSource>();
        }


        ResMgr.Instance.LoadResourcesAsync<AudioClip>("Music/BK/"+name, (clip) =>
        {
            BkMusic.clip = clip;
            BkMusic.volume = bkValue;
            BkMusic.loop = true;
            BkMusic.Play();
        });
    }

    /// <summary>
    /// ‘›Õ£“Ù¿÷
    /// </summary>
    public void PauseBkMusic()
    {
        if (BkMusic == null)
            return;
        BkMusic.Pause();
    }
    
    public void ChangeBkValue(float value)
    {
        bkValue = value;
        if (BkMusic == null)
            return;
        BkMusic.volume = value;
    }

    /// <summary>
    /// Õ£÷π“Ù¿÷
    /// </summary>
    public void StopBkMusic()
    {
        if (BkMusic == null)
            return;
        BkMusic.Stop();
    }
    #endregion

    #region  “Ù–ß
    public void PlaySound(string name, bool isLoop = false, UnityAction<AudioSource> callback = null)
    {
        if(soundObj == null)
        {
            soundObj = new GameObject("Sound");
        }
        ResMgr.Instance.LoadResourcesAsync<AudioClip>("Music/Sound/" + name, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            soundList.Add(source);
            if(callback != null) 
                callback(source);
        });
    }

    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = soundValue;
        }
    }


    public void StopSound(AudioSource source)
    {
        //±È¿˙ “Ù–ß»›∆˜
        for(int i=soundList.Count-1;i>=0; i--)
        {
            if(soundList[i] == source)
            {
                soundList.Remove(source);
                soundList[i].Stop();
                GameObject.Destroy(source);
            }
        }
    }


    #endregion
}
