module Character
open Utils

type CharacterRecord = {location:Point; health:int; score:int; speed:float; waypoints:Point list; timeStamp:int64}  //Holds Data about our charaters

type CharacterMessage =     //Used by the character state machine to update the character
    | Move          of Point        //Move from current location by (x,y)
    | MoveTo        of Point        //Move from current to location (x,y)
    | TakeDamage    of int          //Health -= x
    | Score         of int          //Score += x
    | Tick          of int64   
    | SetWaypoints  of Point list * int64
    | FetchCharacter  of AsyncReplyChannel<CharacterRecord>

type Character = 
    {state: MailboxProcessor<CharacterMessage>} //The characters state as tracked by a mailbox processor
    member this.Get          = this.state.PostAndReply(fun rc-> FetchCharacter rc)  //Get Current state
    member this.Move v       = this.state.Post(Move v)                              //Send move message to state machine
    member this.MoveTo v     = this.state.Post(MoveTo v)                            //Send moveTo message to state machine
    member this.TakeDamage d = this.state.Post(TakeDamage d)                        //Send take damage message to state machine
    member this.Score s      = this.state.Post(Score s)                             //Send score message to state machine
    member this.Tick t       = this.state.Post(Tick t)
    member this.SetWaypoints wp ts = this.state.Post(SetWaypoints(wp,ts))
