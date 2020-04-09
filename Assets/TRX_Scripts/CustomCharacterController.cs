using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Ce Controller est fait pour être utilisé avec un Rigidbody2D et donc tirer parti de la Physique d'Unity.
    Il est conçu pour un set de mécanique et un gamefeel spécifique au projet RUINS. Pour le bon fonctionnement 
    des presets du script , les paramètres du Rigidbody2D doivent êtres réglés à Mass = 1.3 et Gravity Scale = 4*/

public class CustomCharacterController : MonoBehaviour
{
    // Déclarations
    public enum GroundType
    {
        ground,
        wallJump,
        none
    }
    public Animator animator;

    bool jumpInput = false;
    float hKeyInput;
    bool isJumping = false;
    bool isFalling = false;
    bool canImpulse = false;
    bool enableMouv = true;
    bool unfreeze = false;


    [SerializeField] float acceleration = 0.0f;
    [SerializeField] float maxSpeed = 0.0f;
    [SerializeField] float jumpForce = 12.0f;
    [SerializeField] float jumpHForce = 4.0f;
    [SerializeField] float impulseHForce = 6.0f;
    [SerializeField] float impulseForce = 12.0f;
    float speedInfo;

    SpriteRenderer playerSprite;
    Rigidbody2D playerRigidbody;
    BoxCollider2D playerCollider;

    LayerMask groundLayer;
    LayerMask wallJumpLayer;
    GroundType groundType;

    Vector2 movementInput;
    Vector2 velocity;
    Vector2 impulseDir = new Vector2(0f, 0f);
    Vector2 jumpDir = new Vector2(0f, 0f);

    RaycastHit2D[] rGroundCast;
    RaycastHit2D[] rWallCast;

    void Start()
    {
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerCollider = gameObject.GetComponent<BoxCollider2D>();

        groundLayer = LayerMask.GetMask("Ground");
        wallJumpLayer = LayerMask.GetMask("WallJump");

       rGroundCast = new RaycastHit2D[10];
       rWallCast = new RaycastHit2D[10];

    }
    
    void Update()
    {
        InputCheck();
        animator.SetFloat("Speed", Mathf.Abs(speedInfo));

    }
    
    void FixedUpdate()
    {
        GroundingUpdate();
        VelocityUpdate();
        ImpulseDirection();
        JumpUpdate();
        WallJump();
    }

    // Méthodes

    void InputCheck()
    {

        //Détection du mouvement horizontal
        hKeyInput = Input.GetAxisRaw("Horizontal");
        movementInput = new Vector2(hKeyInput, 0);

        //Détection du Saut
        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

    }

    void ImpulseDirection()
    {
        //Change la direction du vecteur utilisé pour l'impulsion (ImpulseDir) en fonction de l'input directionnel (hKeyInupt)
        if (hKeyInput > 0)
        {
            impulseDir = new Vector2(impulseHForce, impulseForce);
        }
        else if (hKeyInput < 0)
        {
            impulseDir = new Vector2(-impulseHForce, impulseForce);
        }
        else
        {
            impulseDir = new Vector2(0f, impulseForce);
        }

        //De même pour la direction du vecteur de saut (JumpDirection)

        if (hKeyInput > 0)
        {
            jumpDir = new Vector2(jumpHForce, jumpForce);
        }
        else if (hKeyInput < 0)
        {
            jumpDir = new Vector2(-jumpHForce, jumpForce);
        }
        else
        {
            jumpDir = new Vector2(0f, jumpForce);
        }
    }

