﻿using System.Data;
using System.Net;
using System.Reflection.Metadata;

namespace OganiWebApp.Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public Basket? Basket { get; set; }
    }
}
