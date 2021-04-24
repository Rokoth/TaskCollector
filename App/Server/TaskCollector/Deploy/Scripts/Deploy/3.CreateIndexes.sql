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
    on client(userid);

--message
create index idx_message_level
    on "message"("level");

create index idx_message_title
    on "message"("title");

create index idx_message_client_id
    on "message"(client_id);

create index idx_message_created_date
    on "message"(created_date);

--message_status
create index idx_message_status_message_id
    on message_status(message_id);

create index idx_message_status_status_id
    on message_status(status_id);

create index idx_message_status_user_id
    on message_status(userid);

