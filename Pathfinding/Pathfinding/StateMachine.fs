module StateMachine
open SdlDotNet.Core
open Character
open Utils

let buildCharacterStateMachine =     
    fun(char:CharacterRecord) ->
         MailboxProcessor.Start(fun inbox ->
            let rec loop (n:CharacterRecord) =
                async {                     
                        let! msg = inbox.Receive()
                        match msg with
                        |Move x -> return! loop {n with location={x= n.location.x + x.x;y= n.location.y + x.y}}
                        |MoveTo x -> return! loop {n with location={x= x.x ;y= x.y}}
                        |SetWaypoints (x, ts)-> return! loop {n with waypoints = x; timeStamp=ts}
                        |Tick t-> 
                                if not n.waypoints.IsEmpty then                                    
                                    let dTime = ((float)(t - n.timeStamp)/100.0)
                                    let d = (int)(dTime * n.speed)                                    
                                    if n.waypoints.Length > d then
                                        return! loop {n with location =  n.waypoints.[d]}                                        
                                    else
                                        return! loop {n with waypoints = []}
                                else return! loop n
                        |TakeDamage x -> return! loop {n with health = n.health - x}
                        |Score x -> return! loop {n with score = n.score + x}
                        |FetchCharacter replyChannel  -> 
                            replyChannel.Reply n
                            return! loop n
                        }
            loop char)