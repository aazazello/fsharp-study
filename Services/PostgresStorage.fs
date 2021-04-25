module Schema.Storage

open Schema
open Schema.Configuration
open FSharp.Data
open FSharp.Data.Sql
open Npgsql
open Microsoft.Extensions.DependencyInjection

//Static connection string used through build time inside sdk container 
let [<Literal>] connectionString = "Host=192.168.7.254;Port=5432;Database=schemas;Username=andrey;Password=123456"
let [<Literal>] RR = @"~/.nuget/packages/npgsql/5.0.0/lib/net5.0"

type sql = SqlDataProvider<
              ConnectionString = connectionString,
              DatabaseVendor = Common.DatabaseProviderTypes.POSTGRESQL,
              ResolutionPath = RR,
              IndividualsAmount = 1000,
              UseOptionTypes = true>

// runtime connection string
let getConnectionString(config: DbConfig) : string =
     "Host=" + config.DbHost + ";Port=5432;Database=" + config.DbName + ";Username=" + config.DbUser + ";Password=" + config.DbPass


let find (config : DbConfig) (criteria : SchemaCriteria) : Schema[] =

    let context = getConnectionString config |> sql.GetDataContext //getConnectionString(config)

    let data = query {
            for schema in context.Public.Schema do select (schema) 
        }
   
    let all = data  |> Seq.map(fun s ->  {Id = s.Id; Key= s.Key; Text = s.Data; Active = true}) |> Seq.toArray

    all

let save (config : DbConfig) (schema: Schema) : Schema =
  //  let item = inMemory.[schema.Id]
  //  match isNull(item) with
  //  | false -> inMemory.Remove(schema.Id) |> ignore
  //  | true -> () |> ignore
  //  inMemory.Add(schema.Id, schema) |> ignore
    schema

type IServiceCollection with
   member this.AddSchemaPostgress (config : DbConfig) =
     this.AddSingleton<SchemaFind>(find config) |> ignore
     this.AddSingleton<SchemaSave>(save config) |> ignore
 