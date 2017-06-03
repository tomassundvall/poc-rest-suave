namespace RestService

module Log =

    type Logger(name : string) =
    
        let _log = log4net.LogManager.GetLogger(name)

        member this.Debug msg = Printf.ksprintf _log.Debug msg
        member this.Info msg = Printf.ksprintf _log.Info msg
        member this.Warn msg = Printf.ksprintf _log.Warn msg
        member this.Error msg = Printf.ksprintf _log.Error msg
        member this.Fatal msg = Printf.ksprintf _log.Fatal msg