--client
alter table client 
add constraint fk_client_user_id 
	foreign key("user_id") 
		references "user"(id) 
		on delete no action on update no action;


--client
alter table "message" 
add constraint fk_message_client_id 
	foreign key(client_id) 
		references client(id) 
		on delete no action on update no action;













