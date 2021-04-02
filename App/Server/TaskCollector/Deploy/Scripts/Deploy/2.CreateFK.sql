alter table client 
add constraint fk_client_user_id 
	foreign key(user_id) 
		references "user"(id) 
		on delete no action on update no action;


