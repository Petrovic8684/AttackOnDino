using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private Transform groundCotact;
    [SerializeField] private LayerMask groundLayer;

    private bool isFacingRight;
    private float horizontalMovement;
    private Rigidbody2D rigidbody2d;
    private Animator animator;

    // Inicijalizacije promenljivih koje nisu vidljive u inspektoru
    private void Awake()
    {
        isFacingRight = true;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update se izvrsava svaki frejm ( npr. 60 puta u sekundi)
    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal"); // -1 kada korisnik pritisne A, 1 kada pritisne D na tastaturi

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) // Ako korisnik pritisne SPACE na tastaturi i ako je igrac na zemlji
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpPower); // Dodaj brzinu po Y osi
        }

        Animate();
        Flip();
    }

    // FixedUpdate je isto sto i Update, samo se koristi pri iterakciji sa sistemom za fiziku
    private void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector2(horizontalMovement * moveSpeed, rigidbody2d.velocity.y); // Podesi brzinu po X osi
    }

    private bool IsGrounded()
    {
        /*
        Proverava da li je igrac na zemlji tako sto generise
        mali krug ispod njegovih nogu. Ako taj krug ima preseka
        sa bilo kojim objektom koji je na sloju "groundLayer", ova
        metoda vraca true. U suprotnom vraca false.
        */

        return Physics2D.OverlapCircle(groundCotact.position, 0.2f, groundLayer);
    }

    private void Animate()
    {
        if (Mathf.Abs(horizontalMovement) > 0.1f) // Ako se igrac krece ulevo ili udesno
        {
            animator.SetBool("IsWalking", true); // Pusti animaciju za hod
        }
        else
        {
            animator.SetBool("IsWalking", false); // U suprotnom, pusti idle animaciju
        }
    }

    // Metoda Flip okrece igraca ulevo ili udesno, u zavisnosti od toga u koju stranu se krece
    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f) // Ako je igrac promenio smer kretanja
        {
            isFacingRight = !isFacingRight;

            /*
            Obicna zamena vrednosti dve promenljve. Menjamo
            predznak transform.localScale.x polju, na indirektan nacin jer
            je to polje Vector3 strukture, a ta polja su pojedinacno gledano read-only,
            tako da kada zelimo da ih menjamo, moramo da promenimo celu strukturu, kao sto
            sam ja to uradio ovde. Npr, nije bilo moguce uraditi transform.localScale.x *= -1.
            */

            Vector3 temp = transform.localScale;
            temp.x *= -1f;
            transform.localScale = temp;
        }
    }
}
