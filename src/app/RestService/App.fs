// --------------------------------------------------------------------------------------------------
// - http://blog.tamizhvendan.in/blog/2015/06/11/building-rest-api-in-fsharp-using-suave/
// - http://blog.tamizhvendan.in/blog/2015/07/15/securing-apis-in-suave-using-json-web-token/
// --------------------------------------------------------------------------------------------------
module RestService.App

open RestService.Log
open RestService.Db
open RestService.Restful
open Suave
open Suave.Successful

let logger = new Logger("RestService.App")

[<EntryPoint>]
let main argv =
    logger.Debug "--- RestService startup..."

    let personWebPart = rest "people" {
        GetAll = getPeople
        Create = createPerson
        Update = updatePerson
        Delete = deletePerson
    }

    startWebServer defaultConfig personWebPart
    0