using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> Waypoints;
    private int i, j;
    private Vector3 dir;
    private AStar pathfinder;
    public List<Vector2> path;
    GridManager grid;
    public static event System.Action PlayerSeen;
    private Transform player;
    private float seenPlayerTimer;
    internal float timeUntilPlayerSpotted = .5f;
    internal Light splight;
    Color originalSpotlightColour;
    RaycastHit hit;
    Vector3 playerPos, pos, target; //Positioner som inte är känsliga för y-axeln
    internal float speed;

    private float viewDistance
    {
        get { return splight.range; }
    }
    private float spotAngle
    {
        get { return splight.spotAngle; }
    }
    private bool CanSeePlayer()
    {
        playerPos = new Vector3(player.position.x, 1, player.position.z);
        pos = new Vector3(transform.position.x, 1, transform.position.z);
        if (Vector3.Distance(pos, playerPos) < viewDistance)
        {
            Vector3 dirToPlayer = (playerPos - pos).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleToPlayer < spotAngle / 2f)
            {
                if (Physics.Raycast(pos, dirToPlayer, out hit, 10))
                {
                    if (hit.collider.gameObject.tag != "Player")
                        return false;
                }
                return true;
            }
        }
        return false;
    }

    protected virtual void Start()
    {
        timeUntilPlayerSpotted = Settings.Instance.TimeUntilPlayerSpotted;
        splight = transform.GetComponentInChildren<Light>();
        originalSpotlightColour = splight.color;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        grid = GameObject.Find("AStar").GetComponent<GridManager>();
        path = new List<Vector2>();
        pathfinder = GameObject.FindGameObjectWithTag("AStar").GetComponent<AStar>();
        speed = Settings.Instance.EnemySpeed;
        StartCoroutine(Walk());
    }
    void Update()
    {
        if (CanSeePlayer())
        {
            seenPlayerTimer += Time.deltaTime;
        }
        else if (seenPlayerTimer > 0)
        {
            seenPlayerTimer -= Time.deltaTime;
        }
        seenPlayerTimer = Mathf.Clamp(seenPlayerTimer, 0, timeUntilPlayerSpotted);
        splight.color = Color.Lerp(originalSpotlightColour, Color.red, seenPlayerTimer / timeUntilPlayerSpotted);

        if (seenPlayerTimer >= timeUntilPlayerSpotted)
        {
            if (PlayerSeen != null)
            {
                PlayerSeen();
            }
        }
        if(dir.magnitude != 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * Settings.Instance.EnemyTurnSpeed);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
    public IEnumerator Walk()
    {
        while (true)
        {
            i = 0;
            while (i < Waypoints.Count)
            {
                j = 0;
                path = pathfinder.FindPath(transform.position, Waypoints[i]);
                if (path == null || path.Count == 0)
                {
                    Debug.Log("Cannot find a path to waypoint " + i);
                    i++;            
                    continue;
                }
                while (j < path.Count) //När vi inte har nått waypointet
                {
                    Node n = grid.grid[(int)path[j].x, (int)path[j].y];

                    target = new Vector3(n.WorldPos.x, 1, n.WorldPos.z);
                    dir = (target - transform.position).normalized; //Riktning

                    if (grid.PositionToGrid(transform.position) == grid.PositionToGrid(target))  //Om vi når denna tile i pathen, forstätt med nästa
                        j++;

                    yield return new WaitForSeconds(0.02f);
                }
                if (grid.PositionToGrid(transform.position) == grid.PositionToGrid(Waypoints[i]))
                    i++;
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
    void OnDrawGizmos()
    {
        if (!UnityEditor.Selection.Contains(gameObject)) //Visa bara gizmon om det här objektet är valt i hierarkin
            return;
        Vector3 pos = transform.position;
        foreach(Vector3 waypoint in Waypoints)
        {
            Gizmos.DrawLine(pos, waypoint);
            pos = waypoint;
        }
    }
    // Whenever the object gets destroyed.

}
