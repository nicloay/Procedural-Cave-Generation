using UnityEngine;
using System.Collections;

namespace Tests{
    
    public class NumberOfBulletOnScene : MonoBehaviour {
        public int ExpectedNumber ;
        public float Timeouth;

        // Use this for initialization
        void Start () {
            StartCoroutine(DoCheck()) ;       
        }
        
        IEnumerator DoCheck(){
            yield return new WaitForSeconds(Timeouth);
            if (FindObjectsOfType<BulletController>().Length == ExpectedNumber){
                IntegrationTest.Pass(gameObject);
            } else {
                IntegrationTest.Fail(gameObject);
            }
        }
    }
}
