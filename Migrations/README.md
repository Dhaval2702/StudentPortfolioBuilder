# Migration/Seed helper

This repository includes `Seed1000.sql` which creates the `StudentProfiles` table and inserts 1000 sample records for filter/load testing.

## Run manually

```bash
sqlite3 studentportfolio.db < Migrations/Seed1000.sql
```

In the application startup, `EnsureCreated()` and app-level seeding are also enabled as a code-first fallback.
