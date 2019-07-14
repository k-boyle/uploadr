﻿using System;

namespace ShareY.Database.Models
{
    public sealed class Token
    {
        public Guid Guid { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
    }
}