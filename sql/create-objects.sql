create table public.schema (
	"id" serial,
	"key" character varying(64) not null,
	"data" jsonb not null,
	"version" timestamp default current_timestamp
)