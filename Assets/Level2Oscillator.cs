using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Level2Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f); //check reason
    [SerializeField] float period = 2f;
    //todo Remove later
    //[SerializeField] [Range(0,1)] float movementFactor;
    float movementFactor;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      float cycles = Time.time / period;

      const float tau = Mathf.PI * 2f; //approximately 6.28
      float rawSinWave = Mathf.Sin( cycles * tau);
      movementFactor = (rawSinWave/2f) + 0.5f;

      Vector3 displacement = movementVector * movementFactor;
      transform.position = startingPos + displacement;
    }
}
