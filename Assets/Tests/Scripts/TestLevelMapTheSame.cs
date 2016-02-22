using UnityEngine;
using System.Collections;

namespace Tests{
    public class TestLevelMapTheSame : RunJobOnStartWithTimeout {
        [Multiline]
        public string MapToCompare;
        #region implemented abstract members of RunTestOnStartWithTimeout
        protected override void DoTest()
        {
            
        }
        #endregion
    }
}
