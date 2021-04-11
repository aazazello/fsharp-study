module Schema.Program

open System
open System.Collections
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open FsConfig
open Schema.Configuration
open Schema.Http
open Schema.Storage

let routes =
    choose [
        SchemaHttp.handlers
    ]

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffe routes

let configureServices (services : IServiceCollection) =

    let config = Configuration.GetConfiguration()
    //let schemas = Hashtable()
    services.AddGiraffe() |> ignore
    services.AddSchemaPostgress(config) |> ignore

[<EntryPoint>]
let main _ =

    WebHostBuilder()
        .UseKestrel()
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .Build()
        .Run()
    0
 

 // docker run -d -p 5000:80 --env APP-DB-HOST=192.168.7.254 --env APP-DB-NAME=schemas --env APP-DB-USER=schema_service --env APP-DB-PASS=@12345 schemas:latest