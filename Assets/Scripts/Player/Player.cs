using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = "OnServerNameChange")] string serverName;
    [SyncVar(hook = "OnScoreChange")] int score;
    [SerializeField] private GameObject nameTextPrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private GameObject gameUIPrefab;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float rotationSpeed = 1.0f;

    private Vector2 movement;
    private Vector2 lastMovement;

    private NameText nameText;
    private Rigidbody rigidbody;
    private Animator animator;
    private GameUI gameUI;

    public Rigidbody Rigidbody
    {
        get
        {
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody>();
            }

            return rigidbody;
        }
    }

    public Animator Animator
    {
        get
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            return animator;
        }
    }

    [TargetRpc]
    public void TargetMoveToServer(NetworkConnection connection, int fromServerID, int toServerID)
    {
        Handler.NetworkManager.ClientMoveServer(fromServerID, toServerID);
    }

    [Command]
    public void CmdSetMovement(Vector2 movement)
    {
        this.movement = movement;
    }

    [Command]
    public void CmdSetPlayerData(string username, int score, int lastServerID)
    {
        // Assign client values.
        serverName = username;
        this.score = score;

        if (Handler.NetworkManager.playerSpawns.ContainsKey(lastServerID))
        {
            Transform spawnPoint = Handler.NetworkManager.playerSpawns[lastServerID];

            Rigidbody.MovePosition(spawnPoint.position);
            transform.rotation = spawnPoint.rotation;
        }
    }

    public void AddScore(int amt)
    {
        score += amt;
    }

    private IEnumerator NextConnection()
    {
        yield return new WaitForSeconds(5.0f);

        yield break;
    }

    private IEnumerator UpdateMovement()
    {
        while (true)
        {
            yield return new WaitUntil(() => movement != lastMovement);

            CmdSetMovement(movement);

            lastMovement = movement;
        }
    }

    private IEnumerator UpdatePlayer()
    {
        while (true)
        {
            Rigidbody.velocity = transform.forward * movement.x * movementSpeed;

            if (movement.y != 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(transform.up * movement.y * rotationSpeed);
                transform.rotation = transform.rotation * deltaRotation;
            }

            yield return new WaitForSeconds(0.08f);
        }
    }
    
    public void MoveToServer(int serverID)
    {
        TargetMoveToServer(connectionToClient, Handler.ServerID, serverID);
    }

    public void Start()
    {
        if (isServer)
        {
            serverName = "NULL";
            score = 0;

            StartCoroutine(UpdatePlayer());
        }

        if (isLocalPlayer)
        {
            PlayerData data = Handler.Instance.PlayerData;

            CmdSetPlayerData(data.Username, data.Score, data.LastServerID);

            // Create our camera object for the player.
            GameObject cameraObject = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);

            // Force camera to follow player.
            SmoothFollow smoothFollow = cameraObject.GetComponent<SmoothFollow>();
            smoothFollow.target = transform;

            // Assign movement vectors.
            movement = Vector2.zero;
            lastMovement = Vector2.zero;

            StartCoroutine(UpdateMovement());
        }
    }


    public void Update()
    {
        if (isLocalPlayer)
        {
            float x = Input.GetAxis("Vertical"),
                y = Input.GetAxis("Horizontal");

            if (Mathf.Abs(x) > 0.3f)
            {
                x = x / Mathf.Abs(x);
            }
            else
            {
                x = 0;
            }

            if (Mathf.Abs(y) > 0.3f)
            {
                y = y / Mathf.Abs(y);
            }
            else
            {
                y = 0;
            }

            movement = new Vector2(x, y);
        }

        if (isClient)
        {
            float localVelocity = Vector3.Dot(Rigidbody.velocity, transform.forward);

            // Check if we are moving.
            if (Mathf.Abs(localVelocity) > 0.8f)
            {
                Animator.SetBool("IsMoving", true);
            }
            else
            {
                Animator.SetBool("IsMoving", false);
            }
        }
    }

    private void OnServerNameChange(string name)
    {
        nameText.SetText(name);
    }

    private void OnScoreChange(int score)
    {
        Handler.Instance.PlayerData.Score = score;
        
        if (gameUI != null)
        {
            gameUI.SetScoreText(score);
        }
    }

    public override void OnStartClient()
    {
        // Create floating text.
        GameObject textObject = Instantiate(nameTextPrefab, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);

        // Make object child of this.
        textObject.transform.parent = transform;

        // Get text name component.
        nameText = textObject.GetComponent<NameText>();

        // Set initial state of name.
        nameText.SetText(serverName);

        GameObject gameUIObject = Instantiate(gameUIPrefab);

        gameUI = gameUIObject.GetComponent<GameUI>();
    }
}
