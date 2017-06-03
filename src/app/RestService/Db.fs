namespace RestService

type Person = {
    Id : int
    Name : string
    Age : int
    Email : string
}

module Db =  
    open RestService.Log
    open System.Collections.Generic

    let private _logger = new Logger("RestService.Db")
    let private _peopleStorage = new Dictionary<int, Person>()

    let getPeople () =
        _logger.Debug "Call to function getPeople"
        _peopleStorage.Values |> Seq.map (fun p -> p)

    let createPerson p =
        _logger.Debug "Call to function createPerson p.\np=%A" p
        let id = _peopleStorage.Count + 1

        _logger.Debug "Generated id for new person = %i" id
        let newPerson = {
            Id = id
            Name = p.Name
            Age = p.Age
            Email = p.Email
        }
        _peopleStorage.Add(id, newPerson)
        newPerson

    let updatePersonById id p =
        _logger.Debug "Call to function updatePersonById id=%i p=%A" id p
        if _peopleStorage.ContainsKey(id) then
            _logger.Info "Found person in storage with id=%i" id

            let updatedPerson = {
                Id = id
                Name = p.Name
                Age = p.Age
                Email = p.Email
            }
            _peopleStorage.[id] <- updatedPerson
            Some updatedPerson
        else
            _logger.Info "No person with id=%i found in storage" id
            None

    let updatePerson p =
        updatePersonById p.Id p

    let deletePerson id =
        _logger.Debug "Call to function deletePerson id=%i" id
        _peopleStorage.Remove(id) |> ignore