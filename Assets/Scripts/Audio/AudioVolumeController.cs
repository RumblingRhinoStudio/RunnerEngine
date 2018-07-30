using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVolumeController : MonoBehaviour
{

    #region Properties

    [SerializeField]
    private FloatReference _musicVolume;
    public FloatReference musicVolume
    {
        get { return _musicVolume; }
    }

    private AudioSource _audioSource;

    #endregion


    #region Public Methods

    public void UpdateVolume()
    {
        _audioSource.volume = musicVolume.UseConstant ? musicVolume.ConstantValue : musicVolume.Variable.Value;
    }

    #endregion


    #region MonoBehaviour Messages

    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        UpdateVolume();
    }

    #endregion

}