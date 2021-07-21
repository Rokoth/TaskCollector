--Copyright 2021 Dmitriy Rokoth
--Licensed under the Apache License, Version 2.0
--
--ref2
--client
alter table client 
add constraint fk_client_user_id 
	foreign key(userid) 
		references "user"(id) 
		on delete no action on update no action;


--message
alter table "message" 
add constraint fk_message_client_id 
	foreign key(client_id) 
		references client(id) 
		on delete no action on update no action;


--message_status
alter table message_status 
add constraint fk_message_status_user_id 
	foreign key(userid) 
		references "user"(id) 
		on delete no action on update no action;

alter table message_status 
add constraint fk_message_status_message_id 
	foreign key(message_id) 
		references "message"(id) 
		on delete no action on update no action;








