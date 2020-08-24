using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABMainLoop : MonoBehaviour
{
	public ABPawnManager manager;

    void Start() {
    	manager.Init();
    }

    void Update() {
    	manager.Tick(Time.deltaTime);
    }
}
