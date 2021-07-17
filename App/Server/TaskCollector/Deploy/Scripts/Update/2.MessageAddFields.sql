alter table message_status
add column is_last boolean;

alter table h_message_status
add column is_last boolean;

alter table message_status
add column next_notify_date timestamptz;

alter table h_message_status
add column next_notify_date timestamptz;