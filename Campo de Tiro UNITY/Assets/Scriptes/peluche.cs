using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class peluche : MonoBehaviour
{
    public int id;
    public GameObject gm;
    // Start is called before the first frame update
    void Start()
    {
        //NotificationCenter.DefaultCenter().AddObserver(this,"morir");
        
    }/*
    public void morir(Notification no)
    {
        Destroy(gameObject);
    }*/
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bala")
        {
            Destroy(gameObject);
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "bala" && gm.GetComponent<prueba360>().id==id)
        {
            GetComponent<AudioSource>().Play();
            gm.GetComponent<prueba360>().punto();
            //NotificationCenter.DefaultCenter().PostNotification(this,"punto");
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
