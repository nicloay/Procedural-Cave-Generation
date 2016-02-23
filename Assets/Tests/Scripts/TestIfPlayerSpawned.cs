using CaveTestTask;


namespace Tests{    
    public class TestIfPlayerSpawned : RunJobOnStartWithTimeout {
        
        #region implemented abstract members of RunTestOnStartWithTimeout
        protected override void DoTest()
        {            
            if (FindObjectOfType<Player>() != null){
                IntegrationTest.Pass(gameObject);
            } else {
                IntegrationTest.Fail(gameObject);
            }
        }
        #endregion
    }
    
}