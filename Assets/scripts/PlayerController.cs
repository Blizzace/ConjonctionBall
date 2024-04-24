
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using System.Security.Cryptography;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    

    private PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private int nbJumps;
    private int nbJumpsDefault = 2;
    Vector3 pos, velocity;
    [SerializeField]
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction deflectAction;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValueDefault = -9.81f;
    private float gravityValue;
    private bool isGravityDown=false;
    [SerializeField]
    private float gravityValueDownwards= -50.0f;
    [SerializeField]
    private float rotationSpeed = .8f;
    [SerializeField]
    private GameObject DeflectEffect;
    [SerializeField]
    private Material Mat;
    [Networked,SerializeField]
    private Color clr { get; set; }
    
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            deflectAction = playerInput.actions["Deflect"];
            cameraTransform = Camera.main.transform;
            clr = new Color(generaterandomcolor(), generaterandomcolor(), generaterandomcolor());
            deflectAction.performed += _ => Deflect();
            nbJumps = 1;
            pos = transform.position;
            gravityValue = gravityValueDefault;
        }
        Material material = new Material(Mat.shader);
   
        material.color = clr;
        gameObject.GetComponent<MeshRenderer>().material = material;
    }
    private float generaterandomcolor()
    {
        return Random.Range(0f, 1f);
    }
    
 
       




  
   

    

    private void Deflect()
    {
        Instantiate(DeflectEffect, transform);
    }

    public override void FixedUpdateNetwork()
    {
        InputSystem.Update();
        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;
        // Only move own player and not every other player. Each player controls its own player object.
        if (HasStateAuthority == false)
        {
            return;
        }
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Runner.DeltaTime * playerSpeed);
        float targetAngle = cameraTransform.eulerAngles.y;

        if (groundedPlayer) { nbJumps = nbJumpsDefault; }
        // Changes the height position of the player..
        if (jumpAction.triggered && nbJumps!=0)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValueDefault);
            nbJumps--;
        }
        if (!jumpAction.enabled)
        {
            playerVelocity.y = 0;
        }
        //If the player is moving downwards
        if (velocity.y < 0)
        {
            if (!isGravityDown)
            {
                isGravityDown = true;
                //Higher gravity if falling
                gravityValue = gravityValueDownwards;
            }
        }
        else
        {
            isGravityDown = false;
            gravityValue = gravityValueDefault;
        }
        playerVelocity.y += gravityValue * Runner.DeltaTime;
        controller.Move(playerVelocity * Runner.DeltaTime);
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);
    }
}