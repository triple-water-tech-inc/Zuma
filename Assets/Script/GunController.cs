using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject ballPrefab;
    public float shootLen;
    public float shootSpeed;
    public GameObject nextBall;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
#else
        if (Input.touchCount>0)
        if (Input.touches[0].phase == TouchPhase.Ended)
        {
            if (PathController.Factory.IsFactoring() && _canShoot) Shoot();
        }
#endif
        
    }
    
    private IEnumerator FollowCoroutine()
    {
        while (true)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            
            
            var dx = Input.mousePosition.x - Camera.main.WorldToScreenPoint(transform.position).x;
            var dy = Input.mousePosition.y - Camera.main.WorldToScreenPoint(transform.position).y;
            var strawRadians = Mathf.Atan2(dy, dx);
            var strawDigrees = 360.0f*strawRadians/(2.0f*Mathf.PI);
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y,
                strawDigrees - 90);
            
#else
            if (Input.touchCount > 0)
            {
                var dx = Input.touches[0].position.x - Camera.main.WorldToScreenPoint(transform.position).x;
                var dy = Input.touches[0].position.y - Camera.main.WorldToScreenPoint(transform.position).y;
                var strawRadians = Mathf.Atan2(dy, dx);
                var strawDigrees = 360.0f*strawRadians/(2.0f*Mathf.PI);
                transform.localRotation = Quaternion.Euler(transform.localRotation.x, -strawDigrees + 90,
                    transform.rotation.z);
            }
#endif
                
            yield return new WaitForEndOfFrame();
        }
    }

    void Shoot()
    {
        var shootVector = transform.up;
        var nextBallPos = nextBall.transform.position;
        var ball = Instantiate(ballPrefab, nextBallPos, Quaternion.identity);
        var ballController = ball.GetComponent<BallController>();
        StartCoroutine(ballController.ShootCoroutine(nextBallPos + shootVector * shootLen, shootLen / shootSpeed));
        StartCoroutine(ballController.SelfDestroyCoroutine(shootLen / shootSpeed));
    }
    

    
    
    
    
    
}
