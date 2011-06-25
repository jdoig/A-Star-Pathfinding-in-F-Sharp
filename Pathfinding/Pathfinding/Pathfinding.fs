module Pathfinding
open Maps
open Utils

//a wrapper for mapPoint that can contain pathing data as per typical A* pathfinding
type PathingNode =  
        {mapPoint:MapPoint; h:float; g:float; parent:PathingNode option} //g = cost of path so far, h = estimated cost to goal, parent = tile we came here from                
//returns a pathnode based on a given map point
let pointToPathNode parent goal node = {mapPoint=node; h=node.Distance goal; g=(parent.g+1.0); parent=Some(parent)} 

//A 2D tile specific version of the A* algorithm
let pathFind (map:Map) (goal:MapPoint) = aStar (fun n-> n.mapPoint) (fun n-> n.g) (fun n-> n.h) (fun n-> (map.GetNeighboursOf n.mapPoint.point) |> List.filter(fun n-> n.value =0) |> List.map (pointToPathNode n goal.point)) goal

let rec readPath (path:PathingNode) (list:Utils.Point list) =
    match path.parent.IsNone with
    | true -> list
    | false -> readPath  path.parent.Value (path.parent.Value.mapPoint.point::list)