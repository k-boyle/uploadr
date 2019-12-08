﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UploadR.Database;

namespace UploadR.Database.Migrations
{
    [DbContext(typeof(UploadRContext))]
    partial class UploadRContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("UploadR.Database.Models.Token", b =>
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

            modelBuilder.Entity("UploadR.Database.Models.Upload", b =>
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

                    b.Property<DateTime>("LastSeen")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_seen")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Password")
                        .HasColumnName("password");

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

            modelBuilder.Entity("UploadR.Database.Models.User", b =>
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

            modelBuilder.Entity("UploadR.Database.Models.Token", b =>
                {
                    b.HasOne("UploadR.Database.Models.User", "User")
                        .WithOne("Token")
                        .HasForeignKey("UploadR.Database.Models.Token", "UserGuid")
                        .HasConstraintName("fkey_token_userid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UploadR.Database.Models.Upload", b =>
                {
                    b.HasOne("UploadR.Database.Models.User", "Author")
                        .WithMany("Uploads")
                        .HasForeignKey("AuthorGuid")
                        .HasConstraintName("fkey_upload_authorid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
