using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip engineTurnedOff;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip success;



    Rigidbody rigidbody;
    AudioSource rocketShipAudioSource;

    enum State {Alive,Dying,Transcending}
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {

      rigidbody = GetComponent<Rigidbody>();
      rocketShipAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //todo if state.Dying

        if(state == State.Alive)
        {
          thrust();
          rotate();
        }
        else
        {
          //rocketShipAudioSource.Stop();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
      //print("collided");
      if(state != State.Alive)
      {
        return;
      }

      switch (collision.gameObject.tag)
      {
        case "Friendly":
          print("Ok");
          break;
        case "Finish":
          print("Hit Finish");
          state = State.Transcending;
          rocketShipAudioSource.Stop();
          rocketShipAudioSource.PlayOneShot(success);
          Invoke("loadNextLevel",1f);
          break;
        default:
          print("Dead");
          state = State.Dying;
          rocketShipAudioSource.Stop();
          rocketShipAudioSource.PlayOneShot(crashSound);
          Invoke("restartLevel",1f);
          break;
          //blast the rocket
      }
        /*
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
            audioSource.Play();
        */
    }
    private void restartLevel()
    {
      SceneManager.LoadScene(0);
    }
    private void loadNextLevel()
    {
      SceneManager.LoadScene(1);
    }

    private void thrust()
    {
      float rocketUpThrust = mainThrust * Time.deltaTime;

      if(Input.GetKey(KeyCode.Space))
      {
        rigidbody.AddRelativeForce(Vector3.up * rocketUpThrust);
        if(!rocketShipAudioSource.isPlaying)
        {
          rocketShipAudioSource.PlayOneShot(mainEngine);
        }

      }
      else
      {
        rocketShipAudioSource.Stop();
      }
    }
    private void rotate()
    {
      float rocketRotation = rcsThrust * Time.deltaTime;
      rigidbody.freezeRotation = true; //take manual control of rotation

      if(Input.GetKey(KeyCode.A))
      {
        transform.Rotate(Vector3.left * rocketRotation);
      }
      else if(Input.GetKey(KeyCode.D))
      {
        transform.Rotate(Vector3.right * rocketRotation);
      }

      rigidbody.freezeRotation = false; //set rotation back to natural physics
    }

}
