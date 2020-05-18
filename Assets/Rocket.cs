using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource rocketShipAudioSource;
    // Start is called before the first frame update
    void Start()
    {
      rigidbody = GetComponent<Rigidbody>();
      rocketShipAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        processInput();
    }

    private void processInput()
    {
      if(Input.GetKey(KeyCode.Space))
      {
        rigidbody.AddRelativeForce(Vector3.up);
        if(!rocketShipAudioSource.isPlaying)
        {
          rocketShipAudioSource.Play();
        }

      }
      else
      {
        rocketShipAudioSource.Stop();
      }

      if(Input.GetKey(KeyCode.A))
      {
        transform.Rotate(Vector3.left);
      }
      else if(Input.GetKey(KeyCode.D))
      {
        transform.Rotate(Vector3.right);
      }
    }

}
