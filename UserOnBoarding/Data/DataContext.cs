﻿using Microsoft.EntityFrameworkCore;

namespace UserOnBoarding.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();

    }
}
