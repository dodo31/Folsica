public class TowerCoreController : TowerStageController
{
	private const float ROTATION_SPEED = 0.05f;

	public float Range = 0;

	private EnemyController currentTarget;


	public EnemyController CurrentTarget { get => currentTarget; set => currentTarget = value; }
}