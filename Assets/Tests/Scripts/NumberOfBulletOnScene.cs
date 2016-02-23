using UnityEngine;
using System.Collections;

namespace Tests{
    
    public class NumberOfBulletOnScene : RunJobOnStartWithTimeout {
        public int ExpectedNumber ;

        #region implemented abstract members of RunTestOnStartWithTimeout
        protected override void DoTest()
        {
            if (FindObjectsOfType<BulletController>().Length == ExpectedNumber){
                IntegrationTest.Pass(gameObject);
            } else {
                IntegrationTest.Fail(gameObject);
            }
        }
        #endregion
    }
}
