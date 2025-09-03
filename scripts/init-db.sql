-- Initialize database for PMS Backend
-- This script runs when the PostgreSQL container starts for the first time

-- Create the main database if it doesn't exist
SELECT 'CREATE DATABASE pms_backend_dev'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'pms_backend_dev')\gexec

-- Create the production database if it doesn't exist
SELECT 'CREATE DATABASE pms_backend'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'pms_backend')\gexec

-- Grant privileges to the user
GRANT ALL PRIVILEGES ON DATABASE pms_backend_dev TO pms_user;
GRANT ALL PRIVILEGES ON DATABASE pms_backend TO pms_user;

-- Create extensions that might be needed
\c pms_backend_dev;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pg_trgm";

\c pms_backend;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pg_trgm";
