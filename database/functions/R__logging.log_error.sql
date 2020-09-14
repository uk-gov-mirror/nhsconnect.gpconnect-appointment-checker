create or replace function logging.log_error
(
	_application varchar(100),
	_logged timestamp,
	_level varchar(100),
	_user_id integer null,
	_user_session_id integer null,
	_message varchar(8000),
	_logger varchar(8000), 
	_callsite varchar(8000), 
	_exception varchar(8000)
)
returns void
as $$
begin

	insert into logging.error_log
	(
		application,
		logged,
		level,
		user_id,
		user_session_id,
		message,
		logger,
		callsite,
		exception
	)
	values
	(
		_application,
		_logged,
		_level,
		_user_id,
		_user_session_id,
		_message,
		_logger,
		_callsite,
		_exception
	);

end;
$$ language plpgsql;