using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace Tests{    
    public abstract class RunJobOnStartWithTimeout : MonoBehaviour {
        [FormerlySerializedAs("timeout")]
        public float Timeout;
        // Use this for initialization
        void Start () {
            StartCoroutine(DoTestCoroutine());
        }

        IEnumerator DoTestCoroutine(){
            yield return new WaitForSeconds(Timeout);
            DoTest();
        }

        protected abstract void DoTest();
    }
}