﻿--Copyright 2021 Dmitriy Rokoth
--Licensed under the Apache License, Version 2.0
--
--ref2
create extension if not exists "uuid-ossp";

create table if not exists "user"(
      id            uuid          not null default uuid_generate_v4() primary key
	, "name"        varchar(100)  not null
	, "description" varchar(1000) null
	, "login"       varchar(100)  not null
	, "password"    bytea         not null
	, version_date  timestamptz   not null default now()
	, is_deleted    boolean       not null
);

create table if not exists "h_user"(
      h_id          bigserial     not null primary key        
    , id            uuid          null
	, "name"        varchar(100)  null
	, "description" varchar(1000) null
	, "login"       varchar(100)  null
	, "password"    bytea         null
	, version_date  timestamptz   null
	, is_deleted    boolean       null
	, change_date   timestamptz   not null default now()
	, "user_id"     varchar       null
);

create table if not exists client(
	  id            uuid          not null primary key
	, "name"        varchar(100)  not null
	, "description" varchar(1000) null
	, "login"       varchar(100)  not null
	, "password"    bytea         not null
	, map_rules     jsonb         null
	, userid        uuid          not null
	, version_date  timestamptz   not null
	, is_deleted    boolean       not null default false	
);

create table if not exists h_client(
	  h_id          bigserial     not null primary key  
	, id            uuid          null 
	, "name"        varchar(100)  null
	, "description" varchar(1000) null
	, "login"       varchar(100)  null
	, "password"    bytea         null
	, map_rules     jsonb         null
	, userid        uuid          null
	, version_date  timestamptz   null
	, is_deleted    boolean       null	
	, change_date   timestamptz   not null default now()
	, "user_id"     varchar       null
);

create table if not exists "message"(
	  id               uuid         not null primary key
	, "level"          smallint     not null
	, title            varchar(100) not null
	, "description"    varchar      not null
	, feedback_contact varchar      not null
	, add_fields       json         null
	, client_id        uuid         not null
	, created_date     timestamptz  not null
	, version_date     timestamptz  not null
	, is_deleted       boolean      not null default false
);

create table if not exists "h_message"(
      h_id             bigserial    not null primary key  
	, id               uuid         null
	, "level"          smallint     null
	, title            varchar(100) null
	, "description"    varchar      null
	, feedback_contact varchar      null
	, add_fields       json         null
	, client_id        uuid         null
	, created_date     timestamptz  null
	, version_date     timestamptz  null
	, is_deleted       boolean      null
	, change_date   timestamptz     not null default now()
	, "user_id"     varchar         null
);

create table if not exists "message_status"(
	  id            uuid        not null primary key
	, message_id    uuid        not null
	, status_id     smallint    not null
	, "description" varchar     not null
	, userid        uuid        not null
	, status_date   timestamptz not null
	, version_date  timestamptz not null
	, is_deleted    boolean     not null default false
);

create table if not exists "h_message_status"(
      h_id          bigserial   not null primary key  
	, id            uuid        null
	, message_id    uuid        null
	, status_id     smallint    null
	, "description" varchar     null
	, userid        uuid        null
	, status_date   timestamptz null
	, version_date  timestamptz null
	, is_deleted    boolean     null
	, change_date   timestamptz not null default now()
	, "user_id"     varchar     null
);

create table if not exists settings(	 
	  id            int           not null primary key
	, param_name    varchar(100)  not null
	, param_value   varchar(1000) not null	
);