using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Properties")] [SerializeField]
        private float moveSpeed = 0f;

        [SerializeField] private float jumpVelocity = 15f;
        [SerializeField] private float fallMultiplier = 5f;
        [SerializeField] private float lowJumpMultiplier = 20f;

        [Header("Ground Properties")] [SerializeField]
        private Transform groundCheck;

        [SerializeField] private float groundCheckRadius = 0.5f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D _rb;
        private bool _isGrounded;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float moveInput = Input.GetAxis("Horizontal");
            Vector2 moveVelocity = new Vector2(moveInput * moveSpeed, _rb.velocity.y);
            _rb.velocity = moveVelocity;

            _isGrounded = CheckIfGrounded();

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpVelocity);
            }

            ApplyGravityModifications();
        }

        private bool CheckIfGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        private void ApplyGravityModifications()
        {
            switch (_rb.velocity.y)
            {
                case < 0:
                    _rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
                    break;
                case > 0 when !Input.GetButton("Jump"):
                    _rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
                    break;
            }
        }
    }
}