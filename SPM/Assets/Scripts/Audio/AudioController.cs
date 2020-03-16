using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {
    //Author: Patrik Ahlgren

    [Header("Player")]
    [SerializeField] private Sound[] playerSounds;
    [Header("Enemies")]
    [SerializeField] private Sound[] enemySounds;
    [Header("Environment")]
    [SerializeField] private Sound[] environmentSounds;
    [Header("Music")]
    [SerializeField] private Sound[] musicSounds;
    [Header("VoiceLines")]
    [SerializeField] private Sound[] voiceSounds;
    [Header("Sound Object")]
    [SerializeField] private GameObject soundObject;
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup voiceMixerGroup;

    private Dictionary<string, Sound> musicDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> sfxDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> voiceDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> allSoundsDictionary = new Dictionary<string, Sound>();

    private Dictionary<string, float> soundTimerDictonary = new Dictionary<string, float>();

    private bool continueFadeIn;
    private bool continueFadeOut;

    private Sound sound;

    private static AudioController _instance;

    public static AudioController Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<AudioController>();
#if UNITY_EDITOR
                if (FindObjectsOfType<AudioController>().Length > 1) {
                    Debug.LogError("There is more than one AudioController in the scene");
                }
#endif
            }
            return _instance;
        }
    }



    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound player_S in playerSounds) { allSoundsDictionary[player_S.name] = player_S; sfxDictionary[player_S.name] = player_S; }
        foreach (Sound enemy_S in enemySounds) { allSoundsDictionary[enemy_S.name] = enemy_S; sfxDictionary[enemy_S.name] = enemy_S; }
        foreach (Sound environment_S in environmentSounds) { allSoundsDictionary[environment_S.name] = environment_S; sfxDictionary[environment_S.name] = environment_S; }
        foreach (Sound music_S in musicSounds) { allSoundsDictionary[music_S.name] = music_S; musicDictionary[music_S.name] = music_S; }
        foreach (Sound voice_S in voiceSounds) { allSoundsDictionary[voice_S.name] = voice_S; voiceDictionary[voice_S.name] = voice_S; }



        foreach (KeyValuePair<string, Sound> s in allSoundsDictionary) {
            s.Value.source = gameObject.AddComponent<AudioSource>();
            s.Value.source.clip = s.Value.clip;
            s.Value.source.volume = s.Value.volume;
            s.Value.source.pitch = s.Value.pitch;
            s.Value.source.spatialBlend = s.Value.spatialBlend_2D_3D;
            s.Value.source.rolloffMode = (AudioRolloffMode)s.Value.rolloffMode;
            s.Value.source.minDistance = s.Value.minDistance;
            s.Value.source.maxDistance = s.Value.maxDistance;
            s.Value.source.loop = s.Value.loop;
            s.Value.source.outputAudioMixerGroup = masterMixerGroup;
        }

        foreach (KeyValuePair<string, Sound> s in sfxDictionary) {
            s.Value.source.outputAudioMixerGroup = sfxMixerGroup;
        }

        foreach (KeyValuePair<string, Sound> s in musicDictionary) {
            s.Value.source.outputAudioMixerGroup = musicMixerGroup;
        }

        foreach (KeyValuePair<string, Sound> s in voiceDictionary) {
            s.Value.source.outputAudioMixerGroup = voiceMixerGroup;
        }

    }

    #region Play/Stop Methods
    public void Play(string name) {       
        try {
            FindSound(name).source.Play();
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void PlaySFX(string name) {
        try {
            FindSFX(name).source.Play();
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void PlayMusic(string name) {
        try {
            FindMusic(name).source.Play();
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void PlayVoiceLine(string name) {
        try {
            FindVoiceLine(name).source.Play();
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void Play_InWorldspace(string name, GameObject gameObjectLocation) {
        try {
            if (allSoundsDictionary.ContainsKey(name)) {
                GameObject soundAtLocationGO = Instantiate(soundObject, gameObjectLocation.transform.position, Quaternion.identity, gameObjectLocation.transform);
                InWorldSpaceRoutine(name, soundAtLocationGO);
                sound.source.pitch = sound.pitch;
                sound.source.Play();
                if (!sound.source.loop) {
                    Destroy(soundAtLocationGO, sound.clip.length);
                }
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void Play_InWorldspace(string name, GameObject gameObjectLocation, float minPitch, float maxPitch) {
        try {
            if (allSoundsDictionary.ContainsKey(name)) {
                GameObject soundAtLocationGO = Instantiate(soundObject, gameObjectLocation.transform.position, Quaternion.identity, gameObjectLocation.transform);
                InWorldSpaceRoutine(name, soundAtLocationGO);
                sound.source.pitch = Random.Range(minPitch, maxPitch);
                sound.source.Play();
                if (!sound.source.loop) {
                    Destroy(soundAtLocationGO, sound.clip.length);
                }
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void Play_InWorldspace_WithTag(string name, string tag) {
        try {
            if (allSoundsDictionary.ContainsKey(name)) {
                GameObject[] gameObjectLocation = GameObject.FindGameObjectsWithTag(tag);
                foreach(GameObject taggedObject in gameObjectLocation) {
                    GameObject soundAtLocationGO = Instantiate(soundObject, taggedObject.transform.position, Quaternion.identity, taggedObject.transform);
                    InWorldSpaceRoutine(name, soundAtLocationGO);
                    sound.source.pitch = sound.pitch;
                    sound.source.Play();
                    if (!sound.source.loop) {
                        Destroy(soundAtLocationGO, sound.clip.length);
                    }
                }               
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void Stop(string name) {
        try {
            FindSound(name).source.Stop();
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }       
    }

    public void StopAllSounds() {
        foreach (KeyValuePair<string, Sound> s in allSoundsDictionary) {
            try {
                s.Value.source.Stop();
            } catch (System.Exception) {

            }
        }
    }

    private void InWorldSpaceRoutine(string name, GameObject soundAtLocationGO) {
        sound = allSoundsDictionary[name];
        sound.source = soundAtLocationGO.GetComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.spatialBlend = sound.spatialBlend_2D_3D;
        sound.source.rolloffMode = (AudioRolloffMode)sound.rolloffMode;
        sound.source.minDistance = sound.minDistance;
        sound.source.maxDistance = sound.maxDistance;
        sound.source.loop = sound.loop;
    }

    #endregion

    #region PlayRandom
    public void Play(string name, float minPitch, float maxPitch) {
        sound = FindSound(name);
        try {
            sound.source.pitch = Random.Range(minPitch, maxPitch);
            sound.source.Play();
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void PlaySFX(string name, float minPitch, float maxPitch) {
        sound = FindSFX(name);
        try {
            sound.source.pitch = Random.Range(minPitch, maxPitch);
            sound.source.Play();
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }       
    }

    public void PlayRandomSFX_InWorldspace(string name, string name2, GameObject gameObjectLocation, float minPitch, float maxPitch) {
        int i = Random.Range(1, 2);
        try {
            if (allSoundsDictionary.ContainsKey(name)) {
                GameObject soundAtLocationGO = Instantiate(soundObject, gameObjectLocation.transform.position, Quaternion.identity, gameObjectLocation.transform);
                if (i == 1) {
                    sound = allSoundsDictionary[name];
                } else if (i == 2) {
                    sound = allSoundsDictionary[name2];
                }
                sound.source = soundAtLocationGO.GetComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.spatialBlend = sound.spatialBlend_2D_3D;
                sound.source.rolloffMode = (AudioRolloffMode)sound.rolloffMode;
                sound.source.minDistance = sound.minDistance;
                sound.source.maxDistance = sound.maxDistance;
                sound.source.loop = sound.loop;
                sound.source.pitch = Random.Range(minPitch, maxPitch);
                sound.source.Play();
                if (!sound.source.loop) {
                    Destroy(soundAtLocationGO, sound.clip.length);
                }
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void PlayRandomSFX(string name, string name2) {
        int i = Random.Range(1, 2);
        switch (i) {
            case 1:
                sound = FindSFX(name);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 2:
                sound = FindSFX(name2);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            default:
                Debug.LogError("RandomSound did not execute correctly, i out of range");
                break;
        }
    }

    public void PlayRandomSFX(string name, string name2, string name3) {
        int i = Random.Range(1, 3);
        switch (i) {
            case 1:
                sound = FindSFX(name);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 2:
                sound = FindSFX(name2);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 3:
                sound = FindSFX(name3);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            default:
                Debug.LogError("RandomSound did not execute correctly, i out of range");
                break;
        }
    }

    public void PlayRandomSFX(string name, string name2, string name3, string name4) {
        int i = Random.Range(1, 4);
        switch (i) {
            case 1:
                sound = FindSFX(name);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 2:
                sound = FindSFX(name2);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 3:
                sound = FindSFX(name3);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 4:
                sound = FindSFX(name4);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            default:
                Debug.LogError("RandomSound did not execute correctly, i out of range");
                break;
        }
    }

    public void PlayRandomSFX(string name, string name2, string name3, string name4, string name5) {
        int i = Random.Range(1, 5);
        switch (i) {
            case 1:
                sound = FindSFX(name);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 2:
                sound = FindSFX(name2);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 3:
                sound = FindSFX(name3);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 4:
                sound = FindSFX(name4);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            case 5:
                sound = FindSFX(name5);
                sound.source.pitch = Random.Range(0.95f, 1f);
                sound.source.Play();
                break;
            default:
                Debug.LogError("RandomSound did not execute correctly, i out of range");
                break;
        }
    }


    #endregion

    #region Volume / Pitch Methods

    public void SoundSetVolume(string name, float volume) {
        try {
            FindSound(name).source.volume = volume;
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void SFXSetPitch(float pitch) {
        audioMixer.SetFloat("SFXPitch", pitch);
    }

    public void SFXSetVolume(float volume) {
        if (volume == -80) {
            audioMixer.SetFloat("SFXVolume", volume);
        } else {
            audioMixer.SetFloat("SFXVolume", (volume / 4));
        }
    }

    public void MusicSetVolume(float volume) {
        if (volume == -80) {
            audioMixer.SetFloat("MusicVolume", volume);
        } else {
            audioMixer.SetFloat("MusicVolume", (volume / 4));
        }
    }

    public void VoiceSetVolume(float volume) {
        if (volume == -80) {
            audioMixer.SetFloat("VoiceVolume", volume);
        } else {
            audioMixer.SetFloat("VoiceVolume", (volume / 4));
        }
    }

    public void AllSoundsSetVolume(float volume) {
        if (volume == -80) {
            audioMixer.SetFloat("MasterVolume", volume);
        } else {
            audioMixer.SetFloat("MasterVolume", (volume / 4));
        }
    }
    #endregion

    #region Pause Methods
    public void Pause(string name, bool pause) {
        try {
            if (pause) {
                FindSound(name).source.Pause();
            } else if (!pause) {
                FindSound(name).source.UnPause();
            }           
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void PauseAllSFX(bool pause) {
        if (pause) {
            foreach (KeyValuePair<string, Sound> s in sfxDictionary) {
                try {
                    s.Value.source.Pause();
                } catch (System.Exception) {

                }
            }
        } else if (!pause) {
            foreach (KeyValuePair<string, Sound> s in sfxDictionary) {
                try {
                    s.Value.source.UnPause();
                } catch (System.Exception) {

                }
            }
        }
    }

    public void PauseAllSound(bool pause) {
        if (pause) {
            foreach (KeyValuePair<string, Sound> s in allSoundsDictionary) {
                try {
                    s.Value.source.Pause();
                } catch (System.Exception) {

                }                
            }                    
        } else if (!pause) {
            foreach (KeyValuePair<string, Sound> s in allSoundsDictionary) {

                try {
                    s.Value.source.UnPause();
                } catch (System.Exception) {

                }
            }
        }

    }
    #endregion

    #region FadeIn/Out Methods
    public void FadeIn(string name, float fadeDuration, float soundVolumePercentage) {
        try {
            sound = FindSound(name);
            if (sound != null) {
                StartCoroutine(FadeInAudio(fadeDuration, (soundVolumePercentage/100), sound));
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }

    }

    public void FadeOut(string name, float fadeDuration, float soundVolumePercentage) {
        try {
            sound = FindSound(name);
            if(sound != null) {
                StartCoroutine(FadeOutAudio(fadeDuration, (soundVolumePercentage/100), sound));
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    private IEnumerator FadeInAudio(float fadeDuration, float soundVolume, Sound sound) {
        continueFadeIn = true;
        continueFadeOut = false;
        float startSoundValue = 0;
        if (continueFadeIn) {
            for (float time = 0f; time < fadeDuration; time += Time.unscaledDeltaTime) {
                float normalizedTime = time / fadeDuration;
                sound.source.volume = Mathf.Lerp(startSoundValue, soundVolume, normalizedTime);
                yield return null;
            }
        }
    }

    private IEnumerator FadeOutAudio(float fadeDuration, float soundVolume, Sound sound) {
        continueFadeIn = false;
        continueFadeOut = true;
        float startSoundValue = sound.source.volume;
        if (continueFadeOut) {
            for (float time = 0f; time < fadeDuration; time += Time.unscaledDeltaTime) {
                if (sound.source.volume <= 0.01) {
                    sound.source.Stop();
                }
                float normalizedTime = time / fadeDuration;
                sound.source.volume = Mathf.Lerp(startSoundValue, soundVolume, normalizedTime);
                yield return null;
            }
        }
    }
    #endregion

    #region FindSound Methods

    private Sound FindSound(string name) {
        if (allSoundsDictionary.ContainsKey(name)) {
            return allSoundsDictionary[name];
        }
        return null;
    }

    private Sound FindMusic(string name) {
        if (musicDictionary.ContainsKey(name)) {
            return musicDictionary[name];
        }
        return null;
    }

    private Sound FindSFX(string name) {
        if (sfxDictionary.ContainsKey(name)) {
            return sfxDictionary[name];
        }
        return null;
    }

    private Sound FindVoiceLine(string name) {
        if (voiceDictionary.ContainsKey(name)) {
            return voiceDictionary[name];
        }
        return null;
    }

    #endregion

    #region WaitForFinish/PlayDelay Methods

    public void Play_Delay(string name, float minDelay, float maxDelay) {
        try {
            sound = FindSound(name);
            StartCoroutine(PlayDelaySequence(sound, minDelay, maxDelay));
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void Play_Delay(string name, float minDelay, float maxDelay, float minPitch, float maxPitch) {
        try {
            sound = FindSound(name);
            StartCoroutine(PlayDelaySequence(sound, minDelay, maxDelay, minPitch, maxPitch));
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    private IEnumerator PlayDelaySequence(Sound sound, float minDelay, float maxDelay) {
        float delay = Random.Range(minDelay, maxDelay);

        sound.source.PlayDelayed(delay);
        yield return new WaitForSeconds(delay + sound.source.clip.length);
        Play_Delay(sound.name, minDelay, maxDelay);
        yield return null;
    }

    private IEnumerator PlayDelaySequence(Sound sound, float minDelay, float maxDelay, float minPitch, float maxPitch) {
        float delay = Random.Range(minDelay, maxDelay);
        float pitch = Random.Range(minPitch, maxPitch);

        

        sound.source.pitch = pitch;
        sound.source.PlayDelayed(delay);
        yield return new WaitForSeconds(delay + sound.source.clip.length);
        Play_Delay(sound.name, minDelay, maxDelay, minPitch, maxPitch);
        yield return null;
    }

    public void Play_ThenPlay(string name, string otherName) {           
        try {
            Sound firstSound = FindSound(name);
            Sound secondSound = FindSound(otherName);
            firstSound.source.Play();
            StartCoroutine(WaitToPlay(secondSound, firstSound.source.clip.length));
        } catch (System.NullReferenceException) {
            AudioNotFound(name); AudioNotFound(otherName);
        }
    }

    public void Play_ThenPlay(string name, string otherName, string thirdname) {
        try {
            Sound firstSound = FindSound(name);
            Sound secondSound = FindSound(otherName);
            Sound thirdSound = FindSound(thirdname);
            firstSound.source.Play();
            StartCoroutine(WaitToPlay(secondSound, firstSound.source.clip.length));
            StartCoroutine(WaitToPlay(thirdSound, firstSound.source.clip.length + secondSound.source.clip.length));
        } catch (System.NullReferenceException) {
            AudioNotFound(name); AudioNotFound(otherName); AudioNotFound(thirdname);
        }
    }

    private IEnumerator WaitToPlay(Sound sound, float waitTime) {
        yield return new WaitForSecondsRealtime(waitTime);
        sound.source.Play();
        yield return null;
    }

    public void PlaySFX_Finish(string name, float minPitch, float maxPitch, float extraTimeIntervall) {
        sound = FindSFX(name);
        try {
            if (!soundTimerDictonary.ContainsKey(sound.name)) {
                soundTimerDictonary[sound.name] = 0f;
            }
            sound.source.pitch = Random.Range(minPitch, maxPitch);
            if (CanPlaySound(sound, extraTimeIntervall)) {
                sound.source.Play();
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    public void PlaySFX_Finish(string name, float minPitch, float maxPitch, float extraTimeIntervall, float minVolume, float maxVolume) {
        sound = FindSFX(name);
        try {
            if (!soundTimerDictonary.ContainsKey(sound.name)) {
                soundTimerDictonary[sound.name] = 0f;
            }
            sound.source.pitch = Random.Range(minPitch, maxPitch);
            sound.source.volume = Random.Range(minVolume, maxVolume);
            if (CanPlaySound(sound, extraTimeIntervall)) {
                sound.source.Play();
            }
        } catch (System.NullReferenceException) {
            AudioNotFound(name);
        }
    }

    private bool CanPlaySound(Sound sound, float extraTime) {
        if (soundTimerDictonary.ContainsKey(sound.name)) {
            float lastTimePlayed = soundTimerDictonary[sound.name];
            float soundTime = sound.source.clip.length;
            if (lastTimePlayed + soundTime + extraTime < Time.time) {
                soundTimerDictonary[sound.name] = Time.time;
                return true;
            } else {
                return false;
            }
        }
        return true;

    }

    #endregion

    #region GetSoundLength Methods

    public float GetSoundLength(string name) {
        if (allSoundsDictionary.ContainsKey(name)) {
            return allSoundsDictionary[name].source.clip.length;
        }
        return 0;
    }

    #endregion

    private void AudioNotFound(string name) {
        Debug.LogWarning("The sound with name '" + name + "' could not be found in list. Is it spelled correctly? (NullReferenceException)");
    }

}
