using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColonyOptions
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///ColonyOptions

    public static readonly int FieldSclaling = 5;  //overall scaling to make it more beatiful 

    ///////////////////////////
    ///general
    ///
    public static bool IsInBuildMode { get; set; }
    // how fast Simulation will play
    public static readonly int PlaySpeedDefault = 2;
    // playgroundSize
    public static int FieldSize { get; set; }
    //how long pheromones will appear in worldspace
    public static int PheromoneDuration { get; set; }
    // time in seconds relative to _PlaySpeed_ ants will spawn
    public static int SpawnInterval { get; set; }
    // spawn with _SpawnInterval_ or together ?
    public static int SpawnTogether { get; set; }


    // playgroundSize   
    public static readonly int FieldSizeDefault = 15;
    //how long pheromones will appear in worldspace
    public static readonly int PheromoneDurationDefault = 30;
    // time in seconds relative to _PlaySpeed_ ants will spawn
    public static readonly int SpawnIntervalDefault = 2;
    // spawn with _SpawnInterval_ or together ?
    public static readonly int SpawnTogetherDefault = 0;




    //////////////////////////
    ///worker
    /// 
    // how many workers
    public static int WorkerCount { get; set; }
    // how long pheromones of workers will apear
    public static int WorkerPerception { get; set; }
    // how many waypoints a worker will know 
    public static int LastPathLength { get; set; }

    // how many workers
    public static readonly int WorkerCountDefault = 150;
    // how long pheromones of workers will apear
    public static readonly int WorkerPerceptionDefault = 2;
    // how many waypoints a worker will know 
    public static readonly int LastPathLengthDefault = 4;




    //////////////////////////
    ///Scouts
    ///
    // how many scouts
    public static int ScoutCount { get; set; }
    // how long pheromones of scouts will apear
    public static int ScoutPerception { get; set; }
    // how many waypoints a path can have -> destroy scout -> spawn new
    public static int MaxPathLimit { get; set; }

    // how many scouts
    public static readonly int ScoutCountDefault = 10;
    // how long pheromones of scouts will apear
    public static readonly int ScoutPerceptionDefault = 2;   
    // how many waypoints a path can have -> destroy scout -> spawn new
    public static readonly int MaxPathLimitDefault = 75;
   
}
