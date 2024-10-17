# Book Management Application - ASP.NET Core MVC

## Overview

This project is a **Book Management Application** built using **ASP.NET Core MVC**. It features a role-based access control system with three roles: **Admin**, **Author**, and **Reader**. Users can sign up and log in, with the default role being **Reader**. Depending on their role, users have varying levels of access to manage books and their profiles.

## Key Features

- **User Registration and Authentication**: Users can sign up and log in. The default role upon registration is **Reader**.
- **Role Management**: 
  - **Admin** can promote a **Reader** to an **Author** or demote an **Author** back to a **Reader**.
- **Book Management**: 
  - **Admins** and **Authors** can create, edit, and delete books.
  - **Readers** can only view books.
- **Profile Management**: 
  - All users can view and edit their profile details.
  - Users can also delete their accounts.

## Role-Based Functionalities

### Admin
- Promote **Readers** to **Authors**.
- Demote **Authors** to **Readers**.
- Create, edit, and delete books.

### Author
- Create, edit, and delete books.

### Reader
- View available books.

### Common for All Roles:
- View and update personal profile.
- Delete user account.
