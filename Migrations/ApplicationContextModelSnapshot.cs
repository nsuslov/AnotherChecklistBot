﻿// <auto-generated />
using System;
using AnotherChecklistBot.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AnotherChecklistBot.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("AnotherChecklistBot.Models.Checklist", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Secret")
                        .HasColumnType("TEXT");

                    b.Property<long>("SourceChatId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SourceMessageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Checklists");
                });

            modelBuilder.Entity("AnotherChecklistBot.Models.ChecklistMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChatId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChecklistId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MessageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ChecklistId");

                    b.ToTable("ChecklistMessages");
                });

            modelBuilder.Entity("AnotherChecklistBot.Models.ListItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Checked")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChecklistId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChecklistId");

                    b.ToTable("ListItems");
                });

            modelBuilder.Entity("AnotherChecklistBot.Models.ChecklistMessage", b =>
                {
                    b.HasOne("AnotherChecklistBot.Models.Checklist", "Checklist")
                        .WithMany("ChecklistMessages")
                        .HasForeignKey("ChecklistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Checklist");
                });

            modelBuilder.Entity("AnotherChecklistBot.Models.ListItem", b =>
                {
                    b.HasOne("AnotherChecklistBot.Models.Checklist", "Checklist")
                        .WithMany()
                        .HasForeignKey("ChecklistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Checklist");
                });

            modelBuilder.Entity("AnotherChecklistBot.Models.Checklist", b =>
                {
                    b.Navigation("ChecklistMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
