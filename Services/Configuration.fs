module Schema.Configuration

open FsConfig

[<Convention("APP", Separator="-")>]
type DbConfig = 
 {
    DbHost: string
    DbName: string
    DbUser: string
    DbPass: string
 }

let GetConfiguration() : DbConfig =

    let config =
        try
            match EnvConfig.Get<DbConfig>() with
            | Ok config -> config
            | Error error ->
                match error with
                    | NotFound envVarName ->
                      failwithf "Variable %s not found" envVarName
                    | BadValue (envVarName, value) ->
                      failwithf "Variable %s has incorrect value " envVarName value
                    | NotSupported msg ->
                      failwith msg 
 
        with
            | ex -> printfn "%s" (ex.ToString()); { DbHost = ""; DbName = ""; DbUser = ""; DbPass = ""; } 
    
    printfn "Configuration: %s" (config.ToString())
 
    config