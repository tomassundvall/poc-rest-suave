// --------------------------------------------------------------------------------------------------
// - http://blog.tamizhvendan.in/blog/2015/06/11/building-rest-api-in-fsharp-using-suave/
// --------------------------------------------------------------------------------------------------
module RestService.App

open Log
open Suave
open Suave.Successful

let logger = new Logger("RestService.App")

[<EntryPoint>]
let main argv =
    logger.Debug "Starting suave..."

    startWebServer defaultConfig (OK "Hello World!")
    0