// Learn more about F# at http://fsharp.net
open SdlDotNet.Core open SdlDotNet.Graphics open SdlDotNet.Graphics.Sprites open SdlDotNet.Input 
open StateMachine open Character open Maps open Pathfinding open Utils
open System.Drawing 

let tilePath    = @"..\..\resources\images\Tiles.png" //path to games tile map
let guyPath    = @"..\..\resources\images\enemy.png" //path to games tile map
let mapPath     = @"..\..\level\"
let tileSize    = 17
let screenW     = 256   
let screenH     = 256
let timer       = System.Diagnostics.Stopwatch()
timer.Start() 
//********************Set up game window********************
Video.WindowCaption <- "Pathfinding  Demo" 
let screen = Video.SetVideoMode(screenW, screenH);  //Drawing Surface


//***********************Set up Level***********************
let level1_collisionMap,level1_tileMap = buildLevel 15 15 mapPath @"lvl1" //Builds all the maps for the level
let tileMap =           {width=24; tileSize=tileSize; surface=new Surface(tilePath)}
let level =             {width=level1_tileMap.width; height=level1_tileMap.height; surface=new Surface(screenW,screenH)}
let tileEngine =        {tiles=tileMap; level=level; tileMap = level1_tileMap.map}
tileEngine.Draw //Create surface now we have set up object

//*************************Pathfind***************************
let start = level1_collisionMap.GetElement 4 8
let goal =  level1_collisionMap.GetElement 4 0
let pathTo a b =    let path = pathFind level1_collisionMap b a [{mapPoint=a;h=a.Distance b.point;g=0.0;parent=None}] []
                    if path.IsSome then 
                        readPath path.Value  [path.Value.mapPoint.point] 
                        |> List.map (fun x-> x*tileSize)
                    else []

//***********************Set up Character*******************
let character = {
        state = buildCharacterStateMachine 
            {location = start.point*tileSize ; score=0; health = 100; speed = 0.6; waypoints = [];timeStamp= timer.ElapsedMilliseconds}
        }
let characterSprite = new Sprite(new Surface(guyPath), (dPoint character.Get.location))

//*************************Update***************************
let Update args =     
    screen.Blit(tileEngine.level.surface) |> ignore           //Draw Background      
    screen.Blit(characterSprite,(dPoint character.Get.location)) |> ignore //Draw character    
    character.Tick(timer.ElapsedMilliseconds)    
    screen.Update() 

//*************************Input***************************
let HandleInput key = 
    match key with     
    | Key.Escape -> Events.QuitApplication()    //Escape -> Quit
    | Key.M -> character.SetWaypoints (pathTo start goal ) timer.ElapsedMilliseconds
    | Key.N -> 
        let startLocation = level1_collisionMap.GetElementP(character.Get.location/tileSize)        
        character.SetWaypoints (pathTo startLocation {start with point={start.point with x = 0}} ) timer.ElapsedMilliseconds
    | _ -> ignore|>ignore
Events.KeyboardDown
    |> Observable.map(fun kbu-> kbu.Key)
    |> Observable.add(fun k-> HandleInput k)

Events.Tick.Add(Update)                 //Update screen and player position
Events.Quit.Add(fun(args)-> timer.Stop())
Events.Run();