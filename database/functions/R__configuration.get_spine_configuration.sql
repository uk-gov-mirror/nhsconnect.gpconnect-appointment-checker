create or replace function configuration.get_spine_configuration
(
)
returns table
(
    use_ssp boolean,
    ssp_hostname varchar(100),
    sds_hostname varchar(100),
    sds_port integer,
    sds_use_ldaps boolean,
    organisation_id integer,
    party_key varchar(20),
    asid varchar(20)
)
as $$
begin

	return query
	select
	    s.use_ssp,
	    s.ssp_hostname,
	    s.sds_hostname,
	    s.sds_port,
	    s.sds_use_ldaps,
	    s.organisation_id,
	    s.party_key,
	    s.asid
	from configuration.spine s;
	
end;
$$ language plpgsql;
