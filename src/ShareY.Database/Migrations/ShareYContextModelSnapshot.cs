﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShareY.Database;

namespace ShareY.Database.Migrations
{
    [DbContext(typeof(ShareYContext))]
    partial class ShareYContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ShareY.Database.Models.Token", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("guid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<bool>("Revoked")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("revoked")
                        .HasDefaultValue(false);

                    b.Property<int>("TokenType")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("token_type")
                        .HasDefaultValue(0);

                    b.Property<Guid>("UserGuid")
                        .HasColumnName("user_guid")
                        .HasColumnType("uuid");

                    b.HasKey("Guid")
                        .HasName("pk_token_guid");

                    b.HasIndex("UserGuid")
                        .IsUnique()
                        .HasName("index_user_id");

                    b.ToTable("tokens");
                });

            modelBuilder.Entity("ShareY.Database.Models.Upload", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("guid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorGuid")
                        .HasColumnName("author_guid")
                        .HasColumnType("uuid");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnName("content_type");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnName("file_name");

                    b.Property<bool>("Removed")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("removed")
                        .HasDefaultValue(false);

                    b.Property<long>("ViewCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("view_count")
                        .HasDefaultValue(0L);

                    b.HasKey("Guid")
                        .HasName("pk_upload_guid");

                    b.HasIndex("AuthorGuid");

                    b.ToTable("uploads");
                });

            modelBuilder.Entity("ShareY.Database.Models.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("guid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<bool>("Disabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("disabled")
                        .HasDefaultValue(false);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email");

                    b.HasKey("Guid")
                        .HasName("pk_user_guid");

                    b.ToTable("users");
                });

            modelBuilder.Entity("ShareY.Database.Models.Token", b =>
                {
                    b.HasOne("ShareY.Database.Models.User", "User")
                        .WithOne("Token")
                        .HasForeignKey("ShareY.Database.Models.Token", "UserGuid")
                        .HasConstraintName("fkey_token_userid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ShareY.Database.Models.Upload", b =>
                {
                    b.HasOne("ShareY.Database.Models.User", "Author")
                        .WithMany("Uploads")
                        .HasForeignKey("AuthorGuid")
                        .HasConstraintName("fkey_upload_authorid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
