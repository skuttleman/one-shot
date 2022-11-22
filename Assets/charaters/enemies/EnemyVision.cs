using UnityEngine;
using Game.System.Events.Player;
using Game.Utils.Mono;
using Game.System.Events;
using Game.System.Events.Enemy;

public class EnemyVision :
    Subscriber<Event<PlayerStance>, PlayerMovementSpeedChange, EnemyCanSeePlayer<EnemyVision>> {
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
    [SerializeField] Color alert = new(0.75f, 0f, 0f);

    [Header("Other Settings")]
    [SerializeField] float speedMultiplier = 5f;
    [SerializeField] float distanceMultiplier = 10f;
    [SerializeField] string playerTag = "Player";
    [SerializeField] LayerMask layerMask;

    GameSession session;
    GameObject player;
    Transform target;
    PlayerStance playerStance = PlayerStance.STANDING;
    SpriteRenderer sprite;
    float minPlayerSpeed = 0.1f;
    float playerSpeed = 0.1f;
    float seeMeter = 0f;

    new void Start() {
        base.Start();
        session = FindObjectOfType<GameSession>();
        player = session.GetPlayer();
        target = transform.parent.transform;
        sprite = target.gameObject.GetComponent<SpriteRenderer>();
    }

    new void Update() {
        base.Update();
        AdjustSeeMeter(-Time.deltaTime);
        UpdateTint();
    }

    void UpdateTint() {
        sprite.color = Color.Lerp(
            normal,
            alert,
            seeMeter);
    }

    bool LineOfSite(out RaycastHit hit) =>
        Physics.Raycast(
            target.position,
            (player.transform.position - target.position).normalized,
            out hit,
            1000f,
            layerMask);

    void AdjustSeeMeter(float amount) {
        seeMeter = Mathf.Clamp(seeMeter + amount, 0f, 1f);
    }

    float CalculateSeeMeterChange(float distance) {
        float amount = 1f;

        if (playerStance == PlayerStance.STANDING) {
            if (distance > standingMaxDistance) return -1f;
            amount *= standingMultiplier;
        } else if (playerStance == PlayerStance.CROUCHING) {
            if (distance > crouchingMaxDistance) return -1f;
            amount *= crouchingMultiplier;
        } else {
            if (distance > crawlingMaxDistance) return -1f;
            amount *= crawlingMultiplier;
        }
        amount *= playerSpeed * speedMultiplier;

        return amount / (distance * distanceMultiplier);
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag(playerTag)
            && LineOfSite(out RaycastHit hit)
            && hit.collider == other) {
            pubsub.Publish(new EnemyCanSeePlayer<EnemyVision>(
                this,
                gameObject.transform.position,
                hit.point,
                hit.distance));
        }
    }

    public override void OnEvent(Event<PlayerStance> e) => playerStance = e.data;
    public override void OnEvent(PlayerMovementSpeedChange e) =>
        playerSpeed = Mathf.Clamp(e.data, minPlayerSpeed, float.MaxValue);
    public override void OnEvent(EnemyCanSeePlayer<EnemyVision> e) {
        if (e.enemy == this)
            AdjustSeeMeter(CalculateSeeMeterChange(e.distance));
    }
}
