﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.DAL;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "User Name is required!")]
    [StringLength(255, ErrorMessage = "User Name must be maximum {1} characters!")] //placeholder
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [StringLength(10, ErrorMessage = "Password must be maximum {1} characters!")]
    public string Password { get; set; }

    public bool IsActive { get; set; }

    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; }
}