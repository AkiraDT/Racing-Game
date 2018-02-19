using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
    public bool[] is_passed;
    private int _index;
    RacingManager racingManager;

    private void Start()
    {
        is_passed = new bool[2];
        _index = int.Parse(transform.name)-1;
        racingManager = transform.GetComponentInParent<RacingManager>();
    }
    void  OnTriggerEnter ( Collider other  )
	{
        int _playNum;
        //get player number
        if (other.tag == "Player")
        {
            _playNum = 0;
        }
        else
        {
            _playNum = 1;
        }

        //if this is first checkpoint
        if(_index == 0)
        {
            is_passed[_playNum] = true;
            racingManager.setPassedCheckpoint(_playNum, _index);
        }
        //check checkpoint before this, 
        //make sure user dont skip checkpoint
        else if (racingManager.isCheckpointPassed(_playNum, _index-1))         {
            is_passed[_playNum] = true;
            racingManager.setPassedCheckpoint(_playNum, _index);
        }
        else
        {
            racingManager.setCheckpointLabel(_playNum,"YOU JUST SKIP CHECKPOINT BEFORE THIS");
            return;
        }

        //make this checkpoint's status passed
        is_passed[_playNum] = true;
	}

    public void Reset()
    {
        is_passed = new bool[2];
    }

}