    void VelocityUpdate()
    {

        velocity = playerRigidbody.velocity; //Donne la vélocité du rigidbody du player à une variable locale (velocity) pour pouvoir la modifier
        speedInfo = playerRigidbody.velocity.x; //Renvoie la vitesse du player pour pouvoir l'utiliser dans l'animation

        if (hKeyInput == 0) //supprime l'inertie
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, 0.85f);
            playerRigidbody.velocity = velocity;
        }
        else if (enableMouv)
        {
            //Applique les changements voulus à la vélocité
            velocity += movementInput * Time.fixedDeltaTime * acceleration; 

            movementInput = Vector2.zero;//L'input de mouvement a été consommé, on le réinitialise

            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

            //Tourne le sprite dans la direction vers laquelle on se dirige
            if (Input.GetAxis("Horizontal") < 0f) 
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (Input.GetAxis("Horizontal") > 0f)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            
            //Smoothing collision with walls
            int numHits = playerCollider.Cast(-Vector2.left, rWallCast, 0.3f);
            if (numHits > 0) velocity += new Vector2 (-1.2f,0);

            int leftNumHits = playerCollider.Cast(Vector2.left, rWallCast, 0.3f);
            if (leftNumHits > 0) velocity += new Vector2(1.2f, 0); ;

            if (isFalling) velocity.x = 0.85f * velocity.x;

            //Rend la vélocité au rigidbody du player
            playerRigidbody.velocity = velocity;
                               
        }
    }

    void JumpUpdate()
    {
        //Mise à jour du marqueur de chute 
        if (isJumping && playerRigidbody.velocity.y < 0)
        {
            isFalling = true;
        }

        //Saut de base
        if (jumpInput && groundType == GroundType.ground)
        {

            playerRigidbody.AddForce(jumpDir, ForceMode2D.Impulse); //Remplacer le simple Add force par quelque chose avec plus de contrôle.
            animator.SetBool("isJumping", true);
            //Mise à jour des marqueurs après le saut
            jumpInput = false;//L'input de saut à été consommé, on le réinitialise
            isJumping = true;
            canImpulse = true;

        }

        //WallJump
        else if (jumpInput && groundType == GroundType.wallJump)
        {
            //Setup des raycasts pour détecter les murs de WallJump à gauche ou à droite
            int wallHitsLeft = playerCollider.Raycast(Vector2.left, rWallCast, 1.3f, wallJumpLayer);
            int wallHitsRight = playerCollider.Raycast(-Vector2.left, rWallCast, 1.3f, wallJumpLayer);

            //Fluidifie le wallJump
            if (wallHitsLeft > 0 && hKeyInput < 0)
            {
                jumpDir = new Vector2(jumpDir.x, 0);
                playerRigidbody.AddForce(jumpDir, ForceMode2D.Impulse); //Remplacer le simple Add force par quelque chose avec plus de contrôle.

                jumpInput = false;

                isJumping = true;
                canImpulse = true;
            }
            else if (wallHitsRight > 0 && hKeyInput > 0)
            {
                jumpDir = new Vector2(jumpDir.x, 0);
                playerRigidbody.AddForce(jumpDir, ForceMode2D.Impulse); //Remplacer le simple Add force par quelque chose avec plus de contrôle.

                jumpInput = false;

                isJumping = true;
                canImpulse = true;
            }
            else
            {
                playerRigidbody.AddForce(jumpDir, ForceMode2D.Impulse); //Remplacer le simple Add force par quelque chose avec plus de contrôle.
                jumpInput = false;

                isJumping = true;
                canImpulse = true;
            }
        }

        //Impulsion après un Saut
        else if (jumpInput && canImpulse)
        {

            //freeze position
            enableMouv = false;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            StartCoroutine("WaitandImpulse");//Démarage de la coroutine qui permet le freeze

            if (unfreeze/*Condition validée dans la coroutine après le temps de freeze*/)
            {
                //unfreeze position
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                //Impulsion
                velocity = playerRigidbody.velocity;
                velocity = impulseDir;
                playerRigidbody.velocity = velocity;

                //Mise à jour des marqueurs après l'impulsion
                enableMouv = true;
                jumpInput = false;

                isJumping = true;
                canImpulse = false;
                StopCoroutine("WaitandImpulse");//On stoppe la coroutine (sinon elle continue à tourner)
            }
        }

        //Aterissage
        else if (isJumping && groundType != GroundType.none)
        {
            isJumping = false;
            isFalling = false;
            unfreeze = false;
        }
    }
    
    void WallJump()
    {
        //Ralentissement sur un WallJump
        if (groundType == GroundType.wallJump)
        {
            //bug : saut sur le mur pour remonter quand aucun input directionnel pressé
            velocity = playerRigidbody.velocity;
            if (!isJumping) velocity.y = velocity.y * 0.75f;
            playerRigidbody.velocity = velocity;
        }
    }



    void GroundingUpdate()
    {
        //Setup des raycast pour mettre à jour le grounding
        int numHits = playerCollider.Cast(-Vector2.up, rGroundCast, 0.1f);
        int wallHitsLeft = playerCollider.Raycast(Vector2.left, rWallCast, 1.3f, wallJumpLayer);
        int wallHitsRight = playerCollider.Raycast(-Vector2.left, rWallCast, 1.3f, wallJumpLayer);

        //Grounding au sol
        if (numHits > 0)
            groundType = GroundType.ground;
       
        //Grounding sur un mur de WallJump
        else if (wallHitsLeft > 0 || wallHitsRight > 0)
        {
            print("touché");
            groundType = GroundType.wallJump;
        }
        //Pas de Grounding (vide)
        else
            groundType = GroundType.none;
    }
    //Coroutine de freeze avant l'Impulse
    IEnumerator WaitandImpulse()
    {
        yield return new WaitForSeconds(0.14f);
        unfreeze = true;
    }

    }