﻿create extension if not exists "uuid-ossp";

create table if not exists "user"(
      id            uuid          not null default uuid_generate_v4() primary key
	, "name"        varchar(100)  not null
	, "description" varchar(1000) null
	, "login"       varchar(100)  not null
	, "password"    varchar(1000) not null
	, version_date  timestamptz   not null default now()
	, is_deleted    boolean       not null
);

create table if not exists client(
	  id            uuid          not null primary key
	, "name"        varchar(100)  not null
	, "description" varchar(1000) null
	, "login"       varchar(100)  not null
	, "password"    varchar(100)  not null
	, map_rules     jsonb         null
	, user_id       uuid          not null
	, version_date  timestamptz   not null
	, is_deleted    boolean       not null default false	
);
