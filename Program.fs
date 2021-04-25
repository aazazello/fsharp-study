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
    