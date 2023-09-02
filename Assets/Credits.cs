using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    bool isScrolling = false;
    public float yLimit;
    private float initialY = -1643;
    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.localPosition.y;
        StartCoroutine(BeginScroll());
    }

    void OnEnable() {
        transform.localPosition = new Vector3(transform.localPosition.x, initialY, transform.localPosition.z);
        StartCoroutine(BeginScroll());
    }

    //ondisable 
    void OnDisable() {
        StopAllCoroutines();
        isScrolling = false;
    }

    IEnumerator BeginScroll()
    {
        yield return new WaitForSeconds(2f);
        isScrolling = true;   
    }

    // Update is called once per frame
    void Update()
    {
        if(isScrolling) {
            if(transform.localPosition.y >= yLimit) {
                isScrolling = false;
            } else {
            
                //check if user holding space. if so scroll faster
                if(Input.GetKey(KeyCode.Space)) {
                    transform.position += Vector3.up * Time.deltaTime * 180f;
                } else {
                    transform.position += Vector3.up * Time.deltaTime * 70f;
                }
            }
        }
    }
}
