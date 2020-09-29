<img src="documentation/images/logo.png" height=72>

# GP Connect Appointment Checker

The GP Connect Appointment Checker is a web application for GP practice and operational support staff to view free appointment slots in a GP practice's appointment book that are available to book via the GP Connect API.

<img src="documentation/images/gpc-appointment-checker-wireframe.png">

## Tech stack

  - .NET Core 3.1
    - Razor pages
    - hosted within Docker
  - Postgresql 11
    - stored functions
    - npgsql + dapper
    - patched with Flyaway 6.5.5
  - NHS UK frontend
  - NHS Mail authentication
  - Built according to progressive enhancement

## High level architecture

<img src="documentation/images/gpc-appointment-checker-high-level-architecture.png">

## User flow

<img src="documentation/images/gpc-appointment-checker-page-routing.png" width="65%">

## Build status

[![Build Actions Status](https://github.com/nhsconnect/gpconnect-appointment-checker/workflows/CI/badge.svg)](https://github.com/nhsconnect/gpconnect-appointment-checker/actions)

## Build and run

Create and patch the database:

```
[TODO]
```

Build the Docker image:

```
cd source
docker build -t gpconnect-appointment-checker:dev .
```

Run the Docker image, replacing the `PG_` variables with your Postgres database connection details:

```
docker run -d -p 8000:80 -e "ConnectionStrings:DefaultConnection=Server=PG_HOST;Port=PG_PORT;Database=PG_DBNAME;User Id=PG_USERID;Password=PG_PASSWORD" --name gpconnect-appointment-checker gpconnect-appointment-checker:dev
```

## Links

- [GP Connect API 1.2.7](https://developer.nhs.uk/apis/gpconnect-1-2-7/)
  - GP connect API calls
    - [Search for free slots](https://developer.nhs.uk/apis/gpconnect-1-2-7/appointments_use_case_search_for_free_slots.html)
    - [Get the FHIR capability statement](https://developer.nhs.uk/apis/gpconnect-1-2-7/foundations_use_case_get_the_fhir_capability_statement.html)
  - Spine integration
    - [Overview](https://developer.nhs.uk/apis/gpconnect-1-2-7/integration_illustrated.html)
    - [Creating the audit JWT](https://developer.nhs.uk/apis/gpconnect-1-2-7/integration_cross_organisation_audit_and_provenance.html)
    - [Spine Directory Service (SDS)](https://developer.nhs.uk/apis/gpconnect-1-2-7/integration_spine_directory_service.html)
    - [Spine Secure Proxy (SSP)](https://developer.nhs.uk/apis/gpconnect-1-2-7/integration_spine_secure_proxy.html)
- [NHS Mail sign sign on integration guide](https://s3-eu-west-1.amazonaws.com/comms-mat/Comms-Archive/NHSmail+Single+Sign-on+Technical+Guidance.pdf)
- [NHS UK frontend](https://github.com/nhsuk/nhsuk-frontend)
- [GOV.UK service manual - technology](https://www.gov.uk/service-manual/technology)
  - [Progressive enhancement](https://www.gov.uk/service-manual/technology/using-progressive-enhancement)
