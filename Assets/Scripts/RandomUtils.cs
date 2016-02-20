using UnityEngine;
using System.Collections;

public static class RandomUtils {
    
    public static int[] GetShuffleArray(int arraySize){
        int[] result = new int[arraySize];
        for (int i = 0; i < arraySize; i++) {
            result[i] = i;
        }
        int tmp, shuffleId;
        for (int i = 0; i < arraySize; i++) {
            shuffleId = Random.Range(0,arraySize);
            tmp = result[i];
            result[i] = result[shuffleId];
            result[shuffleId] = tmp;
        }
        return result;
    }
}
