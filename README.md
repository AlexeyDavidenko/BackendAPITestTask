# BackendAPITestTask
Solution for the task to create API for Products with authentication

1. Please Download/Clone solution from repository and rebuild after downloading
2. Run migration script to create db and tables in developer console:
   a. update-database
3. Run Project.
4. Create admin and simple user in /api/Users/register
5. Login as an admin user in /api/Users/login and copy jwt Token for authentication
6. Check Get products in /api/Products for multiple and for single product in /api/Products/{id} where id is db id of product
7. To perform CRUD operations create (POST /api/Products), update (PUT /api/Products/{id}), delete (DELETE /api/Products/{id}) use token from step 5
