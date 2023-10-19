using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterControl : MonoBehaviour {
    // Default amount of meter given to player
    public static float meterStat { get; set; } = 4f;
    public static float currentMeter = 4f;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        print(currentMeter);
        if (currentMeter >= 0) {
            GetComponent<Transform>().localScale = new Vector3((currentMeter / meterStat), 1, 1);
        }
    }
}