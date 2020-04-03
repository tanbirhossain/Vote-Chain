﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Voting.Model.Context;

namespace Voting.Model.Migrations
{
    [DbContext(typeof(BlockchainContext))]
    [Migration("20200124115613_DataJson")]
    partial class DataJson
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Voting.Model.Entities.Block", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.Property<int>("Difficulty");

                    b.Property<byte[]>("Hash");

                    b.Property<int>("Nonce");

                    b.Property<byte[]>("PreviousHash");

                    b.Property<long>("Timestamp");

                    b.HasKey("Id");

                    b.ToTable("Blocks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Data = "[]",
                            Difficulty = 2,
                            Hash = new byte[] { 76, 138, 47, 41, 137, 101, 32, 5, 136, 170, 19, 208, 175, 228, 48, 222, 168, 0, 52, 184, 228, 69, 124, 35, 140, 25, 214, 14, 199, 169, 202, 93 },
                            Nonce = 0,
                            Timestamp = 0L
                        });
                });

            modelBuilder.Entity("Voting.Model.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Voting.Model.Entities.TransactionInput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<byte[]>("Signature");

                    b.Property<int>("TransactionId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId")
                        .IsUnique();

                    b.ToTable("TransactionInputs");
                });

            modelBuilder.Entity("Voting.Model.Entities.TransactionOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CandidateAddress");

                    b.Property<string>("ElectionAddress");

                    b.Property<DateTime>("Timestamp");

                    b.Property<int>("TransactionId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("TransactionOutputs");
                });

            modelBuilder.Entity("Voting.Model.Entities.TransactionInput", b =>
                {
                    b.HasOne("Voting.Model.Entities.Transaction", "Transaction")
                        .WithOne("Input")
                        .HasForeignKey("Voting.Model.Entities.TransactionInput", "TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Voting.Model.Entities.TransactionOutput", b =>
                {
                    b.HasOne("Voting.Model.Entities.Transaction", "Transaction")
                        .WithMany("Outputs")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
