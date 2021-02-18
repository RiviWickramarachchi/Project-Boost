using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip engineTurnedOff;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;



    Rigidbody rigidbody;
    AudioSource rocketShipAudioSource;

    enum State {Alive,Dying,Transcending}
    State state = State.Alive;
    bool collisionsEnabled = true;
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

        if(Debug.isDebugBuild)
        {
          print("Debugbuild");
          respondToDebugKeys();

        }

    }

    void OnCollisionEnter(Collision collision)
    {
      //print("collided");
      if(state != State.Alive || collisionsEnabled == false)
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
          successParticles.Play();
          mainEngineParticles.Stop();
          Invoke("loadNextLevel",levelLoadDelay);
          break;
        default:
          print("Dead");
          state = State.Dying;
          rocketShipAudioSource.Stop();
          rocketShipAudioSource.PlayOneShot(crashSound);
          crashParticles.Play();
          mainEngineParticles.Stop();
          Invoke("restartLevel",levelLoadDelay);
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
      int numberOfScenesInGame = SceneManager.sceneCountInBuildSettings; //4
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //0
      print(currentSceneIndex);
      print(numberOfScenesInGame);
      int nextSceneIndex = currentSceneIndex + 1;
      print(nextSceneIndex);
      if(nextSceneIndex == numberOfScenesInGame)
      {
      
        restartLevel();
      }
      else
      {
        SceneManager.LoadScene(nextSceneIndex);
      }


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
        mainEngineParticles.Play();


      }
      else
      {
        rocketShipAudioSource.Stop();
        mainEngineParticles.Stop();
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

    private void respondToDebugKeys()
    {
      if(Input.GetKeyDown(KeyCode.L))
      {
        loadNextLevel();
      }
      if(Input.GetKeyDown(KeyCode.C))
      {
        collisionsEnabled = !collisionsEnabled;
      }
    }

}
