using UnityEngine;
using System.Collections;

namespace Tests{
    public class SimulateFire : RunJobOnStartWithTimeout {
        public FireController FireController;

        #region implemented abstract members of RunTestOnStartWithTimeout

        protected override void DoTest()
        {
            FireController.Fire();
        }

        #endregion
    }    
}
