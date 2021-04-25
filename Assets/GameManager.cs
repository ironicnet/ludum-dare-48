using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status {
    Alive,
    Dead,
    Exit,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Controllable[] prefabs = new Controllable[2];
    private Controllable[] Deepers = new Controllable[2];
    public Transform CameraContainer;
    public Transform Spotlight;
    public Vector3 SpotlightOffset;
    public DeeperJoin Join;

    public Level[] Levels;
    public int CurrentLevel;
    public int TargetLevel;

    private int Active;
    private int prevActive = -1;

    private Status[] status = new Status[2] {
        Status.Alive,
        Status.Alive
    };

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        LoadLevel(Levels[CurrentLevel]);
    }
    void CheckWin()
    {
        for (int i = 0; i < Deepers.Length; i++)
        {
            if (status[i] != Status.Exit)
            {
                return;
            }
        }

        NextLevel();
    }

    private void NextLevel()
    {
        TargetLevel += 1;
    }

    private static void FinishLevel(Level prevLevel)
    {
        prevLevel.Finish();
        if (prevLevel.DisableOnExit)
        {
            prevLevel.gameObject.SetActive(false);
        }
    }

    private void LoadLevel(Level level)
    {
        level.gameObject.SetActive(true);
        level.Begin();
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (Deepers[i]!=null && Deepers[i].Status != Status.Dead) {
                GameObject.Destroy(Deepers[i].gameObject);
            }
            Deepers[i] = GameObject.Instantiate(prefabs[i]);
            Deepers[i].transform.position = level.StartPositions[i].position;
            Deepers[i].transform.rotation = level.StartPositions[i].rotation;
            if (i == 0) {
                Join.Deeper1 = Deepers[i].gameObject;
            } else {
                Join.Deeper2 = Deepers[i].gameObject;
            }
            status[i] = Status.Alive;
        }
        Deepers[0].other = Deepers[1].gameObject;
        Deepers[1].other = Deepers[0].gameObject;
        CameraContainer.transform.position = level.CameraLocations[0].position;
        CameraContainer.transform.rotation = level.CameraLocations[0].rotation;
        
    }

    internal void OnExitEnter(Controllable controllable, Exit exit)
    {
        for (int i = 0; i < Deepers.Length; i++)
        {
            if (Deepers[i] == controllable && status[i]!= Status.Dead) {
                status[i] = Status.Exit;
                break;
            }
        }
        CheckWin();
    }

    internal void OnExitExit(Controllable controllable, Exit exit)
    {
        if (controllable.Status != Status.Dead) {
            for (int i = 0; i < Deepers.Length; i++)
            {
                if (Deepers[i] == controllable) {
                    status[i] = Status.Alive;
                    break;
                }
            }
            
            CheckWin();
        }
    }
    internal void OnDeath(Controllable controllable, Death death)
    {
        if (death.Level == Levels[CurrentLevel]) {
            if (controllable.Status != Status.Dead) {
                for (int i = 0; i < Deepers.Length; i++)
                {
                    Deepers[i].Status = Status.Dead;
                    Deepers[i].SelfDestruct();
                    status[i] = Status.Dead;
                }
                
                LoadLevel(Levels[CurrentLevel]);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (TargetLevel != CurrentLevel) {
            for (int i = CurrentLevel; i < TargetLevel; i++)
            {
                FinishLevel(Levels[i]);
            }
            CurrentLevel = TargetLevel;
            LoadLevel(Levels[TargetLevel]);
        }
   
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump")) {
            prevActive = Active;
            Active++;
            if (Active >= Deepers.Length) {
                Active = 0;
            }
             Deepers[prevActive].inputEnabled = false;
             Deepers[Active].inputEnabled =true;
        }

        var force = (Vector3.right * horizontal + Vector3.forward * vertical);
        if (force.magnitude > 0) {
            Deepers[Active].Move(force);
        }
        CameraContainer.transform.position = Levels[CurrentLevel].CameraLocations[0].position;
        CameraContainer.transform.rotation = Levels[CurrentLevel].CameraLocations[0].rotation;
        Spotlight.transform.position =  Deepers[Active].transform.position + SpotlightOffset;
    }
}
