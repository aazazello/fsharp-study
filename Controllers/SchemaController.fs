namespace Schema.Http

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open Schema
open System

module SchemaHttp =
  let handlers : HttpFunc -> HttpContext -> HttpFuncResult =
    choose [
      POST >=> route "/" >=>
        fun next context ->
            task {
                let save = context.GetService<SchemaSave>()
                let! schema = context.BindJsonAsync<Schema>()
                let schema = { schema with Key = ShortGuid.fromGuid(Guid.NewGuid()) }
                return! json (save schema) next context
            }

      GET >=> route "/" >=>
        fun next context ->
            printfn "GET Schemas"
            let find = context.GetService<SchemaFind>()
            let schemas = find SchemaCriteria.All
            json schemas next context

      PUT >=> routef "/%s" (fun id ->
        fun next context ->
            task {
                let save = context.GetService<SchemaSave>()
                let! schema = context.BindJsonAsync<Schema>()
                let schema = { schema with Key = id }
                return! json (save schema) next context
            })

      DELETE >=> routef "/%s" (fun id ->
        fun next context ->
            let delete = context.GetService<SchemaDelete>()
            json (delete id) next context)
    ]
