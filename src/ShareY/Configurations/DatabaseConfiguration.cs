﻿using ShareY.Database;

namespace ShareY.Configurations
{
    public sealed class DatabaseConfiguration : IDatabaseConfiguration
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }
}
