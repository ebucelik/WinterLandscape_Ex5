using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballCamera : MonoBehaviour
{
    #region Public Fields

    public float followSpeed = 0.1f;
    public float rotationSpeed = 1.0f;

    public Snowball snowball;

    #endregion

    #region Private Fields


    private Vector3 offset;

    #endregion

    private void Start()
    {
        this.offset = this.snowball.transform.position - this.transform.position;
    }

    void Update()
    {
        float scale = this.snowball.GetRigidbody().transform.localScale.x;

        var targetDir = -this.snowball.GetRigidbody().transform.TransformDirection(this.offset * scale);
        var targetPosition = this.snowball.GetRigidbody().transform.position + targetDir;



        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * this.followSpeed);
        this.transform.forward = Vector3.Lerp(this.transform.forward, -targetDir.normalized, Time.deltaTime * this.rotationSpeed);
    }
}
