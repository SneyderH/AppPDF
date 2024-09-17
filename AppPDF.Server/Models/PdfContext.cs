using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppPDF.Server.Models;

public partial class PdfContext : DbContext
{
    public PdfContext()
    {
    }

    public PdfContext(DbContextOptions<PdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PdfFile> PdfFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PdfFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PDF_FILE__3213E83F02A469AA");

            entity.ToTable("PDF_FILES");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Archivo)
                .IsUnicode(false)
                .HasColumnName("archivo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
