using UnityEngine;
using System.Collections;

namespace Tests{
    public class TestLevelMapTheSame : RunJobOnStartWithTimeout {
        [Multiline]
        public string MapToCompare;
        #region implemented abstract members of RunTestOnStartWithTimeout
        protected override void DoTest()
        {
            byte[,] thisMap = TestMapGeneration.StringToMap(MapToCompare);
            if (GameData.Map == null || GameData.Map.GetLength(0) != thisMap.GetLength(0)
                || GameData.Map.GetLength(1) != thisMap.GetLength(1)){
                IntegrationTest.Fail(gameObject);
            }

            for (int y = 0; y < thisMap.GetLength(1); y++)
            {
                for (int x = 0; x < thisMap.GetLength(0); x++)
                {
                    if (thisMap[x,y] != GameData.Map[x,y]){
                        IntegrationTest.Fail(gameObject);
                    }
                }
            }
            IntegrationTest.Pass(gameObject);
        }
        #endregion
    }
}
