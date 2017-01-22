using UnityEngine;


public class AudioService : MonoBehaviour
{
	private static AudioService _instance;

	public static AudioService Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

    public AudioClip Jump;
    public AudioClip Dash;
    public AudioClip Bumper;
    public AudioClip Break;
    public AudioClip Middle;
    public AudioClip HitWall;
	public AudioClip NoGrav;

    public void PlayJump()
    {
        PlaySound(Jump);
    }

	public void PlayDash()
    {
		PlaySound(Dash);
    }

	public void PlayBumper()
    {
		PlaySound(Bumper);
    }

	public void PlayMiddle()
    {
		PlaySound(Middle);
    }

    public void PlayHitWall()
    {
		PlaySound(HitWall);
    }

	public void PlayBreak()
    {
		PlaySound(Break);
    }

	public void PlayNoGrav()
	{
		PlaySound(NoGrav);
	}

    private AudioSource PlaySound(AudioClip clip)
    {
        var child = new GameObject();
        child.transform.parent = this.transform;
        child.name = "SoundEmitter_" + clip.name;
        AudioSource audioSource = child.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(child, clip.length);
        return audioSource;
    }
}