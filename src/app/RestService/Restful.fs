namespace RestService

module Restful =
    open RestService.Log
    open RestService.Db
    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization
    open Suave
    open Suave.Successful
    open Suave.Operators
    open Suave.RequestErrors
    open Suave.Filters

    let _logger = new Logger("RestService.Restful")

    type RestResource<'a> = {
        GetAll : unit -> 'a seq
        Create : 'a -> 'a
        Update : 'a -> 'a option
        Delete : int -> unit
    }

    let json v =
        let jsonSerializerSettings = new JsonSerializerSettings()
        jsonSerializerSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(v, jsonSerializerSettings)
        |> OK >=> Writers.setMimeType "application/json; charset-utf-8"

    let fromJson<'a> json =
        JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a

    let getString rawForm =
        let raw = System.Text.Encoding.UTF8.GetString(rawForm)
        raw

    let getResourceFromReq<'a> (req : HttpRequest) =
        req.rawForm |> getString |> fromJson<'a>

    let rest resourceName resource =
        _logger.Debug "Rest request to resource '%s'" resourceName

        let resourcePath = "/" + resourceName
        let resourceIdPath =
            new PrintfFormat<(int -> string),unit,string,string,int>(resourcePath + "/%d")
        _logger.Info "ResourceIdPath=%A" resourceIdPath
        let badRequest = BAD_REQUEST "Resource not found"

        let getAll = warbler (fun _ -> resource.GetAll () |> json)
        
        let handleResource requestError = function
            | Some r -> r |> json
            | _ -> requestError
        
        let deleteResourceById id =
            _logger.Debug "Call to function deleteResouceById (id=%i)" id
            resource.Delete id
            NO_CONTENT

        choose [
            path resourcePath >=> choose [
                GET >=> getAll
                POST >=> request (getResourceFromReq >> resource.Create >> json)
                PUT >=> request (getResourceFromReq >> resource.Update >> handleResource badRequest)
            ]        
            DELETE >=> pathScan resourceIdPath deleteResourceById
        ]