# 🛒 E-Commerce Web Application

This is a full-stack **ASP.NET Core MVC E-Commerce Web Application** that supports role-based access control, product management, shopping cart functionality, and order processing.

<img width="1919" height="974" alt="image" src="https://github.com/user-attachments/assets/fe2655f9-c647-4e6c-9fc1-c328616164cd" />

---

## Features

### 👤 Authentication & Authorization
- User registration and login system
- Role-based access control
- Roles included:
  - **Admin**
  - **Manager**
  - **User**

### 🛠️ Admin Features
- Manage users and assign roles
- Full control over system data
- Add, edit, and delete roles

### 📦 Product Management (Admin & Manager)
- Create new products
- Update product details
- Delete products
- Manage inventory

### 🛍️ User Features
- Browse available products
- View product details
- Add products to shopping cart
- Place orders

### 🛒 Shopping Cart
- Add/remove products
- Update quantities
- View total price

### 📑 Orders
- Create orders from cart
- View order history

---

## Project Structure

- **Controllers/** → Application logic and request handling  
- **Models/** → Database entities  
- **ViewModels/** → Data transfer between views and backend  
- **Views/** → Razor UI pages  
- **wwwroot/** → Static files (CSS, JS, images)  
- **Migrations/** → Database migrations (Entity Framework Core)  
- **Properties/** → Project configuration  

---

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Identity (Authentication & Authorization)
- HTML, CSS, JavaScript
- Bootstrap

---

## 📌 How to Run the Project

1. Clone the repository
```bash
git clone https://github.com/ibtihel11/DotnetWebsite.git
