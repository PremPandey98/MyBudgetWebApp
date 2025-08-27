# BudgetMobApp (BudgetWise)

## About This App
BudgetMobApp (also called BudgetWise) is a modern, mobile-friendly ASP.NET Core MVC web application for managing personal and group budgets. It supports user registration, login, profile management, group collaboration, and robust budget tracking with validation and admin controls.

## Key Features

- **User Registration & Login**
  - Register as a personal or group user
  - Join or create groups with group code
  - Strong validation for email, phone, and password
  - Login with username and password
  - "Remember Me" support

- **Profile Management**
  - View and update name, email, and phone number
  - Change password (with current password or OTP)
  - Profile icon and dropdown in navbar

- **Budget Tracking**
  - Add deposits and expenses (BudgetDeposit, BudgetUsage)
  - "Spend By" logic for group/personal accounts
  - View, filter, and manage all transactions
  - Mobile-friendly, modern UI

- **Validation**
  - Server-side and client-side validation for all forms
  - Phone number: 10 digits, starts with 6-9
  - Name: Only letters and spaces (no special characters)
  - Password: Minimum 6 characters

- **Password Management**
  - Change password from profile (requires current password)
  - Forgot password with OTP to registered mobile
  - Secure password reset flow

- **Group Functionality**
  - Join existing or create new groups
  - Group code required for joining
  - Group dashboard and group user selection for expenses

- **Admin Dashboard**
  - Separate admin login (static credentials)
  - View all users, groups, deposits, and usages
  - Import/export BudgetUsage (Excel)
  - Download sample import template
  - Admin-only access to management tools

## Usage

1. **Registration**
   - Choose Personal or Group account
   - Fill in required details (name, username, email, phone, password)
   - For group: join with code or create new group

2. **Login**
   - Enter username and password
   - Use "Show Password" toggle for convenience
   - Forgot password? Use OTP reset

3. **Profile**
   - Update your details anytime
   - Change password securely

4. **Budget Management**
   - Add deposits and expenses
   - For group users, select "Spend By" (yourself, team, or group member)
   - View and filter all transactions

5. **Admin**
   - Login as admin (`admin` / `admin123`)
   - Access dashboard to view/manage all data
   - Import/export BudgetUsage via Excel

## Technologies Used
- ASP.NET Core MVC (.NET 9)
- Entity Framework Core
- SQL Server
- Bootstrap, custom CSS (glassmorphism, responsive)
- jQuery (for validation and UI)

## Security & Validation
- All sensitive actions require authentication
- Admin dashboard is protected and separate
- All forms have both client-side and server-side validation
- No special characters allowed in Name fields
- Phone and email formats strictly enforced

## Customization
- Easily extendable for more features (reports, notifications, etc.)
- UI can be further themed or branded

## Getting Started
1. Clone the repo and open in Visual Studio
2. Update connection string in `appsettings.json`
3. Run EF Core migrations to set up the database
4. Build and run the app

---
For any issues or feature requests, please contact the developer or open an issue in the repository.
