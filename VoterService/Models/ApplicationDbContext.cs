using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoterService.Models
{
    public class ApplicationDbContext : DbContext
    {
        //private int nDistrictId;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //nDistrictId = nIdx;
        }
        public virtual DbSet<VoterModel> Voters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VoterModel>(entity =>
            {
                entity.ToTable("voter");

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasColumnType("int");

                entity.Property(e => e.userid)
                    .IsRequired()
                    .HasColumnName("userid")
                    .HasMaxLength(255);

                entity.Property(e => e.username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(255);

                entity.Property(e => e.nic)
                    .IsRequired()
                    .HasColumnName("nic")
                    .HasMaxLength(255);

                entity.Property(e => e.address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(255);

                entity.Property(e => e.district)
                    .HasColumnName("district")
                    .HasColumnType("int");

                entity.Property(e => e.province)
                    .HasColumnName("province")
                    .HasColumnType("int");

            });
        }
    }
}
