using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }                     // Primary Key
        public string FullName { get; set; }            // Employee name
        public string Email { get; set; }               // Unique email
        public string PasswordHash { get; set; }        // Hashed password
        public string Role { get; set; }                // super_admin, admin, manager
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
