using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    [Header("===== Audio Sources =====")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float pitchShift = 0.1f;

    [Space(10)]
    [Title("===== SFX Clips Grouped =====")]
    [ListDrawerSettings(DraggableItems = true, Expanded = true)]
    [SerializeField] private List<SFXGroup> sfxGroups = new List<SFXGroup>();

    [Space(10)]
    [Title("===== BGM Clips Grouped =====")]
    [ListDrawerSettings(DraggableItems = true, Expanded = true)]
    [SerializeField] private List<BGMGroup> bgmGroups = new List<BGMGroup>();

    private Dictionary<string, List<AudioClip>> sfxDict;
    private Dictionary<string, AudioClip> bgmDict;
    private string currentBGM = null;

    private const string KEY_MUSIC = "KEY_MUSIC";
    private const string KEY_SFX = "KEY_SFX";

    [System.Serializable]
    public class SFXGroup
    {
        [HorizontalGroup("Group")]
        [LabelWidth(100)]
        public string groupName;

        [HorizontalGroup("Group")]
        [ListDrawerSettings(Expanded = true)]
        public List<AudioClip> clips = new List<AudioClip>();
    }

    [System.Serializable]
    public class BGMGroup
    {
        [HorizontalGroup("Group")]
        [LabelWidth(100)]
        public string groupName;

        [HorizontalGroup("Group")]
        public AudioClip clip;
    }

    private void Awake()
    {
        InitializeSFXDictionary();
        InitializeBGMDictionary();
        ApplySavedSettings();
        base.Awake();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DetectScene();
    }

    private void InitializeSFXDictionary()
    {
        sfxDict = new Dictionary<string, List<AudioClip>>();
        foreach (var group in sfxGroups)
        {
            if (!sfxDict.ContainsKey(group.groupName))
                sfxDict.Add(group.groupName, group.clips);
            else
                Debug.LogWarning($"[AudioManager] Duplicate SFX group: {group.groupName}");
        }
    }

    private void InitializeBGMDictionary()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var group in bgmGroups)
        {
            if (!bgmDict.ContainsKey(group.groupName))
                bgmDict.Add(group.groupName, group.clip);
            else
                Debug.LogWarning($"[AudioManager] Duplicate BGM group: {group.groupName}");
        }
    }

    private void ApplySavedSettings()
    {
        bool musicOn = PlayerPrefs.GetInt(KEY_MUSIC, 1) == 1;
        bool sfxOn = PlayerPrefs.GetInt(KEY_SFX, 1) == 1;

        if (musicSource != null)
            musicSource.mute = !musicOn;

        if (sfxSource != null)
            sfxSource.mute = !sfxOn;
    }

    #region Music Controls
    public void SetMusic(bool isOn)
    {
        PlayerPrefs.SetInt(KEY_MUSIC, isOn ? 1 : 0);
        PlayerPrefs.Save();
        if (musicSource != null)
            musicSource.mute = !isOn;
    }

    public void PlayBGM(string groupName, bool loop = true)
    {
        if (bgmDict == null || !bgmDict.TryGetValue(groupName, out var clip))
        {
            Debug.LogWarning($"[AudioManager] BGM not found: {groupName}");
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = 1f;
        musicSource.Play();
    }

    public void StopBGM()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }

    public void FadeInBGM(string groupName, float duration = 1f, bool loop = true)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInRoutine(groupName, duration, loop));
    }

    public void FadeOutBGM(float duration = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutRoutine(duration));
    }

    private IEnumerator FadeInRoutine(string groupName, float duration, bool loop)
    {
        if (!bgmDict.TryGetValue(groupName, out var clip)) yield break;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = 0f;
        musicSource.Play();

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        musicSource.volume = 1f;
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float startVol = musicSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = 1f;
    }

    public void ApplyLowPass(float cutoff = 300f)
    {
        var filter = musicSource.GetComponent<AudioLowPassFilter>();
        if (filter == null) filter = musicSource.gameObject.AddComponent<AudioLowPassFilter>();
        filter.cutoffFrequency = cutoff;
    }

    public void ApplyHighPass(float cutoff = 700f)
    {
        var filter = musicSource.GetComponent<AudioHighPassFilter>();
        if (filter == null) filter = musicSource.gameObject.AddComponent<AudioHighPassFilter>();
        filter.cutoffFrequency = cutoff;
    }

    public void ResetFilters()
    {
        var low = musicSource.GetComponent<AudioLowPassFilter>();
        var high = musicSource.GetComponent<AudioHighPassFilter>();

        if (low != null) Destroy(low);
        if (high != null) Destroy(high);
    }
    
    public void CrossfadeBGM(string groupName, float duration = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(CrossfadeRoutine(groupName, duration));
    }

    private IEnumerator CrossfadeRoutine(string groupName, float duration)
    {
        if (!bgmDict.TryGetValue(groupName, out var newClip)) yield break;

        float startVol = musicSource.volume;
        float t = 0f;
        
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.outputAudioMixerGroup = musicSource.outputAudioMixerGroup;
        newSource.clip = newClip;
        newSource.loop = true;
        newSource.volume = 0f;
        
        float currentTime = musicSource.time;
        newSource.time = Mathf.Repeat(currentTime, newClip.length);

        newSource.Play();
        
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
            newSource.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        musicSource.Stop();
        Destroy(musicSource);
        musicSource = newSource; 
    }


    private void DetectScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        string targetBGM = null;

        if (sceneName == SceneNames.MENU) 
        {
            targetBGM = AudioPaths.MAINHOME;
        }
        else if (sceneName == SceneNames.GAME) 
        {
            targetBGM = AudioPaths.GAME;
        }
        
        if (targetBGM == null || targetBGM == currentBGM)
            return;
        
        currentBGM = targetBGM;
        CrossfadeBGM(targetBGM, 1f);
    }
    
    #endregion

    #region SFX Controls
    public void SetSFX(bool isOn)
    {
        PlayerPrefs.SetInt(KEY_SFX, isOn ? 1 : 0);
        PlayerPrefs.Save();
        if (sfxSource != null)
            sfxSource.mute = !isOn;
    }

    public void PlaySFX(string groupName, bool loop = false, bool isPitchShift = true)
    {
        if (PlayerPrefs.GetInt(KEY_SFX, 1) == 0) return;

        if (sfxDict == null || !sfxDict.TryGetValue(groupName, out var clips)) return;
        if (clips == null || clips.Count == 0) return;

        sfxSource.pitch = isPitchShift ? Random.Range(1 - pitchShift, 1 + pitchShift) : 1;
        var randomClip = clips[Random.Range(0, clips.Count)];

        if (!loop)
            sfxSource.PlayOneShot(randomClip);
        else
        {
            sfxSource.clip = randomClip;
            sfxSource.loop = true;
            sfxSource.Play();
        }
    }
    #endregion

    [Button("Generate AudioPaths.cs")]
    private void GenerateAudioPaths()
    {
        string folderPath = Application.dataPath + "/Scripts/Generated"; 
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, "AudioPaths.cs");

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public static class AudioPaths");
        sb.AppendLine("{");

        foreach (var group in sfxGroups)
        {
            if (!string.IsNullOrEmpty(group.groupName))
            {
                string safeName = group.groupName.Replace(" ", "_").ToUpper();
                sb.AppendLine($"\tpublic const string {safeName} = \"{group.groupName}\";");
            }
        }
        foreach (var group in bgmGroups)
        {
            if (!string.IsNullOrEmpty(group.groupName))
            {
                string safeName = group.groupName.Replace(" ", "_").ToUpper();
                sb.AppendLine($"\tpublic const string {safeName} = \"{group.groupName}\";");
            }
        }

        sb.AppendLine("}");

        File.WriteAllText(filePath, sb.ToString());
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        Debug.Log($"[AudioManager] AudioPaths.cs generated at {filePath}");
    }
}
