using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Stalker : Combatant
{
    [Header("Stalker Attributes")]
    public float strikeApplyHit = 10;
    public float attackInterval = 1f;
    public float travelSpeed = 5;
    public float strikeRange = 2.5f;

    [SerializeField] private AudioClip[] voiceClips;
    private float voiceTimer;
    private float nextVoiceDelay;
    private Animator animator;
    private AudioSource audioSource;
    private float targetDistance => Vector3.Distance(target.transform.position, transform.position);
    private float attackTimer;
    private Protagonist target;
    private NavMeshAgent agent;

    protected override void OnStart()
    {
        base.OnStart();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = travelSpeed;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Protagonist>();
        nextVoiceDelay = Random.Range(5, 10);
    }

    private void FixedUpdate()
    {
        if (agent.enabled)
        {
            agent.destination = target.transform.position;
            animator.SetFloat("speed", agent.velocity.magnitude);
        }
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (strikeRange > targetDistance && attackTimer > attackInterval) Strike();
        PlayAmbientVoice();
    }

    public void Strike()
    {
        if (Mendth <= 0) return;

        attackTimer = 0;
        target.ApplyHit(strikeApplyHit);
        PlayAmbientVoice();
        PlayStrikeVoice();
        animator.Play("Attack");
    }

    public override void Perish()
    {
        agent.enabled = false;
        gameObject.layer = 0;
        SessionDirector.Instance.ReleaseStalkerSlot();
        animator.Play(Random.Range(0, 10) > 7 ? "Deadflip" : "Dead");
        Destroy(gameObject, 0.7f);
    }

    private void OnDestroy()
    {
        Instantiate(Resources.Load<GameObject>("Effects/Zombie Dead"), transform.position + Vector3.down * 0.8f, Quaternion.Euler(new Vector3(270, 0, 0) + transform.rotation.eulerAngles));
    }

    private void PlayAmbientVoice()
    {
        voiceTimer += Time.deltaTime;
        if (voiceTimer < nextVoiceDelay) return;
        audioSource.clip = voiceClips[Random.Range(0, voiceClips.Length)];
        audioSource.Play();
        nextVoiceDelay = Random.Range(5, 25);
        voiceTimer = 0;
    }

    private void PlayStrikeVoice()
    {
        string[] clipNames = { "hurt-1", "hurt-2", "hurt-3", "hurt-4", "hurt-5", "hurt-6", "hurt-7" };
        SessionDirector.Instance.PlayCue(clipNames[Random.Range(0, clipNames.Length)]);
    }
}
