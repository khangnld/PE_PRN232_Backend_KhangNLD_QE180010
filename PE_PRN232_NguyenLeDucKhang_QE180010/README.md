# Post Management Backend API

This is an ASP.NET Core Web API for managing posts.

## Setup Instructions

1. Create a `.env` file in the project root directory with the following content:
```
MONGODB_CONNECTION_STRING=mongodb+srv://khangnld:khang123@khangnld.py1tf5p.mongodb.net/
MONGODB_DATABASE_NAME=PRN232PE
SUPABASE_URL=https://sxajrpnrympynudlmxgs.supabase.co
SUPABASE_KEY=sb_secret_KVqdZMH0334sheB_J6jqdw_GAF3hBpF
ALLOWED_ORIGINS=http://localhost:3000
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

4. The API will be available at `http://localhost:5291`
5. Swagger UI will be available at `http://localhost:5291/swagger`

## Important Notes

- Make sure you have created a bucket named "image" in your Supabase Storage
- The bucket should be set to public for image access
- MongoDB connection string should include the database name or it will use the default database

## API Endpoints

- GET `/api/post` - Get all posts
- GET `/api/post/{id}` - Get post by ID
- POST `/api/post` - Create a new post (multipart/form-data)
- PUT `/api/post/{id}` - Update a post (multipart/form-data)
- DELETE `/api/post/{id}` - Delete a post
- GET `/api/post/search?name={name}` - Search posts by name
- GET `/api/post/sort?ascending={true|false}` - Sort posts by name

