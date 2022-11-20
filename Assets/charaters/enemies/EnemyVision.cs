using UnityEngine;
using Game.System.Events.Player;
using Game.Utils.Mono;

public class EnemyVision : Subscriber<StanceChange, MovementSpeedChange>
{
    [Header("Standing")]
    [SerializeField] float standingMultiplier = 2f;
    [SerializeField] float standingMaxDistance = 5f;

    [Header("Crouching")]
    [SerializeField] float crouchingMultiplier = 1f;
    [SerializeField] float crouchingMaxDistance = 4f;

    [Header("Crawling")]
    [SerializeField] float crawlingMultiplier = 0.5f;
    [SerializeField] float crawlingMaxDistance = 3f;

    [Header("State Colors")]
    [SerializeField] Color normal = new(0.05f, 0.55f, 0.15f);
    [SerializeField] Color aware = new(0.95f, 0.6f, 0.2f);
    [SerializeField] Color alert = new(0.75f, 0f, 0f);

    [Header("Other Settings")]
    [SerializeField] float speedMultiplier = 5f;
    [SerializeField] float distanceMultiplier = 10f;
    [SerializeField] string playerTag = "Player";
    [SerializeField] LayerMask layerMask;

    GameSession session;
    GameObject player;
    Transform target;
    StanceChange.Stance playerStance = StanceChange.Stance.STANDING;
    SpriteRenderer sprite;
    float minPlayerSpeed = 0.1f;
    float playerSpeed = 0.1f;
    float seeMeter = 0f;
    bool playerInFOV = false;

    void Start()
    {
        Init();
        session = FindObjectOfType<GameSession>();
        player = session.GetPlayer();
        target = transform.parent.transform;
        sprite = target.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateSeeMeter();
        UpdateTint();
    }

    void UpdateSeeMeter()
    {
        if (playerInFOV)
        {
            if (LineOfSite(out RaycastHit hit))
            {
                AdjustSeeMeter(CalculateSeeMeterChange(hit) * Time.deltaTime);
            }
            else AdjustSeeMeter(-Time.deltaTime);
        }
        else seeMeter = Mathf.Clamp(seeMeter - Time.deltaTime, 0f, 1f);
    }

    void UpdateTint()
    {
        sprite.color = Color.Lerp(
            new Color(0.05f, 0.55f, 0.15f),
            new Color(0.75f, 0f, 0f),
            seeMeter);
    }

    bool LineOfSite(out RaycastHit hit) =>
        Physics.Raycast(
            target.position,
            (player.transform.position - target.position).normalized,
            out hit,
            1000f,
            layerMask);

    void AdjustSeeMeter(float amount)
    {
        seeMeter = Mathf.Clamp(seeMeter + amount, 0f, 1f);
    }

    float CalculateSeeMeterChange(RaycastHit hit)
    {
        float distance = hit.distance;
        float amount = 1f;

        if (playerStance == StanceChange.Stance.STANDING)
        {
            if (distance > standingMaxDistance) return -1f;
            amount *= standingMultiplier;
        }
        else if (playerStance == StanceChange.Stance.CROUCHING)
        {
            if (distance > crouchingMaxDistance) return -1f;
            amount *= crouchingMultiplier;
        }
        else
        {
            if (distance > crawlingMaxDistance) return -1f;
            amount *= crawlingMultiplier;
        }
        amount *= playerSpeed * speedMultiplier;

        return amount / (distance * distanceMultiplier);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag) playerInFOV = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == playerTag) playerInFOV = false;
    }

    public override void OnEvent(StanceChange e) => playerStance = e.stance;
    public override void OnEvent(MovementSpeedChange e) =>
        playerSpeed = Mathf.Clamp(e.speed, minPlayerSpeed, float.MaxValue);

    void OnDestroy() => Destroy();
}
