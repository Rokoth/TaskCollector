--Copyright 2021 Dmitriy Rokoth
--Licensed under the Apache License, Version 2.0
--
--ref2

select exists (
	select from information_schema.tables 
		where table_schema = 'public' AND 
			  table_name = 'deploy_history'
);