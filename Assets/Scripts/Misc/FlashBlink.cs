using UnityEngine;

public class FlashBlink : MonoBehaviour
{
	[SerializeField] private MonoBehaviour _damagedleObject;
	[SerializeField] private Material _blinlMaterial;
	[SerializeField] private float _blinkDuration = 0.2f;

	private float blinkTimer;
	private Material defaultMaterial;
	private SpriteRenderer spriteRenderer;
	private bool isBlinking;
	
	/// <summary>
	/// Запускавая
	/// </summary>
	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		defaultMaterial = spriteRenderer.material;

		isBlinking = false;
	}

	private void Start()
	{
		if (_damagedleObject is Player)
		{
			(_damagedleObject as Player).TakeHit += DamagableObject_TakeHit;
		}
	}

	private void DamagableObject_TakeHit()
	{
		SetBlinkMaterial();
	}

	void Update()
	{
		if (isBlinking)
		{
			blinkTimer -= Time.deltaTime;
			if (blinkTimer < 0)
			{
				SetDefaultMaterial();
				StopBlinking();
			}
		}
	}

	private void SetBlinkMaterial()
	{
		isBlinking = true;
		blinkTimer = _blinkDuration;
		spriteRenderer.material = _blinlMaterial;
	}

	private void SetDefaultMaterial()
	{
		spriteRenderer.material = defaultMaterial;
	}

	public void StopBlinking()
	{
		SetDefaultMaterial();
		isBlinking = false;
	}

	private void OnDestroy()
	{
		if (_damagedleObject is Player)
		{
			(_damagedleObject as  Player).TakeHit -= DamagableObject_TakeHit;
		}
	}
}