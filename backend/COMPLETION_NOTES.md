# LogiTrack completion based on the TmsApi reference project

The original LogiTrack project already contained the core Clean Architecture structure,
JWT/Identity authentication, FluentValidation, MediatR, PostgreSQL persistence, CRUD
operations for customers/drivers/vehicles/deliveries/payments, and a global exception
middleware.

The following missing reference-project capabilities were added/adapted:

- MediatR request logging behavior.
- API audit logging action filter.
- SignalR delivery tracking hub at `/hubs/delivery`.
- Real-time `deliveryUpdated` notification after a delivery status change.
- Stronger Identity lockout settings (5 failures / 15 minutes).
- Duplicate AuthController removed; the richer `api/auth` controller is retained.
- MediatR logging behavior is executed before validation.
- Existing Angular CORS policy remains enabled for `http://localhost:4200`.

Features from TmsApi that were intentionally NOT copied directly because they are
TMS-specific include student/course enrollment, grading, transcript workers, and
course caching. They should not be inserted into a delivery-management domain.

Note: the uploaded LogiTrack ZIP did not contain the Angular `logitrack-client` source,
so no Angular files were invented or overwritten. The backend completion is contained
in this archive.
