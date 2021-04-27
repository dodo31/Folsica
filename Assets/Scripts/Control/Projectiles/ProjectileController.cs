using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	public float Speed = 1 / 6f;

	public float Damage = 0;

	public AudioClip LaunchAudio;

	public AudioClip HitAudio;

	private AudioSource audioSource;

	private Vector3 direction;

	private Vector3 emitterPosition;

	private float range;

	private float power;

	protected void Awake()
	{
		audioSource = this.GetComponent<AudioSource>();
	}

	public void Emit(Vector3 direction, Vector3 emitterPosition, float range, float powerMultiplicator)
	{
		this.direction = direction;
		this.emitterPosition = emitterPosition;
		this.range = range;
		this.power = Damage * powerMultiplicator;

		transform.position = emitterPosition;

		audioSource.clip = LaunchAudio;
		audioSource.Play();
	}

	protected void LateUpdate()
	{
		float distanceFromEmitter = Vector3.Distance(transform.position, emitterPosition);

		if (distanceFromEmitter <= range)
		{
			Vector3 currentPosition = transform.position;
			float moveAngle = Mathf.Atan2(direction.z, direction.x);
			float orientation = -moveAngle * Mathf.Rad2Deg + 180;

			float newPosX = currentPosition.x + Mathf.Cos(moveAngle) * Speed;
			float newPosY = currentPosition.z + Mathf.Sin(moveAngle) * Speed;

			transform.position = new Vector3(newPosX, currentPosition.y, newPosY);
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, orientation, transform.rotation.eulerAngles.z);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	protected void OnTriggerEnter(Collider otherCollider)
	{
		EnemyController enemy = otherCollider.gameObject.GetComponent<EnemyController>();

		if (enemy != null)
		{
			audioSource.clip = LaunchAudio;
			audioSource.Play();

			enemy.HandleDamage(power);
			Destroy(gameObject);
		}
	}
}