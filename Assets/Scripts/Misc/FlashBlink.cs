using UnityEngine;

public class FlashBlink : MonoBehaviour
{
	[SerializeField] private MonoBehaviour damageableObject;
	[SerializeField] private Material blinkMaterial;
	[SerializeField] private float blinkDuration = 0.2f;

	private float _blinkTimer;
	private Material _defaultMaterial;
	private SpriteRenderer _spriteRenderer;
	private bool _isBlinking;
	
	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_defaultMaterial = _spriteRenderer.material;

		_isBlinking = false;
	}

	private void Start()
	{
		if (damageableObject is Player player)
		{
			player.TakeHit += DamageableObject_TakeHit;
		}
	}

	private void DamageableObject_TakeHit()
	{
		SetBlinkMaterial();
	}

	private void Update()
	{
		if (!_isBlinking)
		{
			return;
		}
		_blinkTimer -= Time.deltaTime;
		if (!(_blinkTimer < 0))
		{
			return;
		}
			
		SetDefaultMaterial();
		StopBlinking();
	}
	
	public void StopBlinking()
	{
		SetDefaultMaterial();
		_isBlinking = false;
	}
	
	private void SetBlinkMaterial()
	{
		_isBlinking = true;
		_blinkTimer = blinkDuration;
		_spriteRenderer.material = blinkMaterial;
	}

	private void SetDefaultMaterial()
	{
		_spriteRenderer.material = _defaultMaterial;
	}


	private void OnDestroy()
	{
		if (damageableObject is Player player)
		{
			player.TakeHit -= DamageableObject_TakeHit;
		}
	}
}