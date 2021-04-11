module Schema.Storage

open Schema
open Microsoft.Extensions.DependencyInjection
open System.Collections

let find (inMemory : Hashtable) (criteria : SchemaCriteria) : Schema[] =
    match criteria with
    | All -> inMemory.Values |> Seq.cast |> Array.ofSeq

let save (inMemory : Hashtable) (schema: Schema) : Schema =
    let item = inMemory.[schema.Id]
    match isNull(item) with
    | false -> inMemory.Remove(schema.Id) |> ignore
    | true -> () |> ignore
    inMemory.Add(schema.Id, schema) |> ignore
    schema

type IServiceCollection with
   member this.AddSchemaInMemory (inMemory : Hashtable) =
     this.AddSingleton<SchemaFind>(find inMemory) |> ignore
     this.AddSingleton<SchemaSave>(save inMemory) |> ignore
 