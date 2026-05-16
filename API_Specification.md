# Delly Belly API Specification

This document provides a comprehensive specification of all REST endpoints available in the Delly Belly backend API. The API is built using ASP.NET Core Clean Architecture.

---

## 🔐 1. Customer Authentication (`/api/customer-auth`)
Handles all authentication flows for regular customers, including OTP verification and Google SSO.

| Method | Endpoint | Description |
|:---|:---|:---|
| **POST** | `/api/customer-auth/login` | Authenticate customer with email and password. Returns JWT token. |
| **POST** | `/api/customer-auth/google-login` | Authenticate using Google SSO Token (`Credential`). Returns JWT. |
| **POST** | `/api/customer-auth/register` | Register a new customer and trigger OTP email. |
| **POST** | `/api/customer-auth/verify-otp` | Verify 6-digit OTP code to activate customer account. |
| **POST** | `/api/customer-auth/resend-otp` | Resend a new OTP to the registered customer email. |
| **POST** | `/api/customer-auth/check-email` | Check if a customer email is already registered in the DB. |
| **POST** | `/api/customer-auth/forgot-password` | Send a password reset link to the customer email. |
| **POST** | `/api/customer-auth/reset-password` | Set a new password using a valid reset token. |
| **GET** | `/api/customer-auth/me` | Fetch authenticated customer's profile using JWT token. |
| **PUT** | `/api/customer-auth/profile` | Update the authenticated customer's profile details. |

---

## 🛡️ 2. Admin Authentication (`/api/auth`)
Handles authentication and session management exclusively for Admin CMS users.

| Method | Endpoint | Description |
|:---|:---|:---|
| **POST** | `/api/auth/login` | Authenticate admin staff/super-admin. Returns JWT. |
| **POST** | `/api/auth/register` | Register a new admin user (requires subsequent OTP). |
| **POST** | `/api/auth/verify-otp` | Verify admin account via OTP. |
| **POST** | `/api/auth/resend-otp` | Resend verification OTP to admin email. |
| **POST** | `/api/auth/forgot-password` | Trigger an admin password reset email. |
| **POST** | `/api/auth/reset-password` | Reset admin password using the provided token. |
| **GET** | `/api/auth/me` | Fetch current authenticated admin user profile. |

---

## 👥 3. Admin User Management (`/api/admin-users`)
Endpoints for super-admins to manage staff accounts and roles.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/admin-users` | Retrieve all registered admin users. |
| **GET** | `/api/admin-users/{id}` | Retrieve details of a specific admin user. |
| **POST** | `/api/admin-users` | Create a new admin user directly (Super Admin only). |
| **PUT** | `/api/admin-users/{id}` | Update admin details and roles. |
| **DELETE** | `/api/admin-users/{id}` | Delete an admin account permanently. |
| **PATCH** | `/api/admin-users/{id}/status` | Toggle admin account active/inactive status. |

---

## 🛍️ 4. Product Catalog (`/api/products`)
Core operations for the bakery product menu.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/products` | Get all products. |
| **GET** | `/api/products/paged` | Get products with pagination support. |
| **GET** | `/api/products/{id}` | Retrieve a specific product including its image blobs and relations. |
| **POST** | `/api/products` | Create a new product entry. |
| **PUT** | `/api/products/{id}` | Update existing product details. |
| **DELETE** | `/api/products/{id}` | Delete a product from the catalog. |
| **POST** | `/api/products/{id}/upload-photo` | Upload a new photo to a product. |
| **GET** | `/api/products/{id}/photo/{imageId}` | Retrieve binary data for a specific product photo. |
| **PUT** | `/api/products/{id}/update-photo/{imageId}` | Replace a specific product photo. |
| **DELETE** | `/api/products/{id}/delete-photo/{imageId}` | Remove a photo from a product. |
| **PUT** | `/api/products/{id}/set-primary/{imageId}` | Set a specific photo as the product's primary image. |

---

