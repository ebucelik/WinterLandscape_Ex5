using UnityEngine;

public class Snowball : MonoBehaviour
{
    #region Public Fields

    public AnimationCurve scaleOverDistanceTravelled;

    public CustomRenderTexture renderTexture;

    public Rect worldBounds;

    public float force = 3.0f;
    public float rotationForce = 15.0f;
    public float snowRadiusMultiplier = 0.25f;

    public GameObject snowman;

    public Material heightmapMaterial;

    #endregion

    #region Private Fields

    private bool inSnowmanMode = false;

    private float accumulatedDistance;

    private Rigidbody rb;

    private Vector3 prevPosition;

    #endregion

    private static readonly string PlayerPositionProperty = "_PlayerPosition";
    private static readonly string PlayerRadiusProperty = "_PlayerRadius";

    private void Awake()
    {
        this.rb = this.GetComponentInChildren<Rigidbody>();
        this.prevPosition = this.rb.transform.position;

        this.renderTexture.Initialize();
    }

    public Rigidbody GetRigidbody() => this.rb;

    private void CheckSnowmanMode()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.inSnowmanMode = !this.inSnowmanMode;

            this.snowman.transform.position = this.rb.transform.position;
            this.snowman.transform.localScale = this.rb.transform.localScale;

            var camera = FindFirstObjectByType<SnowballCamera>();
            var forward = camera.transform.forward;
            var planeForward = new Vector3(forward.x, 0.0f, forward.z).normalized;

            this.snowman.transform.rotation = Quaternion.LookRotation(-planeForward);

            this.snowman.SetActive(this.inSnowmanMode);
            this.rb.gameObject.SetActive(!this.inSnowmanMode);
        }
    }

    private void Update()
    {
        this.CheckSnowmanMode();
        this.UpdateTexture();
    }

    private void UpdateTexture()
    {
        var playerPos = this.rb.transform.position;
        var relativePos = new Vector2(playerPos.x, playerPos.z) - this.worldBounds.min;
        Vector2 percent = new Vector2(relativePos.x / this.worldBounds.width, relativePos.y  / this.worldBounds.height);

        float radius = this.rb.transform.localScale.x;
        radius /= worldBounds.width;

        this.heightmapMaterial.SetVector(PlayerPositionProperty, new Vector4(1 - percent.x, 1 - percent.y, 0, 0));
        this.heightmapMaterial.SetFloat(PlayerRadiusProperty, radius * this.snowRadiusMultiplier);

        this.renderTexture.Update();
    }

    public void FixedUpdate()
    {

        if (!this.inSnowmanMode)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 forward = this.rb.transform.forward;

            Vector3 forwardVector = forward * vertical;

            var rot = this.rb.transform.rotation;
            var newRot = rot * Quaternion.AngleAxis(horizontal * this.rotationForce * Time.deltaTime, Vector3.up);


            this.rb.AddForce(forwardVector * this.force);
            this.rb.MoveRotation(newRot);

            var pos = this.rb.transform.position;
            float dist = Vector3.Distance(pos, this.prevPosition);
            this.accumulatedDistance += dist;

            this.prevPosition = pos;

            float scale = this.scaleOverDistanceTravelled.Evaluate(this.accumulatedDistance);

            this.rb.gameObject.transform.localScale = Vector3.one * scale;
        }
    }

    private void OnDestroy()
    {
        this.heightmapMaterial.SetVector(PlayerPositionProperty, new Vector4(-0.5f, -0.5f, 0, 0));
        this.heightmapMaterial.SetFloat(PlayerRadiusProperty, 0.0f);

    }

}
