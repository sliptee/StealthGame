using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public float usedSpeed;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    public float smoothMoveTime = 0.1f;
    Vector3 velocity;
    float angle;
    Rigidbody body;
    float inputMagnitude;

    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        usedSpeed = Settings.Instance.PlayerSpeed;
    }

    public void MovePlayer()
    {
        Vector3 inputDirection = Vector3.zero;
        inputDirection = new Vector3(Input.GetAxisRaw("HorizontalMove"), 0, Input.GetAxisRaw("VerticalMove")).normalized;

        inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * Settings.Instance.PlayerTurnSpeed * inputMagnitude);

        velocity = transform.forward * Settings.Instance.PlayerSpeed * smoothInputMagnitude;
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "FinishLine")
            LevelManager.OnAdvancing();
        else if (coll.gameObject.tag == "Pickup")
        {
            Settings.Instance.TemporaryScore += 20;
            ParticleSystem ps = coll.gameObject.GetComponentInChildren<ParticleSystem>();
            ps.Play();
            StartCoroutine(DestroyAfter(ps.main.duration, ps.gameObject));
            ps.gameObject.transform.parent = null;
            Destroy(coll.gameObject);
        }
    }
    private IEnumerator DestroyAfter(float s, GameObject obj)
    {
        yield return new WaitForSeconds(s);
        Destroy(obj);
    }
	// Update is called once per frame
	void Update ()
    {
        MovePlayer();
	}
    void FixedUpdate()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && inputMagnitude != 0) //Om spelaren vill röra på sig och vi inte spelar "run" animationen SÅ
            anim.Play("Run", -1, 0f);

        body.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        body.MovePosition(body.position + velocity * Time.deltaTime);    
    }
}
