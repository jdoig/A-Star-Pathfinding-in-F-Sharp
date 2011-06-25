module Utils
open System
open System.Drawing;

type Point = 
            {x:int;y:int}   
             static member (-) (a :Point , b :Point) = {x=a.x-b.x ; y=a.y-b.y}
             static member (+) (a :Point , b :Point) = {x=a.x+b.x ; y=a.y+b.y}
             static member (*) (a :Point , b :int) = {x=a.x*b ; y=a.y*b}             
             static member (/) (a :Point , b :int) = {x=a.x/b ; y=a.y/b}             

let dPoint p = new System.Drawing.Point(p.x,p.y)

let sqr x = float(x * x) //Square two ints into a float

let rec remove l predicate =    //Remove element where predicate
    match l with
    | [] -> []
    | hd::tl -> if predicate(hd) then
                    (remove tl predicate)
                 else
                     hd::(remove tl predicate)

let loadMap path = 
                    IO.File.ReadAllText(path).Split(';')
                        |> Array.filter(fun s -> s<> String.Empty)
                        |> Array.map(fun s-> Int32.Parse(s.Replace(";",String.Empty)))

//The A* Algorythm
let rec aStar value g h neighbours goal start (openNodes: 'a list) (closedNodes: 'a list) =
    let f x:float = (g x)+(h x) //f will be the value we sort open nodes buy.
    let isShorter nodeA nodeB = nodeA = nodeB && f nodeA < f nodeB 
            
    let rec checkNeighbours neighbours openNodeAcc = 
        match neighbours with
        | [] -> openNodeAcc
        | currentNode::rest ->
            let likeCurrent = fun n -> (value n) = (value currentNode) //vale of n == value of current
            let containsCurrent = List.exists likeCurrent              //list contains likeCurrent
            let checkNeighbours = checkNeighbours rest 

            if openNodeAcc |> List.exists (isShorter currentNode) then //The current node is a shorter path than than one we already have.
                let shorterPath = remove openNodeAcc likeCurrent //So remove the old one...
                checkNeighbours  (currentNode::shorterPath)   //...and arry on with the new one.
            elif not(containsCurrent closedNodes) && not(containsCurrent openNodeAcc) then //The current node has not been queried
                checkNeighbours (currentNode::openNodeAcc) //So add it to the open set
            else checkNeighbours openNodeAcc // else carry on

    let nodes = neighbours openNodes.Head //The next set of nodes to work on
    
    let pathToGoal = nodes |> List.tryFind (fun x -> (value x) = goal) 
    if pathToGoal.IsSome then pathToGoal //Found the goal!
    else
        let nextSet = 
            checkNeighbours nodes openNodes.Tail
            |> List.sortBy f //sort open set by node.f
        if nextSet.Length > 0 then //Carry on pathfinding
            aStar value g h neighbours goal start nextSet (nextSet.Head::closedNodes)
        else None //if there are no open nodes pathing has failed