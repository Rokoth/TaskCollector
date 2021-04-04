--user
create unique index uidx_user_login 
	on "user"("login") where not is_deleted;

create index idx_user_name
    on "user"("name");

--client
create unique index uidx_client_login 
	on client("login") where not is_deleted;

create index idx_client_name
    on client("name");

create index idx_client_user_id
    on client("user_id");
