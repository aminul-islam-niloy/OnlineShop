## Online Shop Readme

## Introduction

Welcome to Online Shop! This project is a complete e-commerce website built using ASP.NET Core MVC with C# Identity Framework Core and Razor Pages. It provides a robust set of features including authentication, authorization, email services, payment methods, a shopping cart, order management, user management, multiple roles, and a dashboard for administrators.

## Features

### Authentication and Authorization

- Utilizes ASP.NET Core Identity Framework for user authentication and management.
- Users can register, login, and reset their passwords securely.
- Implements role-based authorization to control access to various parts of the application.

### Email Service

- Integrates with an email service for sending notifications, such as account confirmation emails and password reset emails.

### Payment Method

- Supports various payment methods for completing transactions securely.
- Integrates with popular payment gateways to handle online payments.

### Cart System

- Provides a user-friendly shopping cart for adding, updating, and removing items.
- Persists the cart items across sessions to ensure a seamless shopping experience.

### Order Management

- Allows users to review their orders, including order history and order details.
- Administrators can manage orders, process payments, and track shipments.

### User Management

- Administrators can manage users, including creating, editing, and deleting user accounts.
- Supports multiple user roles with different permissions and access levels.

### Dashboard

- Includes an admin dashboard for monitoring sales, managing products, and analyzing user activity.
- Provides comprehensive reporting features to track key performance metrics.

## Getting Started

To run the Online Shop locally, follow these steps:

1. **Clone the Repository**: Clone this repository to your local machine using git clone: https://github.com/aminul-islam-niloy/OnlineShop.git.

2. **Install Dependencies**: Navigate to the project directory and install the necessary dependencies using a package manager NuGet .


## Dependency Installation

**Entity Framework Core:**

Install the Entity Framework Core package via NuGet Package Manager Console or manage NuGet packages within Visual Studio.


Install-Package Microsoft.EntityFrameworkCore



**Entity Framework Core Design Tools:**

Install the EF Core design tools package to enable database migrations.


Install-Package Microsoft.EntityFrameworkCore.Tools


**ASP.NET Core Identity (if not included):**

Install ASP.NET Core Identity packages for user management.


Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore


## Database Migration

**DbContext Setup:**

Create a DbContext class representing the application's database context. Define entity sets and relationships within this class.

**Connection String Configuration:**

Configure the connection string in `appsettings.json` to specify the database provider and connection details.

**Initial Migration:**

Create an initial migration to scaffold the initial database schema.
dotnet ef migrations add InitialCreate


**Apply Migrations:**

Apply the migrations to create the database.

dotnet ef database update

**Additional Migrations:**

Create additional migrations whenever there are changes to the database schema.


dotnet ef migrations add <MigrationName>
dotnet ef database update

### By following these steps, you can successfully set up dependencies and perform database migrations in your ASP.NET Core MVC project using Entity Framework Core.


3. **Database Setup**: Configure the database connection string in the `appsettings.json` file. Run Entity Framework Core migrations to create the database schema.

4. **Configure Email Service**: Set up an email service provider and configure the SMTP settings in the `appsettings.json` file to enable email functionality.

5. **Configure Payment Gateway**: Integrate with a payment gateway provider and configure the necessary settings to enable online payments.

6. **Run the Application**: Build and run the application using Visual Studio or the .NET CLI. Navigate to the specified URL in your web browser to access the Online Shop.

## Contributing
We welcome contributions from the community! If you find any bugs or have suggestions for improvements, please open an issue or submit a pull request on GitHub.

## License
This project is licensed under the [MIT License](LICENSE).

## Acknowledgements
- ASP.NET Core Team for providing a powerful framework for building web applications.
- Microsoft Identity Team for developing the Identity Framework Core.
- Various open-source libraries and packages used in the development of this project.