## 📁 5. Categories (`/api/categories`)
Product categorization and section management.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/categories` | Retrieve all categories. |
| **GET** | `/api/categories/{id}` | Fetch specific category details. |
| **POST** | `/api/categories` | Create a new menu category. |
| **PUT** | `/api/categories/{id}` | Update category details. |
| **DELETE** | `/api/categories/{id}` | Delete a category. |
| **POST** | `/api/categories/{id}/upload-photo` | Upload the category cover photo. |
| **GET** | `/api/categories/{id}/photo` | Retrieve binary image data for the category cover. |
| **PUT** | `/api/categories/{id}/update-photo` | Replace the category cover photo. |
| **DELETE** | `/api/categories/{id}/delete-photo` | Remove the category cover photo. |

---

## 🌾 6. Ingredients (`/api/ingredients`)
Manage product ingredients (e.g., Eggless, Gluten-Free, Buttercream).

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/ingredients` | Get all global ingredients. |
| **GET** | `/api/ingredients/by-category/{categoryId}` | Fetch all ingredients linked to a specific category. |
| **GET** | `/api/ingredients/{id}` | Fetch specific ingredient details. |
| **POST** | `/api/ingredients` | Create a new ingredient tag. |
| **PUT** | `/api/ingredients/{id}` | Update ingredient name/details. |
| **DELETE** | `/api/ingredients/{id}` | Delete an ingredient permanently. |

---

## 🛒 7. Customer Wishlist (`/api/wishlist`)
Endpoints for logged-in customers to manage their saved items.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/wishlist` | Get all products currently in the user's wishlist. |
| **POST** | `/api/wishlist/toggle/{productId}` | Add or remove a product from the wishlist. |
| **DELETE** | `/api/wishlist/clear` | Remove all items from the wishlist. |

---

## 📍 8. Customer Addresses (`/api/customer/addresses`)
Address book management for the Custom Order and Checkout flows.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/customer/addresses` | Fetch all saved addresses for the authenticated customer. |
| **POST** | `/api/customer/addresses` | Add a new address (Home, Work, etc.). |
| **DELETE** | `/api/customer/addresses/{id}` | Remove a saved address. |
| **PUT** | `/api/customer/addresses/{id}/default` | Set a specific address as the primary/default choice. |

---

## 📸 9. Public Gallery (`/api/gallery`)
Manage the global image gallery.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/gallery` | Retrieve all gallery items. |
| **GET** | `/api/gallery/debug-count` | Diagnostic endpoint for total image count. |
| **GET** | `/api/gallery/{id}` | Get specific gallery item details. |
| **GET** | `/api/gallery/{id}/photo` | Retrieve the physical binary of a gallery photo. |
| **POST** | `/api/gallery` | Upload a new gallery item. |
| **PUT** | `/api/gallery/{id}` | Update gallery item metadata (e.g. caption). |
| **DELETE** | `/api/gallery/{id}` | Delete a gallery item. |

---

## 💬 10. Feedback & Reviews (`/api/feedback`)
Customer reviews and application feedback system.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/feedback/categories` | Retrieve valid feedback categories/types. |
| **POST** | `/api/feedback` | Submit new customer feedback or product review. |

---

## 💌 11. Newsletter & Marketing (`/api/newsletter`)
Manage email subscribers and bulk broadcasts.

| Method | Endpoint | Description |
|:---|:---|:---|
| **POST** | `/api/newsletter/subscribe` | Add a customer email to the subscription list. |
| **POST** | `/api/newsletter/broadcast` | Send a mass email broadcast to all subscribers. |
| **GET** | `/api/newsletter/quota` | Get SMTP/Email provider daily sending quota status. |

---

## 📞 12. Contact & Inquiries (`/api/contact`)
Handle contact form submissions.

| Method | Endpoint | Description |
|:---|:---|:---|
| **POST** | `/api/contact/send` | Submit a general inquiry or custom order request via the contact form. |

---

## ⚙️ 13. Site Settings (`/api/settings`)
Global configuration and metadata for the storefront.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/settings` | Retrieve public site configuration (Phone numbers, socials, metadata). |
| **PUT** | `/api/settings` | Update site configuration (Admin only). |

---

## 🏠 14. Home Dashboard (`/api/home`)
Aggregate endpoints optimized for the public storefront homepage.

| Method | Endpoint | Description |
|:---|:---|:---|
| **GET** | `/api/home/bestsellers` | Retrieve top-selling products for the homepage carousel. |
| **GET** | `/api/home/recommended` | Retrieve recommended or curated products. |
