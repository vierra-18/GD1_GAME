using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement")]
    public float movespeed;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
	private void FixedUpdate()
	{
        MyInput();
	}
	private void MyInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
	}
    private void MovePlayer() {
        moveDirection = orientation.forward* verticalInput+orientation.right*horizontalInput;

        rb.AddForce(moveDirection.normalized*movespeed*10f, ForceMode.Force);
    }
}
