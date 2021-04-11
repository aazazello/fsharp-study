namespace Schema

type Schema =
  {
    Id: int
    Key: string
    Text: string
    Active: bool
  }

 type SchemaCriteria =
     | All

 type SchemaFind = SchemaCriteria -> Schema[]

 type SchemaSave = Schema -> Schema

 type SchemaDelete = string -> bool
