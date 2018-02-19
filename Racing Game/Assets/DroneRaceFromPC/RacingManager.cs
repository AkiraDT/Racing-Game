using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacingManager : MonoBehaviour {

    public Checkpoint[] checkpointChecker;
    public bool[,] passedData;
    public int currentCheckpoint;
    public int totalCheckpoint;
    public int totalPlayer = 2;

    //index 1 for player 1
    //index 2 for player 2
    public Text[] checkpointLabels;
    
    private void Start()
    {
            
        Transform _transform = GetComponent<Transform>();
        totalCheckpoint = _transform.childCount;
        checkpointChecker = new Checkpoint[totalCheckpoint];

        //create 2 dimension array for storing passed data checkpoint from
        //player 1 and 2
        passedData = new bool[totalPlayer, totalCheckpoint];

        for (int i=0;i<totalCheckpoint; i++)
        {
            checkpointChecker[i] = _transform.GetChild(i).GetComponent<Checkpoint>();
            checkpointChecker[i].Reset();
        }
    }

    public bool isCheckpointPassed(int playerNumber,int index)
    {
        return passedData[playerNumber, index];
    }

    public void setPassedCheckpoint(int playerNumber, int index)
    {
        passedData[playerNumber, index] = true;
        checkpointLabels[playerNumber].text = "CHECKPOINT " + (index+1).ToString()+"/"+ totalCheckpoint.ToString();
    }
    public void setCheckpointLabel(int playerNum, string _text)
    {
        checkpointLabels[playerNum].text = _text;
    }
    public void resetCheckpoint()
    {
        currentCheckpoint = 0;
        for(int i=0;i<totalCheckpoint;i++)
        {
            checkpointChecker[i].Reset();
        }
        passedData = new bool[totalPlayer, totalCheckpoint];
    }
}
