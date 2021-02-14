using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ScalingScript : MonoBehaviour
{

    [SerializeField] Vector3 scalingVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;

    float scalingFactor;
    Vector3 startingScale;
    // Start is called before the first frame update
    void Start()
    {
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
      if (period <= Mathf.Epsilon)
      {
        return;
      }
      //automatically scaling the obstacles
      float cycles = Time.time / period;

      const float tau = Mathf.PI * 2f; //approximately 6.28
      float rawSinWave = Mathf.Sin( cycles * tau);
      scalingFactor = (rawSinWave/2f) + 0.5f;

      Vector3 sizeIncrement = scalingVector * scalingFactor;
      transform.localScale = startingScale + sizeIncrement;

    }
}
