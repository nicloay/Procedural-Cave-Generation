using UnityEngine;
using System.Collections;

namespace Tests{
    public class SimulateFire : MonoBehaviour {
        public float timeout;
        public FireController FireController;

        void Awake () {
            StartCoroutine(DoFire());
        }

        IEnumerator DoFire(){
            yield return new WaitForSeconds(timeout);
            FireController.Fire();
        }
    }    
}
