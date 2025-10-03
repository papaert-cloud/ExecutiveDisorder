using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Library")]
    [SerializeField] private List<Sound> sounds = new List<Sound>();  // Drag clips here

    [Header("Mixer (optional, but recommended)")]
    [SerializeField] private AudioMixer mixer;                   // Optional
    [SerializeField] private AudioMixerGroup masterGroup;        // Optional
    [SerializeField] private AudioMixerGroup musicGroup;         // Optional
    [SerializeField] private AudioMixerGroup sfxGroup;           // Optional
    // Expose these params in your mixer if you want volume control from code:
    [SerializeField] private string masterVolParam = "MasterVol";
    [SerializeField] private string musicVolParam = "MusicVol";
    [SerializeField] private string sfxVolParam = "SFXVol";

    [Header("Music & Ambience")]
    [SerializeField] private int musicFadeMs = 800;
    [SerializeField] private int ambienceFadeMs = 800;

    [Header("SFX Pool")]
    [Range(1, 64)][SerializeField] private int sfxPoolSize = 16;
    [SerializeField] private bool expandPoolIfNeeded = true;

    private readonly Dictionary<SoundType, AudioClip> _clips = new();
    private readonly List<AudioSource> _sfxPool = new();
    private int _nextSfxIndex = 0;

    private AudioSource _musicA, _musicB;
    private AudioSource _ambienceA, _ambienceB;
    private bool _musicUsingA = true;
    private bool _ambUsingA = true;

    // === NEW: track the currently playing music id ===
    private SoundType _currentMusicId;

    #region Setup
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build clip dictionary
        foreach (var s in sounds)
        {
            if (s.clip == null || s.id == SoundType.None) continue;
            if (!_clips.ContainsKey(s.id)) _clips.Add(s.id, s.clip);
        }

        // Create SFX pool
        for (int i = 0; i < sfxPoolSize; i++)
            _sfxPool.Add(CreateSource("SFX_" + i, sfxGroup, loop: false));

        // Music & ambience dual sources (for crossfades)
        _musicA = CreateSource("Music_A", musicGroup, loop: true, spatialBlend: 0f);
        _musicB = CreateSource("Music_B", musicGroup, loop: true, spatialBlend: 0f);
        _ambienceA = CreateSource("Ambience_A", musicGroup ? musicGroup : masterGroup, loop: true, spatialBlend: 0.0f);
        _ambienceB = CreateSource("Ambience_B", musicGroup ? musicGroup : masterGroup, loop: true, spatialBlend: 0.0f);

        _musicA.volume = _musicB.volume = 0f;
        _ambienceA.volume = _ambienceB.volume = 0f;
    }

    private AudioSource CreateSource(string name, AudioMixerGroup group, bool loop, float spatialBlend = 0f)
    {
        var go = new GameObject(name);
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = loop;
        src.spatialBlend = Mathf.Clamp01(spatialBlend);
        if (group != null) src.outputAudioMixerGroup = group;
        return src;
    }
    #endregion

    #region Public API — SFX
    /// <summary>Play a 2D one-shot SFX by id (from the library).</summary>
    public void PlaySFX(SoundType id, float volume = 1f, float pitch = 1f)
        => PlaySFXInternal(id, null, volume, pitch, spatialBlend: 0f);

    /// <summary>
    /// Stops all currently playing SFX that match the given id.
    /// </summary>
    public void StopSFX(SoundType id)
    {
        if (!_clips.TryGetValue(id, out var clip) || clip == null) return;

        foreach (var src in _sfxPool)
        {
            if (src.isPlaying && src.clip == clip)
            {
                src.Stop();
            }
        }
    }

    /// <summary>
    /// Play a 2D SFX by id starting at a specific time (in seconds).
    /// </summary>
    public void PlaySFXAtTime(SoundType id, float startTimeSeconds, float volume = 1f, float pitch = 1f)
    {
        if (!_clips.TryGetValue(id, out var clip) || clip == null) return;

        var sound = sounds.Find(s => s.id == id);
        float finalVolume = volume * (sound.volumeMul <= 0 ? 1f : sound.volumeMul);

        var src = GetNextSfxSource();
        ConfigureSfxSource(src, finalVolume, pitch, spatialBlend: 0f, worldPos: null);

        src.clip = clip;
        src.time = Mathf.Clamp(startTimeSeconds, 0f, clip.length - 0.01f); // safe clamp
        src.Play();
    }

    /// <summary>
    /// Play a 2D SFX by id, starting at a specific time, and stop after `durationSeconds` with a fade-out.
    /// Example: PlaySFXFor("whoosh", durationSeconds: 2f, startTimeSeconds: 0f, fadeOutSeconds: 0.15f)
    /// </summary>
    public void PlaySFXFor(SoundType id, float durationSeconds, float startTimeSeconds = 0f, float fadeOutSeconds = 0.1f, float volume = 1f, float pitch = 1f)
    {
        if (!_clips.TryGetValue(id, out var clip) || clip == null) return;

        var sound = sounds.Find(s => s.id == id);
        float finalVolume = volume * (sound.volumeMul <= 0 ? 1f : sound.volumeMul);

        var src = GetNextSfxSource();
        ConfigureSfxSource(src, finalVolume, pitch, spatialBlend: 0f, worldPos: null);

        // Use clip playback (not PlayOneShot) so we can time and stop
        src.clip = clip;
        src.time = Mathf.Clamp(startTimeSeconds, 0f, Mathf.Max(0f, clip.length - 0.01f));
        src.Play();

        // Schedule fade-out and stop
        StartCoroutine(StopSfxAfterWithFade(src, clip, durationSeconds, fadeOutSeconds));
    }

    /// <summary>
    /// Play a 2D SFX by id, starting at a specific time, and stop after `durationSeconds` with a fade-out.
    /// Pitch will be randomized between minPitch and maxPitch.
    /// </summary>
    public void PlaySFXForRandomPitch(SoundType id, float durationSeconds, float minPitch, float maxPitch,
        float startTimeSeconds = 0f, float fadeOutSeconds = 0.1f, float volume = 1f)
    {
        if (!_clips.TryGetValue(id, out var clip) || clip == null) return;

        var sound = sounds.Find(s => s.id == id);
        float finalVolume = volume * (sound.volumeMul <= 0 ? 1f : sound.volumeMul);

        var src = GetNextSfxSource();
        ConfigureSfxSource(src, finalVolume, UnityEngine.Random.Range(minPitch, maxPitch),
            spatialBlend: 0f, worldPos: null);

        // Use clip playback (not PlayOneShot) so we can time and stop
        src.clip = clip;
        src.time = Mathf.Clamp(startTimeSeconds, 0f, Mathf.Max(0f, clip.length - 0.01f));
        src.Play();

        // Schedule fade-out and stop
        StartCoroutine(StopSfxAfterWithFade(src, clip, durationSeconds, fadeOutSeconds));
    }


    /// <summary>Play a 3D SFX at world position.</summary>
    public void PlaySFXAt(SoundType id, Vector3 position, float volume = 1f, float pitch = 1f, float spatialBlend = 1f)
        => PlaySFXInternal(id, position, volume, pitch, spatialBlend);

    /// <summary>Play a raw clip (useful for dynamic content).</summary>
    public void PlayClip(AudioClip clip, float volume = 1f, float pitch = 1f, float spatialBlend = 0f, Vector3? worldPos = null)
    {
        if (clip == null) return;
        var src = GetNextSfxSource();
        ConfigureSfxSource(src, volume, pitch, spatialBlend, worldPos);
        src.PlayOneShot(clip, 1f);
    }
    #endregion

    #region Public API — Music & Ambience

    /// <summary>
    /// Returns true if the given music id is currently the selected/playing music.
    /// </summary>
    public bool IsMusicPlaying(SoundType id)
    {
        if (id == SoundType.None || _currentMusicId == SoundType.None) return false;
        return _currentMusicId == id;
    }

    public void PlayMusic(SoundType id, float fadeSeconds = -1f, bool loop = true)
    {
        // Ignore if already playing this track
        if (IsMusicPlaying(id)) return;

        if (!_clips.TryGetValue(id, out var clip) || clip == null)
            return;

        var sound = sounds.Find(s => s.id == id);
        float mul = sound.volumeMul <= 0 ? 1f : sound.volumeMul;

        _currentMusicId = id; // track what's intended to play
        Crossfade(clip, isMusic: true, fadeSeconds: fadeSeconds, loop: loop, volumeMul: mul);
    }

    public void PlayMusic(AudioClip clip, float fadeSeconds = -1f, bool loop = true)
    {
        if (clip == null) return;

        // If this clip corresponds to a known id and it's the same, ignore.
        // (Optional convenience: if not found, allow crossfade.)
        foreach (var kv in _clips)
        {
            if (kv.Value == clip && IsMusicPlaying(kv.Key))
                return;
        }

        _currentMusicId = FindIdByClip(clip); // may be null if clip not in library (that's fine)
        Crossfade(clip, isMusic: true, fadeSeconds: fadeSeconds, loop: loop);
    }

    public void StopMusic(float fadeSeconds = -1f)
    {
        _currentMusicId = SoundType.None; // clear current id
        Crossfade(null, isMusic: true, fadeSeconds: fadeSeconds, loop: false);
    }

    public void PlayAmbience(SoundType id, float fadeSeconds = -1f, bool loop = true)
    {
        var sound = sounds.Find(s => s.id == id);
        float mul = sound.volumeMul <= 0 ? 1f : sound.volumeMul;
        Crossfade(_clips.GetValueOrDefault(id), isMusic: false, fadeSeconds: fadeSeconds, loop: loop, volumeMul: mul);
    }

    public void StopAmbience(float fadeSeconds = -1f)
        => Crossfade(null, isMusic: false, fadeSeconds: fadeSeconds, loop: false);

    #endregion

    #region Public API — Volume (Mixer)
    public void SetMasterVolume(float linear01) => SetMixerLinear(masterVolParam, linear01);
    public void SetMusicVolume(float linear01) => SetMixerLinear(musicVolParam, linear01);
    public void SetSfxVolume(float linear01) => SetMixerLinear(sfxVolParam, linear01);

    private void SetMixerLinear(string param, float linear01)
    {
        if (mixer == null || string.IsNullOrWhiteSpace(param)) return;
        linear01 = Mathf.Clamp01(linear01);
        mixer.SetFloat(param, LinearToDb(linear01));
    }
    #endregion

    #region Internals
    private void PlaySFXInternal(SoundType id, Vector3? worldPos, float volume, float pitch, float spatialBlend)
    {
        if (!_clips.TryGetValue(id, out var clip) || clip == null) return;

        // Retrieve the sound definition to get its volumeMul
        var sound = sounds.Find(s => s.id == id);
        float finalVolume = volume * (sound.volumeMul <= 0 ? 1f : sound.volumeMul);

        var src = GetNextSfxSource();
        ConfigureSfxSource(src, finalVolume, pitch, spatialBlend, worldPos);
        src.clip = clip;
        src.Play();
    }

    private AudioSource GetNextSfxSource()
    {
        // Find a free source first
        for (int i = 0; i < _sfxPool.Count; i++)
        {
            int idx = (i + _nextSfxIndex) % _sfxPool.Count;
            if (!_sfxPool[idx].isPlaying) { _nextSfxIndex = (idx + 1) % _sfxPool.Count; return _sfxPool[idx]; }
        }
        if (expandPoolIfNeeded)
        {
            var src = CreateSource("SFX_" + _sfxPool.Count, sfxGroup, loop: false);
            _sfxPool.Add(src);
            _nextSfxIndex = (_sfxPool.Count - 1 + 1) % _sfxPool.Count;
            return src;
        }
        // All busy: overwrite in round-robin
        var chosen = _sfxPool[_nextSfxIndex];
        _nextSfxIndex = (_nextSfxIndex + 1) % _sfxPool.Count;
        return chosen;
    }

    private IEnumerator StopSfxAfterWithFade(AudioSource src, AudioClip clip, float delaySeconds, float fadeOutSeconds)
    {
        // Clamp values
        delaySeconds = Mathf.Max(0f, delaySeconds);
        fadeOutSeconds = Mathf.Max(0f, fadeOutSeconds);

        // Wait the requested delay (unscaled, to match your crossfade style)
        float elapsed = 0f;
        while (elapsed < delaySeconds)
        {
            // If this source got repurposed or stopped, bail out safely
            if (src == null || !src.isPlaying || src.clip != clip) yield break;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // If fade time is zero or source changed, stop immediately
        if (fadeOutSeconds <= 0f || src == null || !src.isPlaying || src.clip != clip)
        {
            if (src != null) src.Stop();
            yield break;
        }

        // Fade out
        float t = 0f;
        float start = src.volume;
        while (t < fadeOutSeconds)
        {
            if (src == null || !src.isPlaying || src.clip != clip) yield break;
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / fadeOutSeconds);
            src.volume = Mathf.Lerp(start, 0f, k);
            yield return null;
        }

        if (src != null && src.isPlaying && src.clip == clip)
        {
            src.Stop();
            src.volume = 0f; // reset for next reuse
        }
    }

    private void ConfigureSfxSource(AudioSource src, float volume, float pitch, float spatialBlend, Vector3? worldPos)
    {
        src.volume = Mathf.Clamp01(volume);
        src.pitch = Mathf.Clamp(pitch, -3f, 3f);
        src.spatialBlend = Mathf.Clamp01(spatialBlend);
        if (worldPos.HasValue)
        {
            src.transform.position = worldPos.Value;
            src.rolloffMode = AudioRolloffMode.Logarithmic;
            src.minDistance = 1f;
            src.maxDistance = 50f;
        }
        else
        {
            src.transform.localPosition = Vector3.zero;
        }
    }

    private void Crossfade(AudioClip target, bool isMusic, float fadeSeconds, bool loop, float volumeMul = 1f)
    {
        var ms = (isMusic ? musicFadeMs : ambienceFadeMs);
        if (fadeSeconds >= 0f) ms = Mathf.RoundToInt(fadeSeconds * 1000f);

        var active = isMusic ? (_musicUsingA ? _musicA : _musicB) : (_ambUsingA ? _ambienceA : _ambienceB);
        var idle = isMusic ? (_musicUsingA ? _musicB : _musicA) : (_ambUsingA ? _ambienceB : _ambienceA);

        StopCoroutineSafe(isMusic ? _musicFadeRoutine : _ambFadeRoutine);

        if (target == null)
        {
            if (isMusic) _musicFadeRoutine = StartCoroutine(FadeOutAndStop(active, ms));
            else _ambFadeRoutine = StartCoroutine(FadeOutAndStop(active, ms));
            return;
        }

        idle.clip = target;
        idle.loop = loop;
        idle.time = 0f;
        idle.volume = 0f;
        idle.Play();

        if (isMusic)
            _musicFadeRoutine = StartCoroutine(CrossfadeRoutine(active, idle, ms, () => _musicUsingA = idle == _musicA, volumeMul));
        else
            _ambFadeRoutine = StartCoroutine(CrossfadeRoutine(active, idle, ms, () => _ambUsingA = idle == _ambienceA, volumeMul));
    }

    private Coroutine _musicFadeRoutine, _ambFadeRoutine;
    private void StopCoroutineSafe(Coroutine c) { if (c != null) StopCoroutine(c); }

    private IEnumerator CrossfadeRoutine(AudioSource from, AudioSource to, int ms, Action onDone, float volumeMul = 1f)
    {
        float t = 0f, dur = Mathf.Max(0.001f, ms / 1000f);
        float fromStart = from != null ? from.volume : 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = t / dur;
            if (from != null) from.volume = Mathf.Lerp(fromStart, 0f, k);
            to.volume = Mathf.Lerp(0f, 1f * volumeMul, k);
            yield return null;
        }
        if (from != null) { from.Stop(); from.volume = 0f; }
        to.volume = 1f * volumeMul;
        onDone?.Invoke();
    }

    private IEnumerator FadeOutAndStop(AudioSource src, int ms)
    {
        if (src == null || !src.isPlaying) yield break;
        float t = 0f, dur = Mathf.Max(0.001f, ms / 1000f);
        float start = src.volume;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            src.volume = Mathf.Lerp(start, 0f, t / dur);
            yield return null;
        }
        src.Stop();
        src.volume = 0f;
    }

    private static float LinearToDb(float linear01)
    {
        if (linear01 <= 0.0001f) return -80f; // effectively mute
        return Mathf.Log10(linear01) * 20f;
    }
    #endregion

    #region Helpers
    private SoundType FindIdByClip(AudioClip clip)
    {
        foreach (var kv in _clips)
        {
            if (kv.Value == clip) return kv.Key;
        }
        return SoundType.None;
    }
    #endregion

    [Serializable]
    private struct Sound
    {
        public SoundType id;
        public AudioClip clip;  // drag & drop
        [Range(0f, 2f)]
        public float volumeMul;     // baseline multiplier (default 1)
    }

    public enum SoundType
    {
        None,
        Stamp,
        Correct,
        Incorrect,
        Boo,
        Applause,
        WalkUpstaris,
        News,
        Siren,
        AlienSiren,
        EmergencySiren,
        BusySquare,
        RelaxingPark,
        RelaxingGuitar,
        CrowdNoise,
        TvNews,
        TvStatic,
        Die,
        TvOff,
        ClockOff,
        Confetti,
        Pop
    }
}
