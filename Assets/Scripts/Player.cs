using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject Sonido; 

    [SerializeField]
    TextMeshProUGUI textMesh;

    [SerializeField]
    float moveSpeed = 2f;

    [SerializeField]
    float JumpForce = 5f;
    Rigidbody rb;

    [SerializeField]
    Color rayColor = Color.magenta;

    [SerializeField, Range(0.1f,10f)]
    float rayDistance = 5;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    Transform rayTransform;

    Animator anim;

    int score;

    GameInputs gameInputs;
    
    void Awake(){
        gameInputs = new GameInputs();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
      gameInputs.Land.Jump.performed+= _=> Jump();
    }

    void OnEnable(){
        gameInputs.Enable();
    }
    
    void OnDisable(){
        gameInputs.Disable();
    }

    void Jump()
    {
        if(isGrounding)
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse );
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color=rayColor;
        Gizmos.DrawRay(transform.position, -transform.up * rayDistance);    
    }

    // Update is called once per frame
    void Update()
    {
        //movement 3d
        Movement();
        anim.SetFloat("movement", AxisMagnitudeAbs);
    }

    void Movement()
    {
        if(IsMoving)
        {
            transform.Translate( Vector3.forward * Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.LookRotation(new Vector3(Axis.x,0f,Axis.y));
        }
    }

    /// <summary>
    /// Retunrs the axis with H input and V Input.
    /// </summary>
    /// <returns></returns>
    Vector3 Axis => gameInputs.Land.Move.ReadValue<Vector2>();

    /// <summary>
    /// Check if player is moving with inputs H and V.
    /// </summary>
    bool IsMoving => AxisMagnitudeAbs > 0;

    /// <summary>
    /// Returns the magnitude of the Axis with inputs H and V.
    /// </summary>
    /// <returns></returns>
    float AxisMagnitudeAbs => Mathf.Abs(Axis.magnitude);

    bool isGrounding => Physics.Raycast(rayTransform.position, -transform.up, rayDistance,groundLayer);

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Collectable"))
        {
            Instantiate(Sonido);
            score++;
            textMesh.text = $"Score: {score}";
            Destroy(other.gameObject);
        }   
    }
}