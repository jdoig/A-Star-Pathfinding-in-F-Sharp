module Maps
open Utils 
open SdlDotNet.Graphics
open System.Drawing

type TileSurface = {width:int; tileSize:int; surface:Surface}
type Level = {width:int; height:int; surface:Surface;}

type TileMap = 
    {tiles:TileSurface; level:Level; tileMap:int list}
    member private this.GetRect x w= 
        let ts = this.tiles.tileSize
        new Rectangle((x % w)*ts, (x / w)*ts,ts,ts);        
    member this.Draw = for i in 0..this.tileMap.Length-1 do
                             let sRect = this.GetRect this.tileMap.[i] this.tiles.width
                             let dRect = this.GetRect i this.level.width
                             this.level.surface.Blit(this.tiles.surface,dRect,sRect)|>ignore

type MapPoint =             
    {point:Utils.Point;value:int}                                     
    member this.Distance mp = sqrt (sqr(this.point.x+mp.x) + sqr(this.point.y+mp.y)) //abs(this.x-mp.x)+abs(this.y-mp.y) //Calculate distance to other map point

type Map =  //Simple construct to hold the 2D map data
    {width:int; height:int; map:int list} //Width & Height of map and map data in 1D array
    member this.GetElement x y = {point = {x=x;y=y}; value=this.map.[x % this.height + y * this.width]} //function to wrap 1D array into 2D array to retrive map point
    member this.GetElementP p = {point = p; value=this.map.[p.x % this.height + p.y * this.width]} //function to wrap 1D array into 2D array to retrive map point
    member this.GetNeighboursOf p =  //return list of map points that surround current map point
        [   for y in p.y-1..p.y+1 do
                for x in p.x-1..p.x+1 do
                    if ((y<>p.y || x <>p.x) && y>=0 && x>=0 && x<this.width && y<this.height) //bounds checking
                    then yield this.GetElement  x y]

let buildLevel w h path level= 
                            {width=w;height=h;map=(loadMap (path+level+"_collision") |> Seq.toList)}, 
                            {width=w;height=h;map=(loadMap (path+level+"_tiles") |> Seq.toList)} 