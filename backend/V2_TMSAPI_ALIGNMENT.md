# LogiTrack V2 — TmsApi V2 Alignment

This version keeps LogiTrack's delivery-management domain while applying the important implementation patterns found in the TmsApi V2 controllers.

## Implemented from TmsApi V2 patterns
- URL-segment API versioning: `/api/v2/...`
- `[ApiVersion("2.0")]` and ApiExplorer support
- Paginated GET responses with `data`, `meta`, and `links`
- Page clamping (`page >= 1`, `pageSize` 1–50)
- `AsNoTracking()` for read queries
- CancellationToken on async operations
- RFC 7807 `ProblemDetails` for important not-found / validation cases
- Rate limiting for authentication and write operations
- `Idempotency-Key` support for customer and delivery creation using an in-memory store, following the same in-memory/idempotency concept used by TmsApi's transcript endpoint
- Registration confirmation response remains explicit and does not automatically log the new user in

## Domain mapping
TmsApi's `CoursesController` pagination pattern is applied to LogiTrack collections such as customers, deliveries, drivers, vehicles, and payments. TmsApi's transcript-specific background queue is not copied because LogiTrack has no transcript domain; copying it literally would add unrelated functionality.

TmsApi's `CryptoDemoController` is likewise not copied as a public password-hashing demo. LogiTrack continues to use ASP.NET Core Identity for password hashing rather than exposing password hashes through an API endpoint.
