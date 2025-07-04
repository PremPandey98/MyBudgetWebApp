# Feature & Functionality Change Summary

| Date       | Feature/Functionality                | Description/Change                                                                                   |
|------------|-------------------------------------|------------------------------------------------------------------------------------------------------|
| 2025-06-13 | User Profile Page                   | Users can view and update their name, email, and phone number.                                       |
| 2025-06-13 | Navbar Profile Dropdown             | Added profile icon with dropdown and link to profile page after login.                               |
| 2025-06-13 | Registration Page                   | Added editable "Name" field and improved mobile view for group options.                              |
| 2025-06-13 | Registration Group Option UI        | Replaced toggle with two side-by-side boxes for "Join Existing Group" and "Create New Group".        |
| 2025-06-14 | Database Migration                  | Ran EF Core migrations to update the new database with all latest tables and fields.                 |
| 2025-06-14 | BudgetUsage Table                   | Added new field `SpendBy` (string) to the BudgetUsage table and model.                               |
| 2025-06-14 | BudgetUsage Create Form             | Added "SpendBy" dropdown: shows all group users + "Team" for group accounts, only user for personal. |
| 2025-06-14 | Controller Logic                    | Updated controller to provide user list/account type for the SpendBy dropdown in the form.           |
