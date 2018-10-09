using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = "OnServerNameChange")] string serverName;
    [SerializeField] private GameObject nameTextPrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float rotationSpeed = 1.0f;

    private Vector2 movement;
    private Vector2 lastMovement;

    private NameText nameText;
    private Rigidbody rigidbody;

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

    [TargetRpc]
    public void TargetMoveToServer(NetworkConnection connection, int serverID)
    {
        Handler.NetworkManager.ClientMoveServer(serverID);
    }

    [Command]
    public void CmdSetMovement(Vector2 movement)
    {
        Rigidbody.velocity = transform.forward * movement.x * movementSpeed;
        Rigidbody.angularVelocity = new Vector3(0, movement.y * rotationSpeed, 0);
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

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Start()
    {
        if (isServer)
        {
            serverName = "Testing";
        }

        if (isLocalPlayer)
        {
            GameObject cameraObject = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);

            SmoothFollow smoothFollow = cameraObject.GetComponent<SmoothFollow>();
            smoothFollow.target = transform;

            movement = Vector2.zero;
            lastMovement = Vector2.zero;

            StartCoroutine(UpdateMovement());
        }
    }

    public void Update()
    {
        if (isLocalPlayer)
        {
            movement.x = Mathf.Round(Input.GetAxis("Vertical"));
            movement.y = Mathf.Round(Input.GetAxis("Horizontal"));
        }
    }

    private void OnServerNameChange(string name)
    {
        nameText.SetText(name);
    }

    public override void OnStartClient()
    {
        // Create floating text.
        GameObject textObject = Instantiate(nameTextPrefab, new Vector3(0, 2.5f, 0), Quaternion.identity);

        // Make object child of this.
        textObject.transform.parent = transform;

        // Get text name component.
        nameText = textObject.GetComponent<NameText>();
    }
}
